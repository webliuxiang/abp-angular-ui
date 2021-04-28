using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.MultiTenancy;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos
{
    /// <summary>
    /// 创建租户的DTO信息
    /// </summary>
    [AutoMapTo(typeof(Tenant))]
    public class CreateTenantDto
    {
        /// <summary>
        /// 全局唯一的租户Id
        /// </summary>
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(AbpTenantBase.TenancyNameRegex)]
        public string TenancyName { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        [Required]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(AbpUserBase.MaxNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [StringLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }

        public bool IsActive { get; set; }


        /// <summary>
        /// 验证码
        /// </summary>
        [DisableAuditing]
        public string VerificationCode { get; set; }

        /// <summary>
        /// 租户管理员密码
        /// </summary>
        public string TenantAdminPassword { get; set; }

    }
}
