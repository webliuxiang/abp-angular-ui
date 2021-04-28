using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Editions.Dtos
{
    public class GetEditionsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
                Sorting = "CreationTime Desc";
        }
    }
}
