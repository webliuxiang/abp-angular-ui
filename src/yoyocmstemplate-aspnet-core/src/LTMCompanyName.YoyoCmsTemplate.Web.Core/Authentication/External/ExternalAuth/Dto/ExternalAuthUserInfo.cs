using System;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto
{
    public class ExternalAuthUserInfo
    {
        public string ProviderKey { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }
        [Obsolete]
        public string Surname { get; set; } = String.Empty;

        public string Provider { get; set; }
        public string AvatarUrl { get; set; }
    }
}
