
using System;
using System.ComponentModel.DataAnnotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments;


namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos
{
    /// <summary>
    /// 评论的列表DTO
    /// <see cref="Comment"/>
    /// </summary>
    public class CommentEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        public Guid PostId { get; set; }

        /// <summary>
        /// 回复评论Id
        /// </summary>
        public Guid? RepliedCommentId { get; set; }



        /// <summary>
        /// 评论内容
        /// </summary>
        [Required(ErrorMessage = "评论内容不能为空")]
        public string Text { get; set; }




        //// custom codes



        //// custom codes end
    }
}
