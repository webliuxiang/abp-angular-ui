using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;
using Microsoft.Extensions.Logging;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.API
{
    public class WechatAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string ProviderName = "Wechat";
        public ILogger Logger { get; set; }

        public WechatAuthProviderApi(ILogger<WechatAuthProviderApi> logger)
        {
            Logger = logger;
        }

        public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            var accessTokenResult = await OAuthApi.GetAccessTokenAsync(base.ProviderInfo.ClientId, base.ProviderInfo.ClientSecret, accessCode);
            var userInfo = await OAuthApi.GetUserInfoAsync(accessTokenResult.access_token, accessTokenResult.openid);
            return new ExternalAuthUserInfo()
            {
                AvatarUrl = userInfo.headimgurl,
                EmailAddress = userInfo.unionid + "@52abp.com",
                Name = userInfo.nickname,
                Provider = ProviderName,
                //Surname = userInfo.nickname,
                ProviderKey = userInfo.unionid
            };
        }
    }
}
