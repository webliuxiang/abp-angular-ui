using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.API
{
    public class MicrosoftAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string ProviderName = "Microsoft";
        public ILogger Logger { get; set; }

        public MicrosoftAuthProviderApi()
        {
            Logger = NullLogger.Instance;
        }

        public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            ExternalAuthUserInfo externalAuthUserInfo;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth middleware");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                client.Timeout = TimeSpan.FromSeconds(30.0);
                client.MaxResponseContentBufferSize = 10485760L;
                HttpResponseMessage httpResponseMessage = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, (string)MicrosoftAccountDefaults.UserInformationEndpoint) { Headers = { Authorization = new AuthenticationHeaderValue("Bearer", accessCode) } });
                httpResponseMessage.EnsureSuccessStatusCode();
                JObject user = JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
                externalAuthUserInfo = new ExternalAuthUserInfo()
                {
                    Name = user.Value<string>("displayName"),
                    EmailAddress = user.Value<string>("mail") ?? user.Value<string>("userPrincipalName"),
                    ProviderKey = Guid.Parse(user.Value<string>("id").PadLeft(32, '0')).ToString(),
                    Provider = ProviderName,
                };
            }
            return externalAuthUserInfo;
        }
    }
}
