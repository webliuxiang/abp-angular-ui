using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Runtime.Session;
using Abp.Timing;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings;
using LTMCompanyName.YoyoCmsTemplate.Timing;
using LTMCompanyName.YoyoCmsTemplate.Timing.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_Tenant_Settings)]
    public class SettingsController : YoyoCmsTemplateControllerBase
    {
        private readonly UserManager _userManager;
        private readonly ITenantSettingsAppService _tenantSettingsAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ITimingAppService _timingAppService;

        public SettingsController(
            ITenantSettingsAppService tenantSettingsAppService,
            IMultiTenancyConfig multiTenancyConfig,
            ITimingAppService timingAppService,
            UserManager userManager)
        {
            _tenantSettingsAppService = tenantSettingsAppService;
            _multiTenancyConfig = multiTenancyConfig;
            _timingAppService = timingAppService;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _tenantSettingsAppService.GetAllSettings();
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            var timezoneItems = await _timingAppService.GetTimezoneComboboxItems(new GetTimezoneComboboxItemsInput
            {
                DefaultTimezoneScope = SettingScopes.Tenant,
                SelectedTimezoneId = await SettingManager.GetSettingValueForTenantAsync(TimingSettingNames.TimeZone, AbpSession.GetTenantId())
            });

            var user = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());

            ViewBag.CurrentUserEmail = user.EmailAddress;

            var model = new SettingsViewModel
            {
                Settings = output,
                TimezoneItems = timezoneItems
            };

            return View(model);
        }
    }
}