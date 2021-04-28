
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos
{
    /// <summary>
    /// 博客的列表DTO
    /// <see cref="Blog"/>
    /// </summary>
    public class BlogEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }



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

        /// <summary>
        /// 博客的所属用户
        /// </summary>
        public long BlogUserId { get; set; }


        /// <summary>
        /// 标签
        /// </summary>
        public List<Guid> TagIds { get; set; }


        //// custom codes



        //// custom codes end
    }
}
