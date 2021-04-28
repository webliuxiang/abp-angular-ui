using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Runtime.Session;
using Abp.Web.Configuration;
using Abp.Web.Models.AbpUserConfiguration;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;
using LTMCompanyName.YoyoCmsTemplate.SignalR;

namespace LTMCompanyName.YoyoCmsTemplate.Sessions
{
    public class SessionAppService : YoyoCmsTemplateAppServiceBase, ISessionAppService
    {
        private readonly AbpUserConfigurationBuilder _abpUserConfigurationBuilder;

        public SessionAppService(AbpUserConfigurationBuilder abpUserConfigurationBuilder)
        {
            _abpUserConfigurationBuilder = abpUserConfigurationBuilder;
        }

        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>
                    {
                        {"SignalR", SignalRFeature.IsAvailable},
                        {"SignalR.AspNetCore", SignalRFeature.IsAspNetCore},
                    }
                }
            };
            if (AbpSession.TenantId.HasValue)
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());

            if (AbpSession.UserId.HasValue)
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());


            return output;
        }

        public async Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken()
        {
            if (AbpSession.UserId <= 0) throw new Exception(L("ThereIsNoLoggedInUser"));

            var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
            user.SetSignInToken();
            return new UpdateUserSignInTokenOutput
            {
                SignInToken = user.SignInToken,
                EncodedUserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id.ToString())),
                EncodedTenantId = user.TenantId.HasValue
                    ? Convert.ToBase64String(Encoding.UTF8.GetBytes(user.TenantId.Value.ToString()))
                    : ""
            };
        }

        [DisableAuditing]
        public async Task<AbpUserConfigurationDto> GetUserConfigurations()
        {
            var userConfig = await _abpUserConfigurationBuilder.GetAll();
            return userConfig;
        }
    }
}
