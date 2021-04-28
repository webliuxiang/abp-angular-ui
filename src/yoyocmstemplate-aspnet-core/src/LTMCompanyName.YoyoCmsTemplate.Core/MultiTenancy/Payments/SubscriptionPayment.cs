using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Editions;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Payments
{
    [Table("AppSubscriptionPayments")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class SubscriptionPayment : FullAuditedEntity<long>
    {
        public SubscriptionPaymentGatewayType Gateway { get; set; }

        public decimal Amount { get; set; }

        public SubscriptionPaymentStatus Status { get; set; }

        public int EditionId { get; set; }

        public int TenantId { get; set; }

        public int DayCount { get; set; }

        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public string PaymentId { get; set; }

        public virtual Edition Edition { get; set; }

        public string InvoiceNo { get; set; }

        public void Cancel()
        {
            if (Status == SubscriptionPaymentStatus.Processing)
            {
                Status = SubscriptionPaymentStatus.Cancelled;
            }
        }
    }

    public enum SubscriptionPaymentGatewayType
    {
        Paypal = 1
    }
    public enum SubscriptionPaymentStatus
    {
        Processing = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4
    }
    public enum PaymentPeriodType
    {
        Monthly = 30,
        Annual = 365
    }
}
