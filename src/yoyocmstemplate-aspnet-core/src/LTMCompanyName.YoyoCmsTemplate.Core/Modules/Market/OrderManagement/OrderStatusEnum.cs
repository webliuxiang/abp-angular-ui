using System.ComponentModel;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement
{
    /// <summary>
    /// 订单状态枚举
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// 等待支付
        /// </summary>
        [Description("等待支付")]
        WaitForPayment,
        /// <summary>
        /// 等待发货
        /// </summary>
        [Description("等待发货")]
        WaitForDelivery,
        /// <summary>
        /// 交易成功
        /// </summary>
        [Description("交易成功")]
        ChangeHands,
        /// <summary>
        /// 交易关闭
        /// </summary>
        [Description("交易关闭")]
        TradingClosed,
    }
}
