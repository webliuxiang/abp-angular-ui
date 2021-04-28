using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; private set; }

        public UserPermissionsEditViewModel(GetUserPermissionsForEditOutput output, User user)
        {
            User = user;
            output.MapTo(this);
        }
    }
}