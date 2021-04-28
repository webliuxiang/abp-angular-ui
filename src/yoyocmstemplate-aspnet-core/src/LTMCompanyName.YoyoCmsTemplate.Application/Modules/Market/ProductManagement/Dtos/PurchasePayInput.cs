using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class PurchasePayInput
    {
        /// <summary>
        /// 产品代码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 是否移动端
        /// </summary>
        public bool IsMobile { get; set; }

        /// <summary>
        /// 订单编码
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 订单来源类型
        /// </summary>
        public  OrderSourceType OrderSourceType { get; set; }


    }
}
