using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Common.Dtos
{
    public class CommonLookupFindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}