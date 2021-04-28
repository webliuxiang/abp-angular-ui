using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Runtime.Session;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Sessions;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Models;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Startup.Navigation;
using Microsoft.AspNetCore.Components;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared.Components.Layouts.Headers
{
    public class LogoAndNavBase:AbpComponentBase
    {

        [Inject]
        private  IMultiTenancyConfig _multiTenancyConfig { get; set; }
        [Inject] private  ISessionAppService _sessionAppService { get; set; }
        [Inject] private  IUserNavigationManager _userNavigationManager { get; set; }
        [Inject] private  ISettingManager _settingManager { get; set; }


        public HeaderViewModel HeaderView{ get; set; }

        public string currentPageName{ get; set; }

        protected override async Task OnInitializedAsync()
        {

            HeaderView = new HeaderViewModel
            {
                LoginInformations = await _sessionAppService.GetCurrentLoginInformations(),
                IsInHostView = !AbpSession.TenantId.HasValue,

                Menu = await _userNavigationManager.GetMenuAsync(PublicNavigationProvider.MenuName, AbpSession.ToUserIdentifier()),
                CurrentPageName = currentPageName,
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                TenantRegistrationEnabled = await _settingManager.GetSettingValueAsync<bool>(AppSettingNames.HostSettings.AllowSelfRegistration)
            };

            //todo:url跳转



        }



    }
}
