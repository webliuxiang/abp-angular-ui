using System;
using System.Threading;
using System.Threading.Tasks;
using Abp;
using Abp.Runtime.Caching;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LTMCompanyName.YoyoCmsTemplate.HealthChecks
{
    public class CacheHealthCheck : AbpServiceBase, IHealthCheck
    {
        private readonly ICacheManager _cacheManager;

        public CacheHealthCheck(ICacheManager cacheManager)
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;
            _cacheManager = cacheManager;
        }

        // This function tries to set and get data from cache.
        // If redis cache is enabled this will try to connect to redis to set and get cache data. If it will not throw an exception it means redis is up and healthy.
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var cacheManager = _cacheManager.GetCache("TestCache");

                var testKey = "Test-" + Guid.NewGuid();

                await cacheManager.SetAsync(testKey, "123");

                await cacheManager.GetOrDefaultAsync(testKey);

                return HealthCheckResult.Healthy("缓存检查是健康的。(如果你使用的是Redis, Redis也检查了)");
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy("缓存检查不健康。(如果你使用的是Redis, Redis也检查了)" + e.Message);
            }
        }
    }
}
