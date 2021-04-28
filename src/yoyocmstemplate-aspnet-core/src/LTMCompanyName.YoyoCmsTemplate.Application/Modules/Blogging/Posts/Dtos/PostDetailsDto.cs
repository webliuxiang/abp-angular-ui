using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Enums;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos
{
    /// <summary>
    /// 文章详情的DTO
    /// <see cref="Post"/>
    /// </summary>
    public class PostDetailsDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 博客Id
        /// </summary>
        public Guid BlogId { get; set; }

        /// <summary>
        /// 博客短名称
        /// </summary>
        public string BlogShortName { get; set; }

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

        public PostType PostType { get; set; }

        /// <summary>
        /// 评论总数
        /// </summary>
        public int CommentCount { get; set; }

        ///// <summary>
        ///// 作者
        ///// </summary>
        // public BlogUserDto Writer { get; set; }

        /// <summary>
        /// 文章标签
        /// </summary>
        public List<TagListDto> Tags { get; set; }

        [CanBeNull]
        public UserListDto Writer { get; set; }

        //// custom codes

        //// custom codes end
    }
}
