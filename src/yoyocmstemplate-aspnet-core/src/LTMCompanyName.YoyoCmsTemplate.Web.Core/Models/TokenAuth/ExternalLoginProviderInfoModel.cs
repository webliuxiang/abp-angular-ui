using System.Collections.Generic;
using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
        public Dictionary<string, string> AdditionalParams { get; set; }

    }
}
