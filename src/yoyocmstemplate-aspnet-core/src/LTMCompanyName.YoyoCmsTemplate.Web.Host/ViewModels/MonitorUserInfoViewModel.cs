using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.ViewModels
{
    public class MonitorUserInfoViewModel
    {
        public bool IsMultiTenancyEnabled { get; set; }

        public GetCurrentLoginInformationsOutput LoginInformation { get; set; }

        public string GetShownLoginName()
        {
            var userName = "<span>" + LoginInformation.User.UserName + "</span>";

            if (!IsMultiTenancyEnabled)
            {
                return userName;
            }

            return LoginInformation.Tenant == null
                ? userName
                : LoginInformation.Tenant.TenancyName + "\\" + userName;
        }
    }
}