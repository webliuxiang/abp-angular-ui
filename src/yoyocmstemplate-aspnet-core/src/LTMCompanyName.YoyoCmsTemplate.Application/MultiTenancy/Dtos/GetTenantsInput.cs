using System;
using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos
{
    public class GetTenantsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        public DateTime? SubscriptionStart { get; set; }
        public DateTime? SubscriptionEnd { get; set; }
        public DateTime? CreationDateStart { get; set; }
        public DateTime? CreationDateEnd { get; set; }
        public int? EditionId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting)) Sorting = "TenancyName";

            Sorting = Sorting.Replace("editionDisplayName", "Edition.DisplayName");
        }
    }
}