using System;
using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos
{
    public class GetCoursesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {


        /// <summary>
        /// 分类Id
        /// </summary>
        public long? CatetoryId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public CourseStateEnum? CourseState { get; set; }
        /// <summary>
        /// 课程视频类型-直播、录播
        /// </summary>
        public CourseVideoTypeEnum? CourseVideoType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

    }
}
