namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.Dtos
{

    /// <summary>
    /// 订单统计信息
    /// </summary>
    public class OrderStatisticsDto
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 实际收入
        /// </summary>
        public decimal ActualPayment { get; set; }

       
        /// <summary>
        /// 按月统计
        /// </summary>
        public string  YearMonth { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int BuyCount { get; set; }

        /// <summary>
        /// 订单来源类型
        /// </summary>
        public OrderSourceType OrderSourceType { get; set; }


    }
}
