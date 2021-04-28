using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Runtime.Caching;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Cachings.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Cachings
{
    public class HostCachingAppService : YoyoCmsTemplateAppServiceBase, IHostCachingAppService
    {
        private readonly ICacheManager _cacheManager;

        public HostCachingAppService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public ListResultDto<HostCacheDto> GetAllCaches()
        {
            var caches = _cacheManager.GetAllCaches()
                .Select(cache => new HostCacheDto
                {
                    Name = cache.Name
                })
                .ToList();

            return new ListResultDto<HostCacheDto>(caches);
        }

        public async Task ClearCache(EntityDto<string> input)
        {
            var cache = _cacheManager.GetCache(input.Id);
            await cache.ClearAsync();
        }

        public async Task ClearAllCaches()
        {
            var caches = _cacheManager.GetAllCaches();
            foreach (var cache in caches) await cache.ClearAsync();
        }
    }
}