using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos
{
    public class CreateOrUpdateCommentInput
    {
        [Required]
        public CommentEditDto Comment { get; set; }

        //// custom codes



        //// custom codes end
    }
}