using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections.Dtos
{

    /// <summary>
    /// 课程章节包含课时Dto
    /// </summary>
  public  class CourseSectionDetailsDto : CreationAuditedEntityDto<long?>
    {
        /// <summary>
        /// 章节名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 章节介绍
        /// </summary>
        public virtual string Intro { get; set; }

        /// <summary>
        /// 章节索引
        /// </summary>
        public virtual int Index { get; set; }

        /// <summary>
        /// 课程id
        /// </summary>
        public long CoursesId { get; set; }

        /// <summary>
        /// 课时列表
        /// </summary>
        public List<CourseClassHourDto> CourseClassHours { get; set; }

    }
}
