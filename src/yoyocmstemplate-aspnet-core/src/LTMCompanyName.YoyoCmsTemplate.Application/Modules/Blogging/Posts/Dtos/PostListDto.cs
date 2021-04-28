using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using L._52ABP.Common.Extensions.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Enums;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos
{
    /// <summary>
    /// 文章的编辑DTO <see cref="Post" />
    /// </summary>
    public class PostListDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 博客Id
        /// </summary>
        public Guid BlogId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>

        public string Title { get; set; }

        /// <summary>
        /// 地址
        /// </summary>

        public string Url { get; set; }

        /// <summary>
        /// 封面
        /// </summary>

        public string CoverImage { get; set; }

        /// <summary>
        /// 内容
        /// </summary>

        public string Content { get; set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        public int ReadCount { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        [Required(ErrorMessage = "文章类型不能为空")]
        public PostType PostType { get; set; }

        /// <summary>
        /// 文章类型描述
        /// </summary>
        public string PostTypeDescirption => PostType.ToDescription();

        /// <summary>
        /// 文章标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 博客名称
        /// </summary>
        public string BlogName { get; set; }

        //// custom codes

        //// custom codes end
    }
}
