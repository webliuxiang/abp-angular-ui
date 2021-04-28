using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Runtime.Session;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Sessions;
using LTMCompanyName.YoyoCmsTemplate.Web.Portal.Startup.Navigation;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Shared.Components.Header
{
    public class HeaderViewComponent : YoyoCmsTemplateViewComponent
    {

        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ISessionAppService _sessionAppService;
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly ISettingManager _settingManager;


        public HeaderViewComponent(IMultiTenancyConfig multiTenancyConfig, ISessionAppService sessionAppService, IUserNavigationManager userNavigationManager, ISettingManager settingManager)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _sessionAppService = sessionAppService;
            _userNavigationManager = userNavigationManager;
            _settingManager = settingManager;
        }


        public async Task<IViewComponentResult> InvokeAsync(string currentPageName)
        {


            // 检查千层饼是否可以触发。


            //var tenancyName = "";
            //if (AbpSession.TenantId.HasValue)
            //{
            //    var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
            //    tenancyName = tenant.TenancyName;
            //}

            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionAppService.GetCurrentLoginInformations(),
                IsInHostView = !AbpSession.TenantId.HasValue,

                Menu = await _userNavigationManager.GetMenuAsync(PortalNavigationProvider.MenuName, AbpSession.ToUserIdentifier()),
                CurrentPageName = currentPageName,
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                TenantRegistrationEnabled = await _settingManager.GetSettingValueAsync<bool>(AppSettingNames.HostSettings.AllowSelfRegistration),

                //todo:url璺宠浆
            };

            return View(headerModel);

        }




    }
}
