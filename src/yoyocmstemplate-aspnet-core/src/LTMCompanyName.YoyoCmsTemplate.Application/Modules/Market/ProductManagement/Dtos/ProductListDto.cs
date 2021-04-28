using System;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos
{
    public class ProductListDto : FullAuditedEntityDto<Guid> 
    {

        
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
		/// 创建项目次数
		/// </summary>
		public int CreateProjectCount { get; set; }



		/// <summary>
		/// 推荐
		/// </summary>
		public bool Recommended { get; set; }



		/// <summary>
		/// 有效天数
		/// </summary>
		public double Indate { get; set; }



		/// <summary>
		/// 产品助记码
		/// </summary>
		public string Code { get; set; }



        /// <summary>
        /// 是否已发布(已发布的禁止修改)
        /// </summary>
        public bool Published { get; set; }

    }
}
