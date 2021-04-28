using System.Collections.Generic;
using L._52ABP.Common.Extensions.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos
{
    /// <summary>
    /// 课程详情内容
    /// </summary>
    public class CourseDetailsDto : CourseDto
    { /// <summary>
      /// 课程显示状态 字符串
      /// </summary>
        public virtual string TypeString => Type.ToDescription();

        /// <summary>
        /// 课程视频类型 字符串
        /// </summary>
        public virtual string CourseVideoTypeString => CourseVideoType.ToDescription();

        /// <summary>
        /// 课程状态 字符串
        /// </summary>
        public virtual string CourseStateString => CourseState.ToDescription();


        /// <summary>
        /// 课程章节
        /// </summary>
        public List<CourseSectionDetailsDto> CourseSections { get; set; }
    }
}
