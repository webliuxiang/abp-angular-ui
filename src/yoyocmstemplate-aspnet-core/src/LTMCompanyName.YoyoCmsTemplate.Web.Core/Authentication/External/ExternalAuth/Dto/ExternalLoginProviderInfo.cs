using System;
using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto
{
    public class ExternalLoginProviderInfo
    {
        public string Name { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public Type ProviderApiType { get; set; }

        public Dictionary<string, string> AdditionalParams { get; set; }

        public ExternalLoginProviderInfo(string name, string clientId, string clientSecret, Type providerApiType, Dictionary<string, string> additionalParams = null)
        {
            Name = name;
            ClientId = clientId;
            ClientSecret = clientSecret;
            ProviderApiType = providerApiType;
            AdditionalParams = additionalParams;
        }
    }
}
