using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}