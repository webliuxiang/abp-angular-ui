using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.Dtos
{
    /// <summary>
    /// 移动课时所属章节
    /// </summary>
    public class MoveCourseClassHourSectionDto
    {
        /// <summary>
        /// 课时id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 章节id
        /// </summary>
        public virtual long CourseSectionId { get; set; }
    }
}
