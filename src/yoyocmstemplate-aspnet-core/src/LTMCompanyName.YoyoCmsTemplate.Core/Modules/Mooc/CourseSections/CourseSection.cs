using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Abp.Domain.Entities.Auditing;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.Relationships;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections
{

    /// <summary>
    /// 章节信息
    /// </summary>
    public class CourseSection : CreationAuditedEntity<long>
    {
        /// <summary>
        /// 章节名称
        /// </summary>
        [StringLength(maximumLength: 250, MinimumLength = 2)]
        public virtual string Name { get; set; }


        /// <summary>
        /// 章节介绍
        /// </summary>
        [StringLength(maximumLength: 250)]
        public virtual string Intro { get; set; }

        /// <summary>
        /// 章节索引
        /// </summary>
        public virtual int Index { get; set; }

        /// <summary>
        /// 课程id
        /// </summary>
        public long CoursesId { get; set; }
    }
}
