using System.ComponentModel;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement
{
    /// <summary>
    /// 订单来源类型
    /// </summary>
    public enum OrderSourceType
    {
        /// <summary>
        /// 产品
        /// </summary>
        [Description("产品")]
        Product=0,

        /// <summary>
        /// 课程
        /// </summary>
        [Description("课程")]
        Course=1,
        
    }
}
