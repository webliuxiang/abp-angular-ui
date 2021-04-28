using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos
{
    public class DownloadLogListDto : EntityDto<long>,IHasCreationTime 
    {

        
		/// <summary>
		/// 版本号
		/// </summary>
		public string Version { get; set; }



		/// <summary>
		/// 类型
		/// </summary>
		public string Type { get; set; }



		/// <summary>
		/// 用户Id
		/// </summary>
		public long UserId { get; set; }



		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; }



		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreationTime { get; set; }


        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

    }
}
