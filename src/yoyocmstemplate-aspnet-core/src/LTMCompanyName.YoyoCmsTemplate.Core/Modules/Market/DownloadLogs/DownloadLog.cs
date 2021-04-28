using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs
{
    public class DownloadLog : Entity<long>, IHasCreationTime
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 下载用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 下载的用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
