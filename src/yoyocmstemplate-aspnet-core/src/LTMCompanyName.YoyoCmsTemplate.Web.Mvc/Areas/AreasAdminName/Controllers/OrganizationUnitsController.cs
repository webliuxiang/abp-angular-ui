using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Organizations;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common.Modals;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.OrganizationUnits;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits)]
    public class OrganizationUnitsController : YoyoCmsTemplateControllerBase
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        public OrganizationUnitsController(IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            _organizationUnitRepository = organizationUnitRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public PartialViewResult CreateModal(long? parentId)
        {
            return PartialView("_CreateModal", new CreateOrganizationUnitModalViewModel(parentId));
        }

        [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<PartialViewResult> EditModal(long id)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(id);
            var model = ObjectMapper.Map<EditOrganizationUnitModalViewModel>(organizationUnit);

            return PartialView("_EditModal", model);
        }

        [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public PartialViewResult AddMemberModal(LookupModalViewModel model)
        {
            return PartialView("_AddMemberModal", model);
        }

        [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageRoles)]
        public PartialViewResult AddRoleModal(LookupModalViewModel model)
        {
            return PartialView("_AddRoleModal", model);
        }
    }
}