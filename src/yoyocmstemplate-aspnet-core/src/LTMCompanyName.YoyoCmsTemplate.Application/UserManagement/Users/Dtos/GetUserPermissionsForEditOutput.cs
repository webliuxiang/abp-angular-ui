using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos
{
    public class GetUserPermissionsForEditOutput
    {
        /// <summary>
        ///     所有的权限
        /// </summary>
        public List<FlatPermissionDto> Permissions { get; set; }

        /// <summary>
        ///     已有权限
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}