using System;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement
{
    /// <summary>
    /// 产品密钥实体
    /// </summary>
    public class ProductSecretKey : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 密钥 (唯一)
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 关联的产品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 关联的产品助记码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 已使用？true=是，false=否
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        /// 使用 用户Id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 使用 用户账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public Guid? OrderId { get; set; }

        /// <summary>
        /// 订单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 在销售状态(销售状态下禁止删除和手动使用)
        /// </summary>
        public bool InSale { get; set; }

    }
}
