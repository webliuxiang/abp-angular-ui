using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Abp.Runtime.Session;
using Abp.Timing;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings;
using LTMCompanyName.YoyoCmsTemplate.Timing;
using LTMCompanyName.YoyoCmsTemplate.Timing.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.HostSettings;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_Host_Settings)]
    public class HostSettingsController : YoyoCmsTemplateControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IHostSettingsAppService _hostSettingsAppService;
        private readonly IEditionAppService _editionAppService;
        private readonly ITimingAppService _timingAppService;

        public HostSettingsController(
            IHostSettingsAppService hostSettingsAppService,
            UserManager userManager,
            IEditionAppService editionAppService,
            ITimingAppService timingAppService)
        {
            _hostSettingsAppService = hostSettingsAppService;
            _userManager = userManager;
            _editionAppService = editionAppService;
            _timingAppService = timingAppService;
        }

        public async Task<ActionResult> Index()
        {
            var hostSettings = await _hostSettingsAppService.GetAllSettings();
            var editionItems = await _editionAppService.GetEditionComboboxItems(hostSettings.TenantManagement.DefaultEditionId);
            var timezoneItems = await _timingAppService.GetTimezoneComboboxItems(new GetTimezoneComboboxItemsInput
            {
                DefaultTimezoneScope = SettingScopes.Application,
                SelectedTimezoneId = await SettingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone)
            });

            var user = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());

            ViewBag.CurrentUserEmail = user.EmailAddress;

            var model = new HostSettingsViewModel
            {
                Settings = hostSettings,
                EditionItems = editionItems,
                TimezoneItems = timezoneItems
            };

            return View(model);
        }
    }
}