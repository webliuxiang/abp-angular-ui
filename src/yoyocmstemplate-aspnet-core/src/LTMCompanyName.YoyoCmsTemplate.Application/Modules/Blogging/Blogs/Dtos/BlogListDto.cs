

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos
{
    /// <summary>
    /// 博客的编辑DTO
    /// <see cref="Blog"/>
    /// </summary>
    public class BlogListDto : FullAuditedEntityDto<Guid>
    {


        /// <summary>
        /// 博客名称
        /// </summary>
        [Required(ErrorMessage = "博客名称不能为空")]
        public string Name { get; set; }



        /// <summary>
        /// 博客短名称
        /// </summary>
        [Required(ErrorMessage = "博客短名称不能为空")]
        public string ShortName { get; set; }



        /// <summary>
        /// 博客描述
        /// </summary>
        public string Description { get; set; }


        public string BlogUserName { get; set; }


        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }

        //// custom codes



        //// custom codes end
    }
}
