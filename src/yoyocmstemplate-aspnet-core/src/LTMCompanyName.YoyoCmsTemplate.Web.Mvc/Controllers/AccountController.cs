using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Abp;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Notifications;
using Abp.Threading;
using Abp.Timing;
using Abp.UI;
using Abp.Web.Models;
using Abp.Zero.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Helpers.Debugging;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Notifications.MailManage;
using LTMCompanyName.YoyoCmsTemplate.Security.Captcha;
using LTMCompanyName.YoyoCmsTemplate.Security.PasswordComplexity;
using LTMCompanyName.YoyoCmsTemplate.Sessions;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Models.Account;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Views.Shared.Components.TenantChange;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Controllers
{
    public class AccountController : YoyoCmsTemplateControllerBase
    {
        private readonly UserManager _userManager;
        private readonly TenantManager _tenantManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly LogInManager _logInManager;
        private readonly SignInManager _signInManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly ISessionAppService _sessionAppService;
        private readonly ITenantCache _tenantCache;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IPasswordComplexitySettingStore _passwordComplexitySettingStore;
        private readonly IMailManager _mailManager;

        public AccountController(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            TenantManager tenantManager,
            IUnitOfWorkManager unitOfWorkManager,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            LogInManager logInManager,
            SignInManager signInManager,
            UserRegistrationManager userRegistrationManager,
            ISessionAppService sessionAppService,
            ITenantCache tenantCache,
            INotificationPublisher notificationPublisher, IPasswordComplexitySettingStore passwordComplexitySettingStore, IMailManager mailManager)
        {
            _userManager = userManager;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _userRegistrationManager = userRegistrationManager;
            _sessionAppService = sessionAppService;
            _tenantCache = tenantCache;
            _notificationPublisher = notificationPublisher;
            _passwordComplexitySettingStore = passwordComplexitySettingStore;
            _mailManager = mailManager;
        }

        #region 登录/注销功能

        public ActionResult Login(string userNameOrEmailAddress = "", string returnUrl = "", string successMessage = "")
        {
            if (AbpSession.UserId.HasValue)
            {
                return Redirect(GetAppHomeUrl());
            }

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = GetAppHomeUrl();
            }

            return View(new LoginFormViewModel
            {
                ReturnUrl = returnUrl,
                SuccessMessage = successMessage,
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                IsSelfRegistrationAllowed = IsSelfRegistrationEnabled(),
                UserNameOrEmailAddress = userNameOrEmailAddress,
                IsTenantSelfRegistrationEnabled = IsTenantSelfRegistrationEnabled(),
            });
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            AbpLoginResult<Tenant, User> loginResult = await 
            GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password, GetTenancyNameOrNull());

            await _signInManager.SignInAsync(loginResult.Identity, loginModel.RememberMe);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Json(new AjaxResponse { TargetUrl = returnUrl });
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            AbpLoginResult<Tenant, User> loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        #endregion 登录/注销功能

        #region 是否允许注册租户/用户

        /// <summary>
        /// 检查是否允许注册新账号
        /// </summary>
        private void CheckSelfRegistrationIsEnabled()
        {
            if (!IsSelfRegistrationEnabled())
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        /// <summary>
        /// 是否允许创建新账号
        /// </summary>
        /// <returns></returns>
        private bool IsSelfRegistrationEnabled()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return false; // 宿主不允许进行自己注册
            }

            return SettingManager.GetSettingValue<bool>(AppSettingNames.UserManagement.AllowSelfRegistration);
        }

        /// <summary>
        /// 是否允许注册租户
        /// </summary>
        /// <returns></returns>
        private bool IsTenantSelfRegistrationEnabled()
        {
            if (AbpSession.TenantId.HasValue)
            {
                return false;
            }

            return SettingManager.GetSettingValue<bool>(AppSettingNames.HostSettings.AllowSelfRegistration);
        }

        #endregion 是否允许注册租户/用户

        #region 忘记密码/重制密码

        public IActionResult ForgotPassword()
        {
            if (AbpSession.UserId.HasValue)
            {
                return Redirect(GetAppHomeUrl());
            }

            return View();
        }

        /// <summary>
        /// 忘记密码Api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ForgotPassword(ForgotPasswordViewModel input)
        {
            // 校验验证码
            //  ValuedationCode(VerificationImgType.ForotPassword, input.Code);
            await VerifyImgCodeAsync(CaptchaType.TenantUserForotPassword, input.Code);

            User user = await _userManager.GetUserByEmail(input.Email);

            if (user != null)
            {
                string resetCode = await SendMailAndReturnCode("52ABP官方 - 重置密码邮件", "请点击链接重置密码: {0}Account/ForgotPasswordReset?code={1}", user);
                user.PasswordResetCode = resetCode;
                await _userManager.UpdateAsync(user);
            }

            return new JsonResult($"/Account/ForgotPasswordByLink");
        }

        #endregion 忘记密码/重制密码

        #region 发送邮件

        /// <summary>
        /// 发送邮件并返回Code
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="contentTemplate">内容模板</param>
        /// <param name="user">用户</param>
        /// <returns>生成的Code</returns>
        private async Task<string> SendMailAndReturnCode(string title, string contentTemplate, User user)
        {
            string code = $"{Guid.NewGuid().ToString()}{user.Id}{user.UserName}".ToMd5();

            string webSiteClientRootAddress = string.Empty;

            // TODO: 发送邮件，开发的时候直接读请求的,发布的时候读取配置文件中的

            webSiteClientRootAddress = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";

            await _mailManager.SendMessage(
                user.EmailAddress, title,
                string.Format(contentTemplate, webSiteClientRootAddress, code)
            );

            return code;
        }

        #endregion 发送邮件

        #region 注册

        /// <summary>
        /// 注册前获取配置信息
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="ss"></param>
        /// <returns></returns>
        public async Task<ActionResult> Register(string returnUrl = "", string ss = "")
        {
            return RegisterView(new RegisterViewModel
            {
                PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync(),
                ReturnUrl = returnUrl,
                SingleSignIn = ss
            });
        }

        private ActionResult RegisterView(RegisterViewModel model)
        {
            CheckSelfRegistrationIsEnabled();

            return View("Register", model);
        }

        /// <summary>
        /// 测试用的模拟地址
        /// </summary>
        /// <returns></returns>
        public IActionResult RegisterResultabc()
        {
            if (!DebugHelper.IsDebug)
            {
                return RedirectToAppHome();
            }
            else
            {
                RegisterResultViewModel model = new RegisterResultViewModel
                {
                    EmailAddress = "ddd@qq.com",
                    NameAndSurname = "NameAndSurname",
                    TenancyName = "NameAndSurname",
                    UserName = "NameAndSurname"
                };
                return View("~/Views/Account/RegisterResult.cshtml", model);
            }
        }

        [HttpPost]
        [UnitOfWork(IsolationLevel.ReadUncommitted)]//允许读入别人尚未提交的脏数据，
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                ExternalLoginInfo externalLoginInfo = null;
                if (model.IsExternalLogin)
                {
                    externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                    if (externalLoginInfo == null)
                    {
                        throw new Exception("Can not external login!");
                    }

                    model.UserName = model.EmailAddress;
                    model.Password = LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users.User.CreateRandomPassword();
                }
                else
                {
                    if (model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                    {
                        throw new UserFriendlyException(L("FormIsNotValidMessage"));
                    }
                }

                User user = await _userRegistrationManager.RegisterAsync(
                    model.Name,
                    model.EmailAddress,
                    model.UserName,
                    model.Password,
                    false // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
                );

                // Getting tenant-specific settings
                bool isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

                if (model.IsExternalLogin)
                {
                    Debug.Assert(externalLoginInfo != null);

                    if (string.Equals(externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email), model.EmailAddress, StringComparison.OrdinalIgnoreCase))
                    {
                        user.IsEmailConfirmed = true;
                    }

                    user.Logins = new List<UserLogin>
                    {
                        new UserLogin
                        {
                            LoginProvider = externalLoginInfo.LoginProvider,
                            ProviderKey = externalLoginInfo.ProviderKey,
                            TenantId = user.TenantId
                        }
                    };
                }

                await _unitOfWorkManager.Current.SaveChangesAsync();

                Debug.Assert(user.TenantId != null);

                Tenant tenant = await _tenantManager.GetByIdAsync(user.TenantId.Value);

                // Directly login if possible
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

                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        await _signInManager.SignInAsync(loginResult.Identity, false);
                        return Redirect(GetAppHomeUrl());
                    }

                    Logger.Warn("New registered user could not be login. This should not be normally. login result: " + loginResult.Result);
                }

                return View("RegisterResult", new RegisterResultViewModel
                {
                    TenancyName = tenant.TenancyName,
                    NameAndSurname = user.Name + " " + user.Surname,
                    UserName = user.UserName,
                    EmailAddress = user.EmailAddress,
                    IsEmailConfirmed = user.IsEmailConfirmed,
                    IsActive = user.IsActive,
                    IsEmailConfirmationRequiredForLogin = isEmailConfirmationRequiredForLogin
                });
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                return View("Register", model);
            }
        }

        #endregion 注册

        #region 扩展快捷登录-如QQ

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            string redirectUrl = Url.Action(
                "ExternalLoginCallback",
                "Account",
                new
                {
                    ReturnUrl = returnUrl
                });

            return Challenge(
                // TODO: ...?
                // new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties
                // {
                //     Items = { { "LoginProvider", provider } },
                //     RedirectUri = redirectUrl
                // },
                provider
            );
        }

        [UnitOfWork]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl, string remoteError = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (remoteError != null)
            {
                Logger.Error("Remote Error in ExternalLoginCallback: " + remoteError);
                throw new UserFriendlyException(L("CouldNotCompleteLoginOperation"));
            }

            ExternalLoginInfo externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                Logger.Warn("Could not get information from external login.");
                return RedirectToAction(nameof(Login));
            }

            await _signInManager.SignOutAsync();

            string tenancyName = GetTenancyNameOrNull();

            AbpLoginResult<Tenant, User> loginResult = await _logInManager.LoginAsync(externalLoginInfo, tenancyName);

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

        private async Task<ActionResult> RegisterForExternalLogin(ExternalLoginInfo externalLoginInfo)
        {
            string email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            (string name, string surname) nameinfo = ExternalLoginInfoHelper.GetNameAndSurnameFromClaims(externalLoginInfo.Principal.Claims.ToList());

            RegisterViewModel viewModel = new RegisterViewModel
            {
                EmailAddress = email,
                Name = nameinfo.name,
                Surname = nameinfo.surname,
                IsExternalLogin = true,
                ExternalLoginAuthSchema = externalLoginInfo.LoginProvider
            };

            if (nameinfo.name != null &&
                nameinfo.surname != null &&
                email != null)
            {
                return await Register(viewModel);
            }

            return RegisterView(viewModel);
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

        #endregion 扩展快捷登录-如QQ

        #region 帮助方法

        /// <summary>
        /// 跳转到应用域中的Home页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RedirectToAppHome()
        {
            return RedirectToAction("Index", "Home", new { area = "AreasAdminName" });
        }

        /// <summary>
        /// 获取到应用域中的Home页面
        /// </summary>
        /// <returns></returns>

        public string GetAppHomeUrl()
        {
            return Url.Action("Index", "Home", new { area = "AreasAdminName" });
        }

        #endregion 帮助方法

        #region Change Tenant

        public async Task<ActionResult> TenantChangeModal()
        {
            Sessions.Dto.GetCurrentLoginInformationsOutput loginInfo = await _sessionAppService.GetCurrentLoginInformations();
            return View("/Views/Shared/Components/TenantChange/_ChangeModal.cshtml", new ChangeModalViewModel
            {
                TenancyName = loginInfo.Tenant?.TenancyName
            });
        }

        #endregion Change Tenant

        #region Common

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

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

            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return defaultValueBuilder();
        }

        #endregion Common

        #region Etc

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            UserIdentifier defaultTenantAdmin = new UserIdentifier(1, 2);
            UserIdentifier hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                    "App.SimpleMessage",
                    new MessageNotificationData(message),
                    severity: NotificationSeverity.Info,
                    userIds: new[] { defaultTenantAdmin, hostAdmin }
                 );

            return Content("Sent notification: " + message);
        }

        #endregion Etc
    }
}
