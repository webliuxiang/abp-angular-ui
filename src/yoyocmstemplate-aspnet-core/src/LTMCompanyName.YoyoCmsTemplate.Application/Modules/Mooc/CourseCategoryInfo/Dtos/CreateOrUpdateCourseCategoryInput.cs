

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos
{
    public class CreateOrUpdateCourseCategoryInput
    {
        [Required]
        public CourseCategoryEditDto CourseCategory { get; set; }
							
							//// custom codes
									
							

							//// custom codes end
    }
}