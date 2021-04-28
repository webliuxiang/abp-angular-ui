
using System;

namespace  LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Dtos
{
    public class NeteaseOrderInfoEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }         


        
		/// <summary>
		/// 订单编号
		/// </summary>
		public string OrderNo { get; set; }



		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? OrderDate { get; set; }



		/// <summary>
		/// 商品类型
		/// </summary>
		public string ProductType { get; set; }



		/// <summary>
		/// 商品名称
		/// </summary>
		public string ProductName { get; set; }



		/// <summary>
		/// 商品详细
		/// </summary>
		public string ProductDes { get; set; }



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
		/// 推广来源
		/// </summary>
		public string GeneralizeSource { get; set; }



		/// <summary>
		/// 推广者
		/// </summary>
		public string GeneralizeName { get; set; }



		/// <summary>
		/// 学习卡
		/// </summary>
		public decimal CardAmount { get; set; }



		/// <summary>
		/// 平台红包
		/// </summary>
		public decimal PlatformHB { get; set; }



		/// <summary>
		/// 实际付款
		/// </summary>
		public decimal PracticalAmount { get; set; }



		/// <summary>
		/// 支付方式
		/// </summary>
		public string PayType { get; set; }



		/// <summary>
		/// 第三方支付费用
		/// </summary>
		public decimal ThirdPartyPayServiceCharge { get; set; }



		/// <summary>
		/// 推广费用
		/// </summary>
		public decimal GeneralizeServiceCharge { get; set; }



		/// <summary>
		/// 平台服务费
		/// </summary>
		public decimal PlatformServiceCharge { get; set; }



		/// <summary>
		/// 实际收入
		/// </summary>
		public decimal RealityAmount { get; set; }



		/// <summary>
		/// 交易状态
		/// </summary>
		public string TransactionStatus { get; set; }



		/// <summary>
		/// 交易成功时间
		/// </summary>
		public DateTime? TransactionSuccessDate { get; set; }



		/// <summary>
		/// 结算状态
		/// </summary>
		public string SettleAccounts { get; set; }



		/// <summary>
		/// 结算时间
		/// </summary>
		public DateTime? SettleAccountsDate { get; set; }



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
