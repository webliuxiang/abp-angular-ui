using System;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.Dtos
{
    public class UserCenterOrderListDto : FullAuditedEntityDto<Guid>
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
        /// 价格
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 优惠
        /// </summary>
        public decimal Discounts { get; set; }

        /// <summary>
        /// 实付款
        /// </summary>
        public decimal ActualPayment { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusEnum Status { get; set; }

        /// <summary>
        /// 订单状态字符串
        /// </summary>
        public string StatusStr { get; set; }

        /// <summary>
        /// 产品的项目创建数量
        /// </summary>
        public int ProductCreateProjectCount { get; set; }

        /// <summary>
        /// 产品的有效期 单位(天)
        /// </summary>
        public double ProductIndate { get; set; }

       

    }
}
