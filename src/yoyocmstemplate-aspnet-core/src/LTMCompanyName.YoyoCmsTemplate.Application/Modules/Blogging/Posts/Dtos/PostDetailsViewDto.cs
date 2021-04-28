using System;
using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Enums;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Dtos
{
    public class PostDetailsViewDto
    {
        public Guid BlogId { get; set; }
        public Guid PostId { get; set; }

        public string Url { get; set; }

        public string CoverImage { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int ReadCount { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 个人头像Id
        /// </summary>
        public Guid? ProfilePictureId { get; set; }

        public long BlogUserId { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 时间差
        /// </summary>
        public string TiemDifference { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public PostType PostType { get; set; }


        /// <summary>
        /// 文章标签
        /// </summary>
        public List<TagListDto> Tags { get; set; }
    }
}
