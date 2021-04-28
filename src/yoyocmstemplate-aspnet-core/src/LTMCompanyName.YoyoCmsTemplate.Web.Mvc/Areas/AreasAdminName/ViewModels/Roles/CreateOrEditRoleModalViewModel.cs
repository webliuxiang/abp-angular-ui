using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode
        {
            get { return Role.Id.HasValue; }
        }

        public CreateOrEditRoleModalViewModel(GetRoleForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}