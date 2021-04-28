using System;
using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos
{
    public class GetUserForEditOutput
    {
        public Guid? ProfilePictureId { get; set; }

        public UserEditDto User { get; set; }

        public UserRoleDto[] Roles { get; set; }

        public List<OrganizationUnitListDto> AllOrganizationUnits { get; set; }

        public List<string> MemberedOrganizationUnits { get; set; }
    }
}