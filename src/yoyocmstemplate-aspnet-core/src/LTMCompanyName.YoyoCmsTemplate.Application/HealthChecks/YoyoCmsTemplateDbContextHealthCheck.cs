using System.Threading;
using System.Threading.Tasks;
using Abp;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LTMCompanyName.YoyoCmsTemplate.HealthChecks
{
    public class YoyoCmsTemplateDbContextHealthCheck : AbpServiceBase, IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public YoyoCmsTemplateDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("YoyoCmsTemplateDbContext已连接到数据库"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("YoyoCmsTemplateDbContext无法连接到数据库。"));
        }
    }
}
