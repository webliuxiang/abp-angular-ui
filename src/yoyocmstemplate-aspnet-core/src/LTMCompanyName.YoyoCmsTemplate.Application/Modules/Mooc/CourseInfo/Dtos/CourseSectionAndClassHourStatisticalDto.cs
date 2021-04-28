using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos
{
    /// <summary>
    /// 章节数量和课时数量统计dto
    /// </summary>
    public class CourseSectionAndClassHourStatisticalDto
    {
        /// <summary>
        /// 章节总数
        /// </summary>
        public int CourseSectionCount { get; set; }

        /// <summary>
        /// 课程总数
        /// </summary>
        public int CourseClassHourCount { get; set; }
    }
}
