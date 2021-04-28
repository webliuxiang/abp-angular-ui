
using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos
{
	/// <summary>
	/// 获取课程分类的传入参数Dto
	/// </summary>
    public class GetCourseCategorysInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {


        public long? Id { get; set; }

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        { 
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "course.Title";
            }
            else if (Sorting.Contains("name"))
            {
                Sorting = Sorting.Replace("name", "course.Title");
            }
            else if (Sorting.Contains("addedTime"))
            {
                Sorting = Sorting.Replace("addedTime", "courseCate.CreationTime");
            }

        }
		
							//// custom codes
									
							

							//// custom codes end
    }
}
