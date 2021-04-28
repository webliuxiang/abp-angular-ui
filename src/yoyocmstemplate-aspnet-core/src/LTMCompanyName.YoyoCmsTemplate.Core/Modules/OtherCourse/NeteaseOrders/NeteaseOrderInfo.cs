using System;
using Abp.Domain.Entities;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders
{
    /// <summary>
    /// 网易云课堂订单
    /// </summary>
    public class NeteaseOrderInfo : Entity<long>
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单创建时间
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
        /// 商品描述
        /// </summary>
        public string ProductDes { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// 买家名称
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// 推广来源
        /// </summary>
        public string GeneralizeSource { get; set; }

        /// <summary>
        /// 推广者名称(可空)
        /// </summary>
        public string GeneralizeName { get; set; }

        /// <summary>
        /// 学习卡金额
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
        /// 渠道推广费用
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
        /// 到款/出账(结算状态)
        /// </summary>
        public string SettleAccounts { get; set; }

        /// <summary>
        /// 到款(结算时间)
        /// </summary>
        public DateTime? SettleAccountsDate { get; set; }


        /// <summary>
        /// 是否已经审核入群(QQ群)
        /// </summary>
        public bool IsChecked { get; set; }
        
        /// <summary>
        /// 码云审核
        /// </summary>
        public bool IsGiteeChecked { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public string Platform { get; set; }
    }
}
