using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Session;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Host.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Controllers
{
    /// <summary>
    /// 监控服务
    /// </summary>
    public class MonitorController : YoyoCmsTemplateControllerBase
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IWebSessionCache _webSessionCache;
        private readonly AccountAppService _accountAppService;
        private readonly SignInManager _signInManager;
        private readonly LogInManager _logInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;

        public MonitorController(IMultiTenancyConfig multiTenancyConfig, IWebSessionCache webSessionCache, AccountAppService accountAppService, SignInManager signInManager, LogInManager logInManager,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _webSessionCache = webSessionCache;
            _accountAppService = accountAppService;
            _signInManager = signInManager;
            _logInManager = logInManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
        }

        [DisableAuditing]
        public async Task<IActionResult> Index()
        {
            var model = new MonitorUserInfoViewModel
            {
                LoginInformation = await _webSessionCache.GetCurrentLoginInformationsAsync(),
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled
            };

            if (model.LoginInformation?.User == null)
            {
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "")
        {
            if (model.TenancyName != null)
            {
                var isTenantAvailable = await _accountAppService.IsTenantAvailable(new IsTenantAvailableInput
                {
                    TenancyName = model.TenancyName
                });

                switch (isTenantAvailable.State)
                {
                    case TenantAvailabilityState.InActive:
                        throw new UserFriendlyException(L("TenantIsNotActive", model.TenancyName));
                    case TenantAvailabilityState.NotFound:
                        throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", model.TenancyName));
                }
            }

            var loginResult = await GetLoginResultAsync(model.UsernameOrEmailAddress, model.Password, model.TenancyName);

            if (loginResult.User.NeedToChangeThePassword)
            {
                throw new UserFriendlyException(L("RequiresPasswordChange"));
            }

            var signInResult = await _signInManager.SignInOrTwoFactorAsync(loginResult, model.RememberMe);

            if (signInResult.RequiresTwoFactor)
            {
                throw new UserFriendlyException(L("RequiresTwoFactorAuth"));
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }
    }
}
