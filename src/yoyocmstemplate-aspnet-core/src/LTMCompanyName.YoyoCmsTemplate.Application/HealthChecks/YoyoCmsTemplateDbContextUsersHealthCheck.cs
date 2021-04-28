using System;
using System.Threading;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LTMCompanyName.YoyoCmsTemplate.HealthChecks
{
    /// <summary>
    /// 
    /// </summary>
    public class YoyoCmsTemplateDbContextUsersHealthCheck : AbpServiceBase, IHealthCheck
    {
        private readonly IDbContextProvider<YoyoCmsTemplateDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContextProvider"></param>
        /// <param name="unitOfWorkManager"></param>
        public YoyoCmsTemplateDbContextUsersHealthCheck(
            IDbContextProvider<YoyoCmsTemplateDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    // Switching to host is necessary for single tenant mode.
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        if (!await _dbContextProvider.GetDbContext().Database.CanConnectAsync(cancellationToken))
                        {
                            return HealthCheckResult.Unhealthy("YoyoCmsTemplateDbContext无法连接到数据库"
                            );
                        }

                        var user = await _dbContextProvider.GetDbContext().Users.AnyAsync(cancellationToken);
                        uow.Complete();

                        if (user)
                        {
                            return HealthCheckResult.Healthy("YoyoCmsTemplateDbContext已连接到数据库和检查用户是否添加");
                        }

                        return HealthCheckResult.Unhealthy("YoyoCmsTemplateDbContext连接到数据库，但没有用户。");

                    }
                }
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy(L("YoyoCmsTemplateDbContextCouldNotConnectToDatabase"), e);
            }
        }
    }
}
