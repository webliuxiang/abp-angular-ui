using System;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.DomainService
{
    public interface IDownloadLogManager : I52AbpDomainService<DownloadLog>
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitDownloadLog();


        /// <summary>
        /// 创建新的下载记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Create(DownloadLog entity);

        /// <summary>
        /// 检查时间段内某个用户下载的次数是否大于目标次数
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="start">开始时间</param>
        /// <param name="stop">结束时间</param>
        /// <param name="count">目标次数</param>
        /// <returns>返回true表示大于目标次数，false表示小于目标次数</returns>
        Task<bool> CheckTimeRangDownloadCount(long? userId, DateTime start, DateTime stop, int count);

    }
}
