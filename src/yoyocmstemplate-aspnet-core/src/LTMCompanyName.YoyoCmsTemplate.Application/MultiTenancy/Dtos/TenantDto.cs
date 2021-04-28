using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.MultiTenancy;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantDto : EntityDto
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(AbpTenantBase.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}