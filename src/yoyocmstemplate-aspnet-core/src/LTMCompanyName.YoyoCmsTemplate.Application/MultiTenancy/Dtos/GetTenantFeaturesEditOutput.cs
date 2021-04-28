using System.Collections.Generic;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}
