using System;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement
{
    /// <summary>
    /// 产品实体
    /// </summary>
    public class Product : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 产品Code(助记码)(唯一)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 项目创建数量
        /// </summary>
        public int CreateProjectCount { get; set; }
        /// <summary>
        /// 推荐？true=是，false=否
        /// </summary>
        public bool Recommended { get; set; }

        /// <summary>
        /// 有效期 单位(天)
        /// </summary>
        public double Indate { get; set; }

        /// <summary>
        /// 是否已发布(已发布的禁止修改)
        /// </summary>
        public bool Published { get; set; }
    }
}
