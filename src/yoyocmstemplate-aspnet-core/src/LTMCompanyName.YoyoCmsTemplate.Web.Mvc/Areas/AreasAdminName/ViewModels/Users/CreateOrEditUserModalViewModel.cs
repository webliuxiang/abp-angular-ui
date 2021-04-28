using System.Linq;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Security.PasswordComplexity;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Users
{
    [AutoMapFrom(typeof(GetUserForEditOutput))]
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput, IOrganizationUnitsEditViewModel
    {
        public bool CanChangeUserName
        {
            get { return User.UserName != AbpUserBase.AdminUserName; }
        }

        public int AssignedRoleCount
        {
            get { return Roles.Count(r => r.IsAssigned); }
        }

        public bool IsEditMode
        {
            get { return User.Id.HasValue; }
        }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public CreateOrEditUserModalViewModel(GetUserForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}