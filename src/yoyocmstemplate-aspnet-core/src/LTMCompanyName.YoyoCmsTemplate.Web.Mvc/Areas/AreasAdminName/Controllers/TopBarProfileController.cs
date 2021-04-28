using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Timing;
using LTMCompanyName.YoyoCmsTemplate.Timing.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.TopBarProfile;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize]
    public class TopBarProfileController : YoyoCmsTemplateControllerBase
    {
        private readonly IProfileAppService _profileAppService;
        private readonly ITimingAppService _timingAppService;
        private readonly ITenantCache _tenantCache;
        private readonly IUserLoginAppService _userLoginAppService;

        public TopBarProfileController(
            IProfileAppService profileAppService,
            ITimingAppService timingAppService,
            ITenantCache tenantCache, IUserLoginAppService userLoginAppService)
        {
            _profileAppService = profileAppService;
            _timingAppService = timingAppService;
            _tenantCache = tenantCache;
            _userLoginAppService = userLoginAppService;
        }

        public async Task<PartialViewResult> MySettingsModal()
        {
            var output = await _profileAppService.GetCurrentUserProfileForEdit();

            var timezoneItems = await _timingAppService.GetTimezoneComboboxItems(new GetTimezoneComboboxItemsInput
            {
                DefaultTimezoneScope = SettingScopes.User,
                SelectedTimezoneId = output.Timezone
            });


            var viewModel = new MySettingsViewModel(output)
            {
                TimezoneItems = timezoneItems,
                SmsVerificationEnabled = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.UserManagement.SmsVerificationEnabled)
            };

            return PartialView("_MySettingsModal", viewModel);



        }

        public PartialViewResult ChangePictureModal()
        {
            return PartialView("_ChangePictureModal");
        }

        public PartialViewResult ChangePasswordModal()
        {
            return PartialView("_ChangePasswordModal");
        }
        /// <summary>
        /// 登录足迹
        /// </summary>
        /// <returns></returns>
        public async Task<PartialViewResult> LoginAttemptsModal()
        {
            var output = await _userLoginAppService.GetRecentUserLoginAttempts();
            var model = new UserLoginAttemptModalViewModel
            {
                LoginAttempts = output.Items.ToList()
            };
            return PartialView("_LoginAttemptsModal", model);
        }

        public PartialViewResult SmsVerificationModal()
        {
            return PartialView("_SmsVerificationModal");
        }


        public PartialViewResult LinkedAccountsModal()
        {
            return PartialView("_LinkedAccountsModal");
        }

        public PartialViewResult LinkAccountModal()
        {
            ViewBag.TenancyName = GetTenancyNameOrNull();
            return PartialView("_LinkAccountModal");
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }
    }
}