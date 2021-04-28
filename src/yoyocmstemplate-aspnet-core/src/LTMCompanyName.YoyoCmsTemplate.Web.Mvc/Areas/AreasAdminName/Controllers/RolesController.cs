using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Roles;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_Roles)]
    public class RolesController : YoyoCmsTemplateControllerBase
    {
        private readonly IRoleAppService _roleAppService;
        private readonly IPermissionAppService _permissionAppService;

        public RolesController(
            IRoleAppService roleAppService,
            IPermissionAppService permissionAppService)
        {
            _roleAppService = roleAppService;
            _permissionAppService = permissionAppService;
        }

        public ActionResult Index()
        {
            var permissions = _permissionAppService.GetAllPermissions()
                                                .Items
                                                .Select(p => new ComboboxItemDto(p.Name, new string('-', p.Level * 2) + " " + p.DisplayName))
                                                .ToList();

            permissions.Insert(0, new ComboboxItemDto("", ""));
            var model = new RoleListViewModel
            {
                Permissions = permissions
            };

            return View(model);
        }

        [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_Roles_Create, YoyoSoftPermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            var output = await _roleAppService.GetRoleForEdit(new NullableIdDto { Id = id });
            var viewModel = new CreateOrEditRoleModalViewModel(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}