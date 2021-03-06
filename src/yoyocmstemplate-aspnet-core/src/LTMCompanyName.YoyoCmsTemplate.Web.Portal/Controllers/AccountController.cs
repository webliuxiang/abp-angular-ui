using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Notifications;
using Abp.Runtime.Security;
using Abp.Threading;
using Abp.Timing;
using Abp.UI;
using Abp.Web.Models;
using Abp.Zero.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Notifications.MailManage;
using LTMCompanyName.YoyoCmsTemplate.Security.Captcha;
using LTMCompanyName.YoyoCmsTemplate.Url;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Portal.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.String;
using StringExtensions = L._52ABP.Common.Extensions.StringExtensions;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Controllers
{
    public class AccountController : YoyoCmsTemplateControllerBase
    {
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly IAccountAppService _accountAppService;

        private readonly ExternalLoginInfoManagerFactory _externalLoginInfoManagerFactory;
        private readonly LogInManager _logInManager;
        private readonly IMailManager _mailManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly SignInManager _signInManager;

        private readonly ITenantCache _tenantCache;
        private readonly TenantManager _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly UserManager _userManager;
        private readonly UserRegistrationManager _userRegistrationManager;

        public AccountController(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            TenantManager tenantManager,
            IUnitOfWorkManager unitOfWorkManager,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            LogInManager logInManager,
            SignInManager signInManager,
            UserRegistrationManager userRegistrationManager,
            ITenantCache tenantCache,
            INotificationPublisher notificationPublisher,
            IMailManager mailManager,
            ExternalLoginInfoManagerFactory externalLoginInfoManagerFactory, IAccountAppService accountAppService)
        {
            _userManager = userManager;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _userRegistrationManager = userRegistrationManager;

            _tenantCache = tenantCache;
            _notificationPublisher = notificationPublisher;

            _mailManager = mailManager;

            _externalLoginInfoManagerFactory = externalLoginInfoManagerFactory;
            _accountAppService = accountAppService;
        }

        #region Etc

        /// <summary>
        ///     This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        ///     Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );

            return Content("Sent notification: " + message);
        }

        #endregion Etc

        /// <summary>
        ///     ????????????
        /// </summary>
        /// <param name="returnUrl">??????????????????</param>
        /// <param name="actionResult">????????????action</param>
        /// <returns></returns>
        private bool LoginCheck(string returnUrl, out IActionResult actionResult)
        {
            actionResult = null;
            if (AbpSession.UserId.HasValue)
            {
                actionResult = Redirect(returnUrl.IsNullOrWhiteSpace() ? "/" : returnUrl);

                return true;
            }

            return false;
        }

        /// <summary>
        ///     ?????????????????????Code
        /// </summary>
        /// <param name="title">????????????</param>
        /// <param name="contentTemplate">????????????</param>
        /// <param name="user">??????</param>
        /// <returns>?????????Code</returns>
        private async Task<string> SendMailAndReturnCode(string title, string contentTemplate, User user)
        {
            var code = GenerateActivationCode(user);

            await SendActivationEmail(title, contentTemplate, user, code);

            return code;
        }

        private async Task SendActivationEmail(string title, string contentTemplate, User user, string code)
        {
            var webSiteClientRootAddress = IocManager.Instance.Resolve<IAppConfigurationAccessor>().GetWebSiteClientRootAddress();

            await _mailManager.SendMessage(
                user.EmailAddress, title,
                Format(contentTemplate, webSiteClientRootAddress, code)
            );
        }

        private string GenerateActivationCode(User user)
        {
            var code = $"{Guid.NewGuid()}{user.Id}{user.UserName}";

            code = StringExtensions.ToMd5Hash(code);
            return code;
        }

        #region ?????? / ??????

        /// <summary>
        ///     ????????????
        /// </summary>
        /// <param name="userNameOrEmailAddress"></param>
        /// <param name="returnUrl"></param>
        /// <param name="successMessage"></param>
        /// <returns></returns>
        public async Task<IActionResult> Login(string userNameOrEmailAddress = "", string returnUrl = "",
            string successMessage = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            // ????????????????????????????????????
            if (LoginCheck(returnUrl, out var redirect))
            {
                return redirect;
            }

            //var ee=AsyncHelper.RunSync(() => _signInManager.GetExternalLoginInfoAsync());

            var loginProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(s => !s.DisplayName.IsNullOrWhiteSpace())
                .ToList();

            if (IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = GetAppHomeUrl();
            }

            return View(new LoginFormViewModel
            {
                ReturnUrl = returnUrl,
                IsSelfRegistrationAllowed = IsSelfRegistrationEnabled(), // ??????????????????

                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled //????????????????????????
            });
        }

        /// <summary>
        ///     ??????API
        /// </summary>
        /// <param name="loginModel">????????????</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel)
        {
            //?????????????????????
            //    this.ValuedationCode(CaptchaType.TenantUserLogin, loginModel.Code);

            await VerifyImgCodeAsync(CaptchaType.TenantUserLogin, loginModel.Code);

            loginModel.ReturnUrl = NormalizeReturnUrl(loginModel.ReturnUrl);
            if (!IsNullOrWhiteSpace(loginModel.ReturnUrlHash))
            {
                loginModel.ReturnUrl += loginModel.ReturnUrlHash;
            }

            /*
             ????????? loginResult.Identity.Claims ??????Core??? UserClaimsPrincipalFactory????????????Claim,
             ??????????????? ?????????Claim????????????????????????
             */
            var loginResult = await GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password,
                GetTenancyNameOrNull());

            // TOTO:?????????????????????
            //if (!loginResult.User.IsEmailConfirmed)
            //{
            //    throw new UserFriendlyException("");
            //}

            await _signInManager.SignOutAndSignInAsync(loginResult.Identity, loginModel.RememberMe);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Json(new AjaxResponse { TargetUrl = loginModel.ReturnUrl });
        }

        /// <summary>
        ///     ????????????
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        /// <summary>
        ///     ??????????????????
        /// </summary>
        /// <param name="usernameOrEmailAddress"></param>
        /// <param name="password"></param>
        /// <param name="tenancyName"></param>
        /// <returns></returns>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress,
            string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result,
                        usernameOrEmailAddress, tenancyName);
            }
        }

        #endregion ?????? / ??????

        #region ??????

        /// <summary>
        ///     ??????- ????????????
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            // ????????????????????????
            if (LoginCheck("/", out var redirect))
            {
                return redirect;
            }

            var model = new RegisterViewModel();
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return View(model);
        }

        /// <summary>
        ///     ??????- ????????????
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {


            throw new UserFriendlyException("?????????????????????????????????");
#pragma warning disable CS0162 // ??????????????????????????????
            ExternalLoginInfo externalLoginInfo = null;

            if (model.IsExternalLogin)
            {
                //?????????bug???????????????
                externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    throw new Exception("Can not external login!");
                }

                using (var providerManager =
                    _externalLoginInfoManagerFactory.GetExternalLoginInfoManager(externalLoginInfo.LoginProvider))
                {
                    model.UserName =
                        providerManager.Object.GetUserNameFromClaims(externalLoginInfo.Principal.Claims.ToList());
                }

                model.Password = await _userManager.CreateRandomPassword();
            }
            else
            {
                await VerifyImgCodeAsync(CaptchaType.TenantUserRegister, model.Code);

                if (model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                {
                    throw new UserFriendlyException(L("FormIsNotValidMessage"));
                }
            }

            var user = await _userRegistrationManager.RegisterAsync(
                model.UserName,
                model.EmailAddress,
                model.UserName,
                model.Password,
                false // ??????????????????????????????????????????????????????????????????????????????????????????????????????
            );

            // ??????????????????
            var isEmailConfirmationRequiredForLogin =
                await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                    .IsEmailConfirmationRequiredForLogin);

            await _unitOfWorkManager.Current.SaveChangesAsync();
            Debug.Assert(user.TenantId != null);//?????????????????????Id???????????????
            var tenant = await _tenantManager.GetByIdAsync(user.TenantId.Value);

            // ???????????????????????????
            if (user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin))
            {
                AbpLoginResult<Tenant, User> loginResult;
                if (externalLoginInfo != null)
                {
                    loginResult = await _logInManager.LoginAsync(externalLoginInfo, tenant.TenancyName);
                }
                else
                {
                    loginResult = await GetLoginResultAsync(user.UserName, model.Password, tenant.TenancyName);
                }

                if (model.IsExternalLogin)
                {
                    // ???????????????????????????, ????????????
                    user.Logins = new List<UserLogin>
                    {
                        new UserLogin
                        {
                            LoginProvider = externalLoginInfo.LoginProvider,
                            ProviderKey = externalLoginInfo.ProviderKey,
                            TenantId = user.TenantId
                        }
                    };
                    await _unitOfWorkManager.Current.SaveChangesAsync();

                    await _signInManager.SignInAsync(loginResult.Identity, false);
                    return Redirect("/Home/Index");
                }

                if (loginResult.Result == AbpLoginResultType.Success)
                {
                    await _signInManager.SignInAsync(loginResult.Identity, false);
                    return Json("/Home/Index");
                }
            }

            var activeCode = GenerateActivationCode(user);
            if (!model.IsExternalLogin)
            {
                // ??????????????????
                await SendActivationEmail("52ABP?????? - ??????????????????", "???????????????????????????: {0}Account/RegisterResult?code={1}", user,
                    activeCode);

                return Json($"/Account/RegisterByLink?u={user.UserName}&e={user.EmailAddress}");
            }

            // ???????????????????????????, ?????????code??????
            user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalLoginInfo.LoginProvider,
                    ProviderKey = externalLoginInfo.ProviderKey,
                    TenantId = user.TenantId
                }
            };
            user.EmailConfirmationCode = activeCode;
            await _unitOfWorkManager.Current.SaveChangesAsync();

            return Redirect(Url.Action("RegisterResult", new { Code = activeCode }));
#pragma warning restore CS0162 // ??????????????????????????????
        }

        /// <summary>
        ///     ??????- ????????????,????????????????????????
        /// </summary>
        /// <param name="u"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task<ActionResult> RegisterByLink(string u, string e)
        {
            await Task.Yield();
            var viewModel = new RegisterResultViewModel { UserName = u, EmailAddress = e };
            return View(viewModel);
        }

        /// <summary>
        ///     ??????- ??????????????????API
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ActionResult> RegisterResult(string code)
        {
            // ?????? resetcode??????
            var user = await _userManager.GetUserByEmailConfirmationCode(code);
            if (user == null)
            {
                return View(false);
            }

            user.IsEmailConfirmed = true;
            user.IsActive = true;
            user.EmailConfirmationCode = Empty;
            await _userManager.UpdateAsync(user);

            // ??????????????????,????????????
            var identity = await _logInManager.LoginAsync(user);
            await _signInManager.SignOutAndSignInAsync(identity, false);

            return View(true);
        }

        /// <summary>
        ///     ??????- ???????????????????????????API
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ReSendEmailConfirmationCode([FromBody] EmailConfirmationViewModel input)
        {
            // ???????????????
            //   ValuedationCode(VerificationImgType.RegisterActive, input.Code);

            await VerifyImgCodeAsync(CaptchaType.TenantUserRegisterActiveEmail, input.Code);


            // ?????? resetcode??????
            var user = _userManager.Users
                .Where(o => o.EmailAddress == input.EmailAddress)
                .FirstOrDefault();
            if (user == null)
            {
                return new JsonResult(false);
            }

            if (user.IsEmailConfirmed || user.IsActive)
            {
                throw new UserFriendlyException("?????????????????????!");
            }

            // ???????????????
            var activeCode = await SendMailAndReturnCode("52ABP?????? - ??????????????????",
                "???????????????????????????: {0}Account/RegisterResult?code={1}", user);

            user.EmailConfirmationCode = activeCode;
            await _userManager.UpdateAsync(user);

            return new JsonResult(true);
        }

        private bool IsSelfRegistrationEnabled()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return false;
            }

            return true;
        }

        #endregion ??????

        #region ????????????

        /// <summary>
        ///     ??????????????????
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ForgotPassword()
        {
            // ????????????????????????
            if (LoginCheck("/", out var redirect))
            {
                return redirect;
            }

            await Task.Yield();
            return View();
        }

        /// <summary>
        ///     ????????????Api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ForgotPassword(ForgotPasswordViewModel input)
        {
            // ???????????????
            //   ValuedationCode(VerificationImgType.ForotPassword, input.Code);
            await VerifyImgCodeAsync(CaptchaType.TenantUserForotPassword, input.Code);


            var webSiteClientRootAddress = IocManager.Instance.Resolve<IAppConfigurationAccessor>().GetWebSiteClientRootAddress();


            webSiteClientRootAddress += "Account/ForgotPasswordReset?c={resetCode}";

            await _accountAppService.SendPasswordResetCode(new SendPasswordResetCodeInput
            {
                EmailAddress = input.Email,
                link = webSiteClientRootAddress
            });


            return Json(new AjaxResponse("/Account/ForgotPasswordByLink"));




        }

        /// <summary>
        ///     ????????????- ??????????????????????????????,????????????????????????
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ForgotPasswordByLink()
        {
            await Task.Yield();
            return View();
        }

        /// <summary>
        /// ????????????- ?????????????????????
        /// </summary>
        /// <param name="c">??????c?????????????????????</param>
        /// <returns></returns>
        public async Task<ActionResult> ForgotPasswordReset(string c)
        {
            await Task.Yield();
            //??????????????????????????? 
            ViewBag.Code = Uri.UnescapeDataString(c);



            return View();
        }

        /// <summary>
        ///     ????????????- ??????????????????
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordInput input)
        {
            // ???????????????
            await VerifyImgCodeAsync(CaptchaType.TenantUserResetPassword, input.Code);



            // ???????????????????????????
            if (input.NewPassword != input.ConfirmPassword)
            {
                throw new UserFriendlyException("???????????????????????????");
            }


            //????????????????????????/???????????????
            input.ResetCode = SimpleStringCipher.Instance.Decrypt(input.ResetCode);//????????????
            var query = HttpUtility.ParseQueryString(input.ResetCode);
            input.ResetCode = query["c"];

            var output = await _accountAppService.ResetPasswordAsync(input);

            if (output.CanLogin)
            {
                var user = await _userManager.GetUserByEmail(output.EmailAddress);
                // ??????????????????,????????????
                var identity = await _logInManager.LoginAsync(user);
                //??????????????????????????????????????????
                await _signInManager.SignOutAndSignInAsync(identity, false);

            }


            return new JsonResult(true);

        }

        #endregion ????????????

        #region External Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(
                "ExternalLoginCallback",
                "Account",
                new { ReturnUrl = returnUrl, authSchema = provider });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [UnitOfWork]
        public virtual async Task<IActionResult> ExternalLoginCallback(string returnUrl, string remoteError = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (remoteError != null)
            {
                //Logger.Error("Remote Error in ExternalLoginCallback: " + remoteError);
                Logger.Error("ExternalLoginCallback????????????????????????: " + remoteError);
                throw new UserFriendlyException(L("CouldNotCompleteLoginOperation"));
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                Logger.Warn("????????????????????????????????????");

                //Logger.Warn("Could not get information from external login.");
                return RedirectToAction(nameof(Login));
            }

            await _signInManager.SignOutAsync();

            var tenancyName = GetTenancyNameOrNull();

            var loginResult = await _logInManager.LoginAsync(externalLoginInfo, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    await _signInManager.SignInAsync(loginResult.Identity, false);
                    return Redirect(returnUrl);

                case AbpLoginResultType.UnknownExternalLogin:
                    return await RegisterForExternalLogin(externalLoginInfo);

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                        loginResult.Result,
                        externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ?? externalLoginInfo.ProviderKey,
                        tenancyName
                    );
            }
        }

        private async Task<IActionResult> RegisterForExternalLogin(ExternalLoginInfo externalLoginInfo)
        {
            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ??
                        UserManagerment.Users.User.CreateRandomEmail();
            var nameinfo =
                ExternalLoginInfoHelper.GetNameAndSurnameFromClaims(externalLoginInfo.Principal.Claims.ToList());

            var viewModel = new RegisterViewModel
            {
                EmailAddress = email,
                //Name = nameinfo.name,
                //Surname = nameinfo.surname,
                IsExternalLogin = true,
                ExternalLoginAuthSchema = externalLoginInfo.LoginProvider
            };

            if (nameinfo.name != null &&
                nameinfo.surname != null &&
                email != null)
            {
                return await Register(viewModel);
            }

            return await Register(viewModel);
        }

        [UnitOfWork]
        protected virtual async Task<List<Tenant>> FindPossibleTenantsOfUserAsync(UserLoginInfo login)
        {
            List<User> allUsers;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                allUsers = await _userManager.FindAllAsync(login);
            }

            return allUsers
                .Where(u => u.TenantId != null)
                .Select(u => AsyncHelper.RunSync(() => _tenantManager.FindByIdAsync(u.TenantId.Value)))
                .ToList();
        }

        #endregion External Login

        #region Helpers

        public ActionResult RedirectToAppHome()
        {
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        ///     ???????????????
        /// </summary>
        /// <returns></returns>
        public string GetAppHomeUrl()
        {
            return Url.Action("Index", "Home");
        }

        #endregion Helpers

        #region Common

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        /// <summary>
        ///     ???ReturnURL?????????
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="defaultValueBuilder"></param>
        /// <returns></returns>
        private string NormalizeReturnUrl(string returnUrl, Func<string> defaultValueBuilder = null)
        {
            if (defaultValueBuilder == null)
            {
                defaultValueBuilder = GetAppHomeUrl;
            }

            if (returnUrl.IsNullOrEmpty())
            {
                return defaultValueBuilder();
            }

            //?????????????????????
            return Url.IsLocalUrl(returnUrl) ? returnUrl : defaultValueBuilder();
        }

        #endregion Common
    }
}
