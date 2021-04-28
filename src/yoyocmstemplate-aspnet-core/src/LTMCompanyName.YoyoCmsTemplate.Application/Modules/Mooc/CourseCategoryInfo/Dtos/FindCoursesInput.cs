using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos
{
    public class FindCoursesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

        /// <summary>
        /// 课程分类的Id
        /// </summary>
        public long CategoryId { get; set; }


        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        { 
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
            

        }
    }
}
