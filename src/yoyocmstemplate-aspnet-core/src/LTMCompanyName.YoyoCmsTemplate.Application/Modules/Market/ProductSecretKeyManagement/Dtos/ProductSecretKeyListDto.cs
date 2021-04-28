using System;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.Dtos
{
    public class ProductSecretKeyListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// 卡密
		/// </summary>
		public string SecretKey { get; set; }



		/// <summary>
		/// 产品助记码
		/// </summary>
		public string ProductCode { get; set; }



		/// <summary>
		/// 已使用
		/// </summary>
		public bool Used { get; set; }



		/// <summary>
		/// 用户Id
		/// </summary>
		public long? UserId { get; set; }



		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { get; set; }


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
