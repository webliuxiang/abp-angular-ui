using System;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Dtos
{
    public class NeteaseOrderInfoListDto : EntityDto<long> 
    {

        
		/// <summary>
		/// 订单编号
		/// </summary>
		public string OrderNo { get; set; }



		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? OrderDate { get; set; }



		/// <summary>
		/// 商品名称
		/// </summary>
		public string ProductName { get; set; }



		/// <summary>
		/// 原价
		/// </summary>
		public decimal OriginalPrice { get; set; }



		/// <summary>
		/// 现价
		/// </summary>
		public decimal TransactionAmount { get; set; }



		/// <summary>
		/// 买家
		/// </summary>
		public string BuyerName { get; set; }



		/// <summary>
		/// 支付方式
		/// </summary>
		public string PayType { get; set; }



		/// <summary>
		/// 实际收入
		/// </summary>
		public decimal RealityAmount { get; set; }



		/// <summary>
		/// 交易状态
		/// </summary>
		public string TransactionStatus { get; set; }



		/// <summary>
		/// QQ已审核
		/// </summary>
		public bool IsChecked { get; set; }

        /// <summary>
        /// 码云已审核
        /// </summary>
        public bool IsGiteeChecked { get; set; }


        /// <summary>
        /// 平台
        /// </summary>
        public string Platform { get; set; }




    }
}
