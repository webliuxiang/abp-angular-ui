using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;

namespace YoyoSoft.Mooc.CourseManagement.Dto
{
    public class QueryCourseStatisticsInput : PagedSortedAndFilteredInputDto
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public long? CatetoryId { get; set; }
        /// <summary>
        /// 课程视频类型-直播、录播
        /// </summary>
        public CourseVideoTypeEnum? CourseVideoType { get; set; }
    }
}
