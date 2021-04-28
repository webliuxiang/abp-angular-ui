using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitListDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
    }
}