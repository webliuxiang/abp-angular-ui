
using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Dropdown.Dtos
{
	/// <summary>
	/// 获取下拉组件的传入参数Dto
	/// </summary>
    public class GetDropdownListsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

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
