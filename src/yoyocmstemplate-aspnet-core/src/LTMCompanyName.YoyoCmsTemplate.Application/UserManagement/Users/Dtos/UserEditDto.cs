using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Domain.Entities;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos
{
    /// <summary>
    ///     用户信息编辑用Dto
    /// </summary>
    public class UserEditDto : IPassivable
    {
        /// <summary>
        ///     根据id是否有值来判断是创建还是添加
        /// </summary>
        public long? Id { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(UserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public virtual bool IsLockoutEnabled { get; set; }

        /// <summary>
        ///     需要修改密码
        /// </summary>
        public bool NeedToChangeThePassword { get; set; }

        /// <summary>
        ///     头像Id
        /// </summary>
        public virtual Guid? ProfilePictureId { get; set; }

        public virtual bool IsTwoFactorEnabled { get; set; }

        public bool IsActive { get; set; }
    }
}