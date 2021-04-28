using System.Threading.Tasks;
using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);


        Task ClearLogo();

        Task ClearCustomCss();
    }
}
