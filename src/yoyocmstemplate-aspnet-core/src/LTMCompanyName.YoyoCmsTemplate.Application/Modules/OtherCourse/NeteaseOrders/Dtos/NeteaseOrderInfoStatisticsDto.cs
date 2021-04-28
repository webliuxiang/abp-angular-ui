namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Dtos
{
    public class NeteaseOrderInfoStatisticsDto
    {
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 实际收入
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 销售总额
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int BuyCount { get; set; }

        /// <summary>
        /// 服务费(手续费)
        /// </summary>
        public decimal ServiceCharge
        {
            get
            {
                return this.Sum - this.Value;
            }
        }
    }
}
