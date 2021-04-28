using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using L._52ABP.Common.Extensions.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos
{
    /// <summary>
    /// 课程列表Dto
    /// </summary>
    public class CourseListDto : CourseDto
    {
        /// <summary>
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
    }
}

