using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos
{
    public class GetUserPermissionsTreeForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}