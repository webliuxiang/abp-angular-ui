using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.Dtos
{
    /// <summary>
    /// 交换课时索引Dto,将课时A的索引与课时B交换
    /// </summary>
    public class CourseClassHourExchangeIndexDto
    {
        /// <summary>
        /// 课时A
        /// </summary>
        public virtual long AId { get; set; }

        /// <summary>
        /// 课时B
        /// </summary>
        public virtual long BId { get; set; }
    }
}
