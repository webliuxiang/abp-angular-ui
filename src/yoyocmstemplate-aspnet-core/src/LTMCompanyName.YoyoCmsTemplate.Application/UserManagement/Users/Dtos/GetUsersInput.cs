using System.Collections.Generic;
using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos
{
    /// <summary>
    ///     用户信息查询Dto
    /// </summary>
    public class GetUsersInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        //DOTO:在这里增加查询参数

        /// <summary>
        ///     权限
        /// </summary>
        public List<string> Permission { get; set; }

        /// <summary>
        ///     检索角色Id列表
        /// </summary>
        public List<int> Role { get; set; }

        /// <summary>
        ///     是否已验证邮箱
        /// </summary>
        public bool? IsEmailConfirmed { get; set; }

        /// <summary>
        ///     是否已激活
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// 仅被锁定的用户
        /// </summary>
        public bool OnlyLockedUsers { get; set; }

        /// <summary>
        ///     用于排序的默认值
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting)) Sorting = "Id";
        }
    }
}