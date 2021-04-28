

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos
{
    /// <summary>
    /// 创建或更新课程的dto
    /// </summary>
    public class CreateOrUpdateCourseDto
    {
        /// <summary>
        /// 课程信息
        /// </summary>
        [Required]
        public CourseDto Entity { get; set; }

        /// <summary>
        /// 分类id集合
        /// </summary>
        public List<long> CategoryIds { get; set; }


        /// <summary>
        /// 课程显示状态
        /// </summary>
        public List<KeyValuePair<string, string>> CourseDisplayTypeEnum { get; set; }
        /// <summary>
        /// 课程类型
        /// </summary>
        public List<KeyValuePair<string, string>> CourseVideoTypeEnum { get; set; }
        /// <summary>
        /// 课程状态
        /// </summary>
        public List<KeyValuePair<string, string>> CourseStateEnum { get; set; }
    }
}
