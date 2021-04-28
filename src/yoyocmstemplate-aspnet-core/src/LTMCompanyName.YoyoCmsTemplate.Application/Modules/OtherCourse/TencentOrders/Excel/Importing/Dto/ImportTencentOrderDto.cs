namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing.Dto
{
    /// <summary>
    /// 导入订单的dto
    /// </summary>
    public class ImportTencentOrderDto
    {
        /// <summary>
        /// 机构id
        /// </summary>
      
        public long OrganizationId { get; set; }



        /// <summary>
        /// 订单号
        /// </summary>
      
        public long OrderNumber { get; set; }



        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseTitle { get; set; }



        /// <summary>
        /// 课程类型
        /// </summary>
        public string CourseType { get; set; }



        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime { get; set; }



        /// <summary>
        /// 支付时间
        /// </summary>
        public string PurchaseTime { get; set; }



        /// <summary>
        /// 购买Uin码
        /// </summary>
        public string UIn { get; set; }



        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }



        /// <summary>
        /// 交易状态
        /// </summary>
        public string TradingStatus { get; set; }



        /// <summary>
        /// 联系人电话
        /// </summary>
        public string MobileNumber { get; set; }



        /// <summary>
        /// 收件人电话
        /// </summary>
        public string RecipientPhone { get; set; }



        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string RecipientName { get; set; }



        /// <summary>
        /// 收件人地址
        /// </summary>
        public string RecipientAddress { get; set; }



        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeType { get; set; }



        /// <summary>
        /// 用户支付
        /// </summary>
        public decimal UsersPay { get; set; }



        /// <summary>
        /// 平台补贴
        /// </summary>
        public decimal PlatformSubsidies { get; set; }



        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; }



        /// <summary>
        /// 实算金额
        /// </summary>
        public decimal AmountPaid { get; set; }



        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal SettlementAmount { get; set; }



        /// <summary>
        /// 结算比例
        /// </summary>
        public decimal SettlementRatio { get; set; }



        /// <summary>
        /// 渠道费用
        /// </summary>
        public decimal ChannelFee { get; set; }



        /// <summary>
        /// 流量转化服务费
        /// </summary>
        public decimal TrafficConversionFee { get; set; }



        /// <summary>
        /// 苹果分成
        /// </summary>
        public decimal AppleShares { get; set; }



        /// <summary>
        /// 分销商分成
        /// </summary>
        public decimal DistributorShare { get; set; }



        /// <summary>
        /// 结算状态
        /// </summary>
        public string SettlementStatus { get; set; }



        /// <summary>
        /// 是否IOS订单
        /// </summary>
        public bool IsIosOrder { get; set; }



        /// <summary>
        /// 是否分销订单
        /// </summary>
        public bool IsDistributorOrder { get; set; }



        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType { get; set; }



        /// <summary>
        /// 备注1
        /// </summary>
        public string RemarkOne { get; set; }



        /// <summary>
        /// 备注2
        /// </summary>
        public string RemarkTwo { get; set; }



        /// <summary>
        /// 备注3
        /// </summary>
        public string RemarkThree { get; set; }



        /// <summary>
        /// 是否加入qq群
        /// </summary>
        public bool IsQqGroupChecked { get; set; }



        /// <summary>
        /// 码云是否通过
        /// </summary>
        public bool IsGiteeChecked { get; set; }



        /// <summary>
        /// 是否同步52abp
        /// </summary>
        public bool Is52AbpChecked { get; set; }



        /// <summary>
        /// 平台
        /// </summary>
        public string Platform { get; set; }



        /// <summary>
        /// 当从excel读取数据或导入信息
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }

    }
}
