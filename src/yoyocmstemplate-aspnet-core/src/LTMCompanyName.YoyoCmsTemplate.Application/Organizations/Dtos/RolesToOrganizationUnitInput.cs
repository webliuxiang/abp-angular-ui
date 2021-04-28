using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{
    public class RolesToOrganizationUnitInput
    {
        public List<int> RoleIds { get; set; }

        [Range(1, long.MaxValue)] public long OrganizationUnitId { get; set; }
    }
}
