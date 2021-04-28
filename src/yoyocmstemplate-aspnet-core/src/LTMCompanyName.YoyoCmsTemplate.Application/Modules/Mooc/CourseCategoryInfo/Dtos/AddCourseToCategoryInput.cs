using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos
{
    public class AddCourseToCategoryInput
    {
        public List<long> CourseIds { get; set; }

        [Range(1, long.MaxValue)]
        public long CourseCategoryId { get; set; }
    }
     
}
