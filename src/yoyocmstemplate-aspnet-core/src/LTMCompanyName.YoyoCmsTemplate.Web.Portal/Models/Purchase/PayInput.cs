namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Models.Purchase
{
    public class PayInput
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
    }
}
