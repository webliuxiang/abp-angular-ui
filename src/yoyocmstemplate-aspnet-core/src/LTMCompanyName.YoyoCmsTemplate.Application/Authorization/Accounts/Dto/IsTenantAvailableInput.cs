using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto
{
    /// <summary>
    /// 验证租户名称是否可用
    /// </summary>
    public class IsTenantAvailableInput
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}