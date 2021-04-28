using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs
{
    /// <summary>
    /// 用户项目下载信息配置 实体
    /// </summary>
    public class UserDownloadConfig : Entity<long>, ICreationAudited, IModificationAudited, IDeletionAudited
    {
        /// <summary>
        /// 产品助记码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 下载类型
        /// </summary>
        public string DownloadType { get; set; }

        /// <summary>
        /// 剩余下载次数
        /// </summary>
        public long ResidueDegree { get; set; }

        /// <summary>
        /// 开始记录时间 (重置是需要置空)
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 有效期(天)
        /// 
        /// (注:若当天时间大于或等于开始时间+有效期，则有效期清零,下载次数清零)
        /// </summary>
        public double Indate { get; set; }

        /// <summary>
        /// 用户Id
        /// (每一个用户只有一个下载配置信息)
        /// (小于等于0则不正确,没有Id小于0的用户)
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }


        /// <summary>
        /// 重置配置信息
        /// </summary>
        public void Reset()
        {
            this.ResidueDegree = 0;
            this.StartTime = null;
            this.Indate = 0;
        }

        /// <summary>
        /// 是否有效(是否可以创建项目)
        /// </summary>
        /// <returns></returns>
        public bool IsEfficient()
        {
            return this.ResidueDegree > 0;
            //// 数据校验
            //if (!this.StartTime.HasValue || this.ResidueDegree <= 0 || this.Indate <= 0)
            //{
            //    return false;
            //}
            //try
            //{
            //    // 用户开始开始时间+有效期时间 大于 当前时间表示有效
            //    var userHaveDate = this.StartTime.Value.AddDays(this.Indate);
            //    return userHaveDate > DateTime.Now;
            //}
            //catch// 如果抛出异常,表示用户开始时间+有效期时间超出界限,必定是有效的
            //{
            //    return true;
            //}
        }

        /// <summary>
        /// 获取剩余天数
        /// </summary>
        /// <returns></returns>
        public long RemainingDays()
        {
            if (!this.StartTime.HasValue || this.Indate <= 0)
            {
                return 0;
            }

            return (DateTime.Now - this.StartTime.Value.AddDays(this.Indate)).Days;
        }
    }
}
