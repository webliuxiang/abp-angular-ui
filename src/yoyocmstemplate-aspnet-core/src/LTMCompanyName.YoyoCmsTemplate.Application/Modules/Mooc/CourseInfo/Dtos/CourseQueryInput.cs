using System;
using System.Collections.Generic;
using System.Text;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos
{
    public class CourseQueryInput: QueryInput
    {

        public virtual string Sorting { get; set; }

        public CourseQueryInput()
        {
            
        }

    }
}
