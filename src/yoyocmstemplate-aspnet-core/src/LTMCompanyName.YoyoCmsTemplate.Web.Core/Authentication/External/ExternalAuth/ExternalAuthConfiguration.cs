using System.Collections.Generic;
using Abp.Dependency;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth
{
    public class ExternalAuthConfiguration : IExternalAuthConfiguration, ISingletonDependency
    {
        public List<ExternalLoginProviderInfo> Providers { get; }

        public ExternalAuthConfiguration()
        {
            Providers = new List<ExternalLoginProviderInfo>();
        }
    }
}
