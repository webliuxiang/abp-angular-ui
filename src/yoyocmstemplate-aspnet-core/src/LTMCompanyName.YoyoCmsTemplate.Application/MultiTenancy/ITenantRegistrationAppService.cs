using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy
{
    public interface ITenantRegistrationAppService
    {
        /// <summary>
        /// 注册租户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RegisterTenantResultDto> RegisterTenantAsync(CreateTenantDto input);
    }
}
