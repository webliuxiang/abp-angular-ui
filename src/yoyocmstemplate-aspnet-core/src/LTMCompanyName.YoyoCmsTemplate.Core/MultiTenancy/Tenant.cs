using System;
using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;
using Abp.Timing;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public const int MaxLogoMimeTypeLength = 64;


        /// <summary>
        /// 订阅结束时间
        /// </summary>
        public DateTime? SubscriptionEndUtc { get; set; }

        /// <summary>
        /// 是否试用
        /// </summary>
        public bool IsInTrialPeriod { get; set; }

        /// <summary>
        /// 自定义cssId
        /// </summary>
        public virtual Guid? CustomCssId { get; set; }

        /// <summary>
        /// 自定义LogoId
        /// </summary>
        public virtual Guid? LogoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(MaxLogoMimeTypeLength)]
        public virtual string LogoFileType { get; set; }

        public Tenant()
        {
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {

        }


        public virtual bool HasLogo()
        {
            return LogoId != null && LogoFileType != null;
        }

        public void ClearLogo()
        {
            LogoId = null;
            LogoFileType = null;
        }

        /// <summary>
        /// 是否订阅结束
        /// </summary>
        /// <returns></returns>
        private bool IsSubscriptionEnded()
        {
            return SubscriptionEndUtc < Clock.Now.ToUniversalTime();
        }

        /// <summary>
        /// 剩余天数
        /// </summary>
        /// <returns></returns>
        public int CalculateRemainingDayCount()
        {
            return SubscriptionEndUtc != null ? (SubscriptionEndUtc.Value - Clock.Now.ToUniversalTime()).Days : 0;
        }

        /// <summary>
        /// 是否无限期订阅
        /// </summary>
        /// <returns></returns>
        public bool HasUnlimitedTimeSubscription()
        {
            return SubscriptionEndUtc == null;
        }
    }
}
