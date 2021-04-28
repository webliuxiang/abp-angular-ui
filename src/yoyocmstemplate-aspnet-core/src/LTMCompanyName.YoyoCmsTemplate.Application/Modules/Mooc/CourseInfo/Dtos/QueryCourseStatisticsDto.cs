using Abp.Application.Services.Dto;
using L._52ABP.Common.Extensions.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;

namespace YoyoSoft.Mooc.CourseManagement.Dto
{
    public class QueryCourseStatisticsDto : EntityDto<long>
    {
        /// <summary>
        /// 课程标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 课程视频类型-直播、录播
        /// </summary>
        public CourseVideoTypeEnum CourseVideoType { get; set; }
        /// <summary>
        /// 课程视频类型-直播、录播
        /// </summary>
        public string CourseVideoTypeName => CourseVideoType.ToDescription();
        /// <summary>
        /// 课程状态
        /// </summary>
        public CourseStateEnum CourseState { get; set; }
        /// <summary>
        /// 课程状态
        /// </summary>
        public string CourseStateName => CourseState.ToDescription();
        /// <summary>
        /// 课程时长
        /// </summary>
        public decimal TotalTime { get; set; }
        /// <summary>
        /// 学员数
        /// </summary>
        public int StudentCount { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public decimal TotalMoney { get; set; }
    }
}
