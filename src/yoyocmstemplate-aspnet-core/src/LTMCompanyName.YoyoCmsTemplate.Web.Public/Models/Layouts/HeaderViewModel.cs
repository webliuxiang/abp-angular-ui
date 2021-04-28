using Abp.Application.Navigation;
using Abp.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Models
{

    /// <summary>
    /// 头部栏目的Viewmodel
    /// </summary>
    public class HeaderViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }


        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }

        public bool IsMultiTenancyEnabled { get; set; }

        public bool TenantRegistrationEnabled { get; set; }

        public bool IsInHostView { get; set; }

        /// <summary>
        /// 服务端的根路径地址
        /// </summary>
        public string AdminServerRootAddress { get; set; }
        /// <summary>
        /// Web端的根路径地址
        /// </summary>
        public string WebClientRootAddress { get; set; }

        public string GetShownLoginName()
        {
            if (!IsMultiTenancyEnabled)
            {
                return LoginInformations.User.UserName;
            }

            return LoginInformations.Tenant == null
                ? ".\\" + LoginInformations.User.UserName
                : LoginInformations.Tenant.TenancyName + "\\" + LoginInformations.User.UserName;
        }

        public string GetLogoUrl(string appPath)
        {
            if (!IsMultiTenancyEnabled || LoginInformations?.Tenant?.LogoId == null)
            {
                return appPath + "Common/Images/app-logo-on-light.png";
            }

            return AdminServerRootAddress.EnsureEndsWith('/') + "TenantCustomization/GetLogo?id=" + LoginInformations.Tenant.LogoId;
        }
    }
}

