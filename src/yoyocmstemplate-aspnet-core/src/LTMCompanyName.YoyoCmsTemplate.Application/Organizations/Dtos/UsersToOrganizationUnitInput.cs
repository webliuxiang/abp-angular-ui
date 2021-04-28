using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{
    public class UsersToOrganizationUnitInput
    {
        public List<long> UserIds { get; set; }

        [Range(1, long.MaxValue)] public long OrganizationUnitId { get; set; }
    }
}