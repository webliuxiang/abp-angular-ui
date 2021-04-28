using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Web.Models.AbpUserConfiguration;
using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        /// <summary>
        /// 获取当前登陆信息
        /// </summary>
        /// <returns></returns>
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        /// <summary>
        /// 更新当前用户的Token信息
        /// </summary>
        /// <returns></returns>
        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();

        /// <summary>
        /// 获取用户配置信息
        /// </summary>
        /// <returns></returns>
        Task<AbpUserConfigurationDto> GetUserConfigurations();
    }
}