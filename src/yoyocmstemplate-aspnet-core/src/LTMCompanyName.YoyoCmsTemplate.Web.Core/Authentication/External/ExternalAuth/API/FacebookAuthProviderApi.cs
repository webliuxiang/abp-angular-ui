using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.API
{
    public class FacebookAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string ProviderName = "Facebook";
        public ILogger Logger { get; set; }

        public FacebookAuthProviderApi()
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
                HttpResponseMessage httpResponseMessage = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, FacebookDefaults.UserInformationEndpoint) { Headers = { Authorization = new AuthenticationHeaderValue("Bearer", accessCode) } });
                httpResponseMessage.EnsureSuccessStatusCode();
                JObject user = JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
                externalAuthUserInfo = new ExternalAuthUserInfo()
                {
                    Name = user.Value<string>("name"),
                    EmailAddress = User.CreateRandomEmail(),
                    ProviderKey = user.Value<string>("id"),
                    Provider = ProviderName,
                };
            }
            return externalAuthUserInfo;
        }
    }
}