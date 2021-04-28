using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections.Dtos
{
    /// <summary>
    /// 交换章节索引Dto,将章节A的索引与章节B交换
    /// </summary>
    public class CourseSectionExchangeIndexDto
    {
        /// <summary>
        /// 章节A
        /// </summary>
        public virtual long AId { get; set; }

        /// <summary>
        /// 章节B
        /// </summary>
        public virtual long BId { get; set; }

    }
}
