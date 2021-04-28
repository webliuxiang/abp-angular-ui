
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using System.Collections.ObjectModel;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo;


namespace  LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos
{
	/// <summary>
	/// 课程分类的列表DTO
	/// <see cref="CourseCategory"/>
	/// </summary>
    public class CourseCategoryEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }         


        
		/// <summary>
		/// 名称
		/// </summary>
		[MaxLength(250, ErrorMessage="名称超出最大长度")]
		[MinLength(2, ErrorMessage="名称小于最小长度")]
		public string Name { get; set; }






		/// <summary>
		/// 图片路径
		/// </summary>
		public string ImgUrl { get; set; }



		/// <summary>
		/// 父类Id
		/// </summary>
		public long? ParentId { get; set; }


         

		
							//// custom codes
									
							

							//// custom codes end
    }
}
