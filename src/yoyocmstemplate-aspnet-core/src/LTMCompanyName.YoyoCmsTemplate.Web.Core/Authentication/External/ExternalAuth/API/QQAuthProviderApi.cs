using System.Net.Http;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.API
{
    public class QQAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string ProviderName = "QQ";
        public ILogger Logger { get; set; }

        public QQAuthProviderApi()
        {
            Logger = NullLogger.Instance;
        }

        public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            // 没有采用jwt的方式解析数据, 后期可优化安全性
            var tokenModel = JsonConvert.DeserializeAnonymousType(accessCode, new { openId = "", accessToken = "" });
            // client token
            if (!tokenModel.accessToken.IsNullOrEmpty())
            {
                // 暂时没有比较好用的QQ的sdk, 手撸解析
                using (var httpClient = new HttpClient())
                {
                    var res = await httpClient.GetStringAsync($"https://graph.qq.com/user/get_user_info?access_token={tokenModel.accessToken}&oauth_consumer_key={base.ProviderInfo.ClientId}&openid={tokenModel.openId}");
                    var jObject = JObject.Parse(res);
                    if (jObject["ret"].Value<int>() != 0)
                    {
                        throw new UserFriendlyException("CouldNotValidateExternalUser");
                    }
                    var user = jObject.ToObject<QQUser>();
                    return new ExternalAuthUserInfo()
                    {
                        Name = user.nickname,
                        EmailAddress = User.CreateRandomEmail(),
                        Provider = ProviderName,
                        ProviderKey = tokenModel.openId,
                        //Surname = user.nickname,
                        AvatarUrl = user.figureurl_qq_1
                    };
                }
            }
            else
            {
                // client userinfo
                return JsonConvert.DeserializeObject<ExternalAuthUserInfo>(accessCode);
            }
        }
    }


    public class QQUser
    {
        //public int ret { get; set; }
        //public string msg { get; set; }
        public string nickname { get; set; }
        //public string figureurl { get; set; }
        //public string figureurl_1 { get; set; }
        //public string figureurl_2 { get; set; }
        public string figureurl_qq_1 { get; set; }
        //public string figureurl_qq_2 { get; set; }
        //public string gender { get; set; }
        //public string is_yellow_vip { get; set; }
        //public string vip { get; set; }
        //public string yellow_vip_level { get; set; }
        //public string level { get; set; }
        //public string is_yellow_year_vip { get; set; }
    }

}
