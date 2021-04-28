using System;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos
{
    public class TenantListDto : EntityDto<int>
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public string EditionDisplayName { get; set; }

        public string ConnectionString { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? SubscriptionEndUtc { get; set; }

        public int? EditionId { get; set; }
    }
}