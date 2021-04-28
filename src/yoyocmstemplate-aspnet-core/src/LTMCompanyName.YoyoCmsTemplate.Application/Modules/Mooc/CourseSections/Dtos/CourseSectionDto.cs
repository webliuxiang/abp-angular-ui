using System;
using System.Collections.Generic;
using System.Text;

using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections.Dtos
{
    /// <summary>
    /// 章节dto
    /// </summary>
    public class CourseSectionDto : CreationAuditedEntityDto<long?>
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
        /// 前端折叠面板使用 不涉及业务
        /// </summary>
        public bool IsHidden { get; set; } = true;
    }
}
