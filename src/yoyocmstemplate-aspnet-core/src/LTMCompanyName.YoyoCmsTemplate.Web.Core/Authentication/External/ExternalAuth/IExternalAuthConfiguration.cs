using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
