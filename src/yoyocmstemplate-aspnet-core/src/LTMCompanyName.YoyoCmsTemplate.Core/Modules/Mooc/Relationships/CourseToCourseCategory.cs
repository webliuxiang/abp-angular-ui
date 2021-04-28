using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.Relationships
{
    /// <summary>
    /// 视频和视频分类关联表
    /// </summary>
    public class CourseToCourseCategory : CreationAuditedEntity<long>
    {
        public virtual long CourseId { get; set; }

        public virtual long CourseCategoryId { get; set; }

        public virtual Course Course { get; set; }

        public virtual CourseCategory CourseCategory { get; set; }
    }
}
