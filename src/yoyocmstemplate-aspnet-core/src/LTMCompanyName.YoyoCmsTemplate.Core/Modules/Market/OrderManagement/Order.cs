using System;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement
{
    /// <summary>
    /// 订单实体
    /// </summary>
    public class Order : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 订单编码
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid? ProductId { get; set; }



        /// <summary>
        ///课程Id
        /// </summary>
        public long? CourseId { get; set; }
       
        /// <summary>
        /// 产品助记码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Discounts { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal ActualPayment { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusEnum Status { get; set; }

        /// <summary>
        /// 产品的项目创建数量
        /// </summary>
        public int ProductCreateProjectCount { get; set; }

        /// <summary>
        /// 产品的有效期 单位(天)
        /// </summary>
        public double ProductIndate { get; set; }

        /// <summary>
        /// 订单关联的用户Id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 订单关联的UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderSourceType OrderSourceType { get; set; }


    }
}
