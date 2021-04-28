using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.Relationships;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo
{
    public class CourseCategory : CreationAuditedEntity<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(maximumLength:250,MinimumLength = 2)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(maximumLength:250)]
        public virtual string Code { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [StringLength(maximumLength:250)]
        public virtual string ImgUrl { get; set; }


        public virtual long? ParentId { get; set; }

        public virtual ICollection<CourseCategory> Children { get; set; }

        /// <summary>
        /// 课程关联
        /// </summary>
        public virtual ICollection<CourseToCourseCategory> CourseToCourseCategory { get; set; }




    }
}
