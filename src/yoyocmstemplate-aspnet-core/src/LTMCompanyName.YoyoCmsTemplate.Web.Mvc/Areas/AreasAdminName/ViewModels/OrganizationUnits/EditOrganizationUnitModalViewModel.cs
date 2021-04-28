using Abp.AutoMapper;
using Abp.Organizations;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.OrganizationUnits
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class EditOrganizationUnitModalViewModel
    {
        public long? Id { get; set; }

        public string DisplayName { get; set; }
    }
}