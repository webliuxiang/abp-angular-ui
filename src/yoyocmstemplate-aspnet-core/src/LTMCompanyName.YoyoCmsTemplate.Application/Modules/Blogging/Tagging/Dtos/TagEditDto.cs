using System;
using System.ComponentModel.DataAnnotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos
{
    /// <summary>
    /// 标签的列表DTO <see cref="Tag" />
    /// </summary>
    public class TagEditDto
    {
        /// <summary>
        /// 博客Id
        /// </summary>
        public virtual Guid? BlogId { get; set; }

        /// <summary>
        /// 文章Id
        /// </summary>
        public virtual Guid? PostId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        [MaxLength(100, ErrorMessage = "标签名称超出最大长度")]
        [Required(ErrorMessage = "标签名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 标签描述
        /// </summary>
        public string Description { get; set; }

        //// custom codes

        //// custom codes end
    }
}
