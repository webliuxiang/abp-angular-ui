using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos
{
    public class CreateOrUpdateCourseInput
    {
        [Required]
        public CourseEditDto Course { get; set; }

    }
}
