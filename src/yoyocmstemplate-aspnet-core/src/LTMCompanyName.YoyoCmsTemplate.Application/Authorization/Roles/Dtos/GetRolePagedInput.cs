using System.Collections.Generic;
using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos
{
    public class GetRolePagedInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public List<string> PermissionNames { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting)) Sorting = "Id";
        }
    }
}