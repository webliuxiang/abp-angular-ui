

using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo;
using System.Collections.ObjectModel;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos
{	
	/// <summary>
	/// 课程分类的列表DTO
	/// <see cref="CourseCategory"/>
	/// </summary>
    public class CourseCategoryListDto : CreationAuditedEntityDto<long> 
    {

        
		/// <summary>
		/// 名称
		/// </summary>
		[MaxLength(250, ErrorMessage="名称超出最大长度")]
		[MinLength(2, ErrorMessage="名称小于最小长度")]
		public string Name { get; set; }



		/// <summary>
		/// 编码
		/// </summary>
		[MaxLength(200, ErrorMessage="编码超出最大长度")]
		public string Code { get; set; }



		/// <summary>
		/// 图片路径
		/// </summary>
		public string ImgUrl { get; set; }



		/// <summary>
		/// 父类Id
		/// </summary>
		public long? ParentId { get; set; }

        /// <summary>
        /// 课程总数量
        /// </summary>
        public int CourseCount { get; set; }

		
							//// custom codes
									
							

							//// custom codes end
    }
}
