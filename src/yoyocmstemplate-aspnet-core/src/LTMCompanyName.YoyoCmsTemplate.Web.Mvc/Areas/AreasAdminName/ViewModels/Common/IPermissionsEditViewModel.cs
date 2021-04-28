using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}