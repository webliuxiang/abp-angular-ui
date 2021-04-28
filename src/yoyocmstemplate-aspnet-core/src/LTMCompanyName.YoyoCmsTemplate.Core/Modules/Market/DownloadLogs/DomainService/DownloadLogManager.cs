using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.DomainService
{
    /// <summary>
    /// DownloadLog领域层的业务管理
    ///</summary>
    public class DownloadLogManager : AbpDomainService<DownloadLog>, IDownloadLogManager
    {


        public DownloadLogManager(IRepository<DownloadLog, long> entityRepo) 
            : base(entityRepo)
        {
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitDownloadLog()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码



        public async Task Create(DownloadLog entity)
        {
            await this.EntityRepo.InsertAsync(entity);
        }

        public async Task<bool> CheckTimeRangDownloadCount(long? userId, DateTime start, DateTime stop, int count)
        {
            var queryCount = await this.QueryAsNoTracking
                .Where(o => o.UserId == userId && o.CreationTime >= start && o.CreationTime < stop)
                .CountAsync();

            return queryCount >= count;
        }

    }
}
