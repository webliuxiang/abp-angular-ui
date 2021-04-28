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
        ///     登陆检查
        /// </summary>
        /// <param name="returnUrl">重定向的路由</param>
        /// <param name="actionResult">重定向的action</param>
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
        ///     发送邮件并返回Code
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="contentTemplate">内容模板</param>
        /// <param name="user">用户</param>
        /// <returns>生成的Code</returns>
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

        #region 登陆 / 注销

        /// <summary>
        ///     登陆页面
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

            // 已登陆跳转到首页或登录页
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
                IsSelfRegistrationAllowed = IsSelfRegistrationEnabled(), // 是否允许注册

                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled //是否启动了多租户
            });
        }

        /// <summary>
        ///     登陆API
        /// </summary>
        /// <param name="loginModel">登陆信息</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel)
        {
            //取消验证码登录
            //    this.ValuedationCode(CaptchaType.TenantUserLogin, loginModel.Code);

            await VerifyImgCodeAsync(CaptchaType.TenantUserLogin, loginModel.Code);

            loginModel.ReturnUrl = NormalizeReturnUrl(loginModel.ReturnUrl);
            if (!IsNullOrWhiteSpace(loginModel.ReturnUrlHash))
            {
                loginModel.ReturnUrl += loginModel.ReturnUrlHash;
            }

            /*
             这里的 loginResult.Identity.Claims 是在Core层 UserClaimsPrincipalFactory中添加的Claim,
             如果要增加 自定义Claim，请查看具体实现
             */
            var loginResult = await GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password,
                GetTenancyNameOrNull());

            // TOTO:确认邮箱已认证
            //if (!loginResult.User.IsEmailConfirmed)
            //{
            //    throw new UserFriendlyException("");
            //}

            await _signInManager.SignOutAndSignInAsync(loginResult.Identity, loginModel.RememberMe);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Json(new AjaxResponse { TargetUrl = loginModel.ReturnUrl });
        }

        /// <summary>
        ///     退出登陆
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        /// <summary>
        ///     获取登陆结果
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

        #endregion 登陆 / 注销

        #region 注册

        /// <summary>
        ///     注册- 注册页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            // 已登陆跳转到首页
            if (LoginCheck("/", out var redirect))
            {
                return redirect;
            }

            var model = new RegisterViewModel();
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return View(model);
        }

        /// <summary>
        ///     注册- 注册接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {


            throw new UserFriendlyException("暂未开放注册，请等待。");
#pragma warning disable CS0162 // 检测到无法访问的代码
            ExternalLoginInfo externalLoginInfo = null;

            if (model.IsExternalLogin)
            {
                //估计有bug。需要验证
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
                false // 假定的电子邮件地址总是被确认。如果要实现电子邮件确认，请更改此选项。
            );

            // 获取租户设置
            var isEmailConfirmationRequiredForLogin =
                await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                    .IsEmailConfirmationRequiredForLogin);

            await _unitOfWorkManager.Current.SaveChangesAsync();
            Debug.Assert(user.TenantId != null);//下个断言，租户Id不可能为空
            var tenant = await _tenantManager.GetByIdAsync(user.TenantId.Value);

            // 如有可能，直接登入
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
                    // 外部登陆不校验邮箱, 直接激活
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
                // 发送激活邮件
                await SendActivationEmail("52ABP官方 - 激活账号邮件", "请点击链接进行激活: {0}Account/RegisterResult?code={1}", user,
                    activeCode);

                return Json($"/Account/RegisterByLink?u={user.UserName}&e={user.EmailAddress}");
            }

            // 外部登陆不校验邮箱, 直接带code跳转
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
#pragma warning restore CS0162 // 检测到无法访问的代码
        }

        /// <summary>
        ///     注册- 注册成功,重新发送邮件页面
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
        ///     注册- 邮箱认证激活API
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ActionResult> RegisterResult(string code)
        {
            // 校验 resetcode错误
            var user = await _userManager.GetUserByEmailConfirmationCode(code);
            if (user == null)
            {
                return View(false);
            }

            user.IsEmailConfirmed = true;
            user.IsActive = true;
            user.EmailConfirmationCode = Empty;
            await _userManager.UpdateAsync(user);

            // 激活校验成功,立即登陆
            var identity = await _logInManager.LoginAsync(user);
            await _signInManager.SignOutAndSignInAsync(identity, false);

            return View(true);
        }

        /// <summary>
        ///     注册- 重新发送邮箱校验码API
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ReSendEmailConfirmationCode([FromBody] EmailConfirmationViewModel input)
        {
            // 校验验证码
            //   ValuedationCode(VerificationImgType.RegisterActive, input.Code);

            await VerifyImgCodeAsync(CaptchaType.TenantUserRegisterActiveEmail, input.Code);


            // 校验 resetcode错误
            var user = _userManager.Users
                .Where(o => o.EmailAddress == input.EmailAddress)
                .FirstOrDefault();
            if (user == null)
            {
                return new JsonResult(false);
            }

            if (user.IsEmailConfirmed || user.IsActive)
            {
                throw new UserFriendlyException("此用户已经激活!");
            }

            // 发送激活码
            var activeCode = await SendMailAndReturnCode("52ABP官方 - 激活账号邮件",
                "请点击链接进行激活: {0}Account/RegisterResult?code={1}", user);

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

        #endregion 注册

        #region 忘记密码

        /// <summary>
        ///     忘记密码页面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ForgotPassword()
        {
            // 已登陆跳转到首页
            if (LoginCheck("/", out var redirect))
            {
                return redirect;
            }

            await Task.Yield();
            return View();
        }

        /// <summary>
        ///     忘记密码Api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ForgotPassword(ForgotPasswordViewModel input)
        {
            // 校验验证码
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
        ///     忘记密码- 找回密码邮件发送成功,找回密码提示页面
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ForgotPasswordByLink()
        {
            await Task.Yield();
            return View();
        }

        /// <summary>
        /// 忘记密码- 输入新密码页面
        /// </summary>
        /// <param name="c">参数c不要更改变量名</param>
        /// <returns></returns>
        public async Task<ActionResult> ForgotPasswordReset(string c)
        {
            await Task.Yield();
            //解析被转义的字符串 
            ViewBag.Code = Uri.UnescapeDataString(c);



            return View();
        }

        /// <summary>
        ///     忘记密码- 重置密码接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordInput input)
        {
            // 校验验证码
            await VerifyImgCodeAsync(CaptchaType.TenantUserResetPassword, input.Code);



            // 两次输入密码不一致
            if (input.NewPassword != input.ConfirmPassword)
            {
                throw new UserFriendlyException("两次输入密码不一致");
            }


            //可用于简单地加密/解密文本。
            input.ResetCode = SimpleStringCipher.Instance.Decrypt(input.ResetCode);//解析文本
            var query = HttpUtility.ParseQueryString(input.ResetCode);
            input.ResetCode = query["c"];

            var output = await _accountAppService.ResetPasswordAsync(input);

            if (output.CanLogin)
            {
                var user = await _userManager.GetUserByEmail(output.EmailAddress);
                // 重置密码成功,立即登陆
                var identity = await _logInManager.LoginAsync(user);
                //注销并重新登录，非持久化登录
                await _signInManager.SignOutAndSignInAsync(identity, false);

            }


            return new JsonResult(true);

        }

        #endregion 忘记密码

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
                Logger.Error("ExternalLoginCallback中发生的远程错误: " + remoteError);
                throw new UserFriendlyException(L("CouldNotCompleteLoginOperation"));
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                Logger.Warn("无法从外部登录获取信息。");

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
        ///     返回的主页
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
        ///     让ReturnURL正常化
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

            //防御开放式漏洞
            return Url.IsLocalUrl(returnUrl) ? returnUrl : defaultValueBuilder();
        }

        #endregion Common
    }
}
