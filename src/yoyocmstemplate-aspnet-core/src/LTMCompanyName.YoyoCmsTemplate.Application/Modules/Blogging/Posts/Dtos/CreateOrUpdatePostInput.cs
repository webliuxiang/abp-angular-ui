using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos
{
    public class CreateOrUpdatePostInput
    {
        [Required]
        public PostEditDto Post { get; set; }

        //// custom codes



        //// custom codes end
    }
}