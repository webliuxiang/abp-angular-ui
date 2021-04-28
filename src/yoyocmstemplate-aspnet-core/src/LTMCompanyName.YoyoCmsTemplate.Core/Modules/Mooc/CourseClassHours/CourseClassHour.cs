using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours
{
    /// <summary>
    /// 课时
    /// </summary>
    public class CourseClassHour : FullAuditedEntity<long>
    {
        /// <summary>
        /// 课时名称
        /// </summary>
        [StringLength(maximumLength: 256)]
        public string Name { get; set; }


        /// <summary>
        /// 课时简介
        /// </summary>
        [StringLength(maximumLength: 512)]
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
        [StringLength(maximumLength: 512)]
        public virtual string ResourceId { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public virtual CourseClassHourResourcTypeEnum ResourceType { get; set; }
    }
}
