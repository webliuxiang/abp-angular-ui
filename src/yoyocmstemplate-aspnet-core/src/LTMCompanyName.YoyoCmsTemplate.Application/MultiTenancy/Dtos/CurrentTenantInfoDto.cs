using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos
{
    public class CurrentTenantInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
