using System;
using System.Collections.Generic;
using System.Text;

namespace YoyoSoft.Mooc.CourseManagement.Dto
{
    /// <summary>
    /// 课程数量统计
    /// </summary>
    public class CourseStatisticsDto
    {
        /// <summary>
        /// 课程总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 未发布课程数
        /// </summary>
        public int WaitPublishCount { get; set; }
        /// <summary>
        /// 已发布课程数
        /// </summary>
        public int PublishCount { get; set; }
        /// <summary>
        /// 已官币课程数
        /// </summary>
        public int ClosedCount { get; set; }
    }
}
