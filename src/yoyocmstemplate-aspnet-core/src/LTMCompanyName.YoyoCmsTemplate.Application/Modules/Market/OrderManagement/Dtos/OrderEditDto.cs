using System;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.Dtos
{
    public class OrderEditDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

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
        /// 订单类型
        /// </summary>
        public OrderSourceType OrderSourceType { get; set; }


    }
}
