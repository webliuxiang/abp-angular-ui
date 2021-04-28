using System.Collections.Generic;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Views.Shared.Components.AdminTopBar
{

    /// <summary>
    /// 管理端顶部菜单栏
    /// </summary>
    public class AdminTopBarViewModel
    {

        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

        public IReadOnlyList<LanguageInfo> Languages { get; set; }

        public LanguageInfo CurrentLanguage { get; set; }

        public bool IsMultiTenancyEnabled { get; set; }

        public bool IsImpersonatedLogin { get; set; }





        public string GetShownLoginName()
        {
            var userName = "<span id=\"TopBarCurrentUserName\">" + LoginInformations.User.UserName + "</span>";

            if (!IsMultiTenancyEnabled)
            {
                return userName;
            }

            return LoginInformations.Tenant == null
                ? "<span class='tenancy-name'>.\\</span>" + userName
                : "<span class='tenancy-name'>" + LoginInformations.Tenant.TenancyName + "\\" + "</span>" + userName;
        }

        public string GetLogoUrl(string appPath, string logoSkin)
        {
            if (LoginInformations?.Tenant?.LogoId == null)
            {
                return appPath + $"Common/Images/app-logo-on-{logoSkin}.svg";
            }

            //id parameter is used to prevent caching only.
            return appPath + "TenantCustomization/GetLogo?id=" + LoginInformations.Tenant.LogoId;
        }
    }
}