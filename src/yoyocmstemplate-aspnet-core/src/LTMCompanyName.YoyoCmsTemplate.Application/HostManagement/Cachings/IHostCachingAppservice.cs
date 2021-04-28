using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Cachings.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Cachings
{
    public interface IHostCachingAppService
    {
        ListResultDto<HostCacheDto> GetAllCaches();

        Task ClearCache(EntityDto<string> input);

        Task ClearAllCaches();
    }
}