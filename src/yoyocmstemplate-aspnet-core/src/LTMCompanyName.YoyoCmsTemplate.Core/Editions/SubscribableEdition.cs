using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Editions;

namespace LTMCompanyName.YoyoCmsTemplate.Editions
{
    /// <summary>
    /// 扩展版本订阅功能
    /// </summary>
    public class SubscribableEdition : Edition
    {
        /// <summary>
        /// 指定过期后的版本
        /// </summary>
        public int? ExpiringEditionId { get; set; }

        public decimal? MonthlyPrice { get; set; }

        public decimal? AnnualPrice { get; set; }

        public int? TrialDayCount { get; set; }

        /// <summary>
        /// 帐户将在订阅过期的指定日期后采取行动(终止租户帐户)
        /// </summary>
        public int? WaitingDayAfterExpire { get; set; }

        [NotMapped]
        public bool IsFree => !MonthlyPrice.HasValue && !AnnualPrice.HasValue;

        public bool HasTrial()
        {
            if (IsFree)
            {
                return false;
            }

            return TrialDayCount.HasValue && TrialDayCount.Value > 0;
        }



        public bool HasSamePrice(SubscribableEdition edition)
        {
            return !IsFree &&
                   MonthlyPrice == edition.MonthlyPrice && AnnualPrice == edition.AnnualPrice;
        }
    }
}