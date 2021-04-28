

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments;


namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos
{
    /// <summary>
    /// 评论的编辑DTO
    /// <see cref="Comment"/>
    /// </summary>
    public class CommentListDto : FullAuditedEntityDto<Guid>
    {


        /// <summary>
        /// 文章Id
        /// </summary>
        public Guid PostId { get; set; }



        /// <summary>
        /// 回复评论Id
        /// </summary>
        public Guid? RepliedCommentId { get; set; }

        /// <summary>
        /// 回复评论
        /// </summary>
        public string RepliedCommentTest { get; set; }



        /// <summary>
        /// 评论内容
        /// </summary>
        [Required(ErrorMessage = "评论内容不能为空")]
        public string Text { get; set; }


        /// <summary>
        /// 文章Title
        /// </summary>
        public string Title { get; set; }

        //// custom codes



        //// custom codes end
    }
}
