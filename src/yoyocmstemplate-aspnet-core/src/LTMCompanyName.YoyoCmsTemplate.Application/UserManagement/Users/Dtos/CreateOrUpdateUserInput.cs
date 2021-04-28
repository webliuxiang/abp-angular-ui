using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos
{
    /// <summary>
    ///     用户信息新增和编辑时用Dto
    /// </summary>
    public class CreateOrUpdateUserInput
    {
        public CreateOrUpdateUserInput()
        {
            OrganizationUnits = new List<long>();
        }

        /// <summary>
        ///     用户信息编辑Dto
        /// </summary>
        public UserEditDto User { get; set; }

        /// <summary>
        ///     授权的角色
        /// </summary>
        [Required]
        public string[] AssignedRoleNames { get; set; }

        /// <summary>
        ///     所在的组织机构的ID
        /// </summary>
        public List<long> OrganizationUnits { get; set; }


        /// <summary>
        ///     发送激活邮件
        /// </summary>
        public bool SendActivationEmail { get; set; }

        /// <summary>
        ///     设置随机密码
        /// </summary>
        public bool SetRandomPassword { get; set; }
    }
}