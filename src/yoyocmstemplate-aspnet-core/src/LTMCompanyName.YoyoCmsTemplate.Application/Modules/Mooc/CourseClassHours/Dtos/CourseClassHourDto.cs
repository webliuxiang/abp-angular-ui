using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.Dtos
{
    /// <summary>
    /// 课时Dto
    /// </summary>
    public class CourseClassHourDto : FullAuditedEntityDto<long?>
    {
        /// <summary>
        /// 课时名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 课时简介
        /// </summary>
        public virtual string Intro { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public long SortNumber { get; set; }


        /// <summary>
        /// 所属课程id
        /// </summary>
        public virtual long CourseId { get; set; }

        /// <summary>
        /// 所属章节id
        /// </summary>
        public virtual long CourseSectionId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public virtual string ResourceId { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public virtual CourseClassHourResourcTypeEnum ResourceType { get; set; }
    }
}
