

using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos
{
    public class GetCourseForEditOutput
    {

        public CourseEditDto Course { get; set; }

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
