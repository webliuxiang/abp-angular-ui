using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Views.Shared.Components.AdminFooter
{

    public class AdminFooterViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

        public bool UseWrapperDiv { get; set; }

        public string GetProductNameWithEdition()
        {
            const string productName = "52ABP-PRO";

            if (LoginInformations.Tenant == null)
            {
                return productName;
            }

            if (LoginInformations.Tenant.Edition == null)
            {
                return productName;
            }

            if (LoginInformations.Tenant.Edition.DisplayName == null)
            {
                return productName;
            }

            return productName + " " + LoginInformations.Tenant.Edition.DisplayName;
        }
    }
}