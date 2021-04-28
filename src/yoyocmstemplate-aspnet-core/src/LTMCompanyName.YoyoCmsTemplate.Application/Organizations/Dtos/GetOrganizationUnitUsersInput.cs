using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{
    public class GetOrganizationUnitUsersInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        [Range(1, long.MaxValue)] public long Id { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
                Sorting = " user.Id ";
            else if (Sorting.Contains("userName"))
                Sorting = Sorting.Replace("userName", "user.userName");
            else if (Sorting.Contains("addedTime")) Sorting = Sorting.Replace("addedTime", "uou.creationTime");
        }
    }
}