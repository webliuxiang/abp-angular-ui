using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos
{
    public class GetUserForEditTreeOutput
    {
        //public Guid? ProfilePictureId { get; set; }

        public UserEditDto User { get; set; }

        public List<UserRoleDto> Roles { get; set; }

        public List<OrganizationUnitListDto> AllOrganizationUnits { get; set; }

        public List<string> MemberedOrganizationUnits { get; set; }
    }
}