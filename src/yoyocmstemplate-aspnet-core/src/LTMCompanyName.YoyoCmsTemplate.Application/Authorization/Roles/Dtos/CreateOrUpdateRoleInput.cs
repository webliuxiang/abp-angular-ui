using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos
{
    public class CreateOrUpdateRoleInput
    {
        [Required] public RoleEditDto Role { get; set; }

        [Required] public List<string> GrantedPermissionNames { get; set; }
    }
}