using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos
{
    public class GetProjectsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Sort Desc";
            }
        }

    }
}
