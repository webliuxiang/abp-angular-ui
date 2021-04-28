using System;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos
{
    /// <summary>
    /// 付款到订单的视图模型
    /// </summary>
    public class PurchaseToOrderDto
    {
 

        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid? ProductId { get; set; }



        /// <summary>
        ///课程Id
        /// </summary>
        public long? CourseId { get; set; }

        /// <summary>
        /// 助记码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 产品的项目创建数量
        /// </summary>
        public int ProductCreateProjectCount { get; set; }

        /// <summary>
        /// 产品的有效期 单位(天)
        /// </summary>
        public double ProductIndate { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal ActualPayment { get; set; }
       


        



    }
}
