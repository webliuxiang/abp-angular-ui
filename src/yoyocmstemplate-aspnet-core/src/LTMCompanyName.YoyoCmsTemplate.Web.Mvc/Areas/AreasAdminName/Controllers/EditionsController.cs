using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Editions;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Editions)]
    public class EditionsController : YoyoCmsTemplateControllerBase
    {
        private readonly IEditionAppService _editionAppService;
        private readonly TenantManager _tenantManager;

        public EditionsController(
            IEditionAppService editionAppService,
            TenantManager tenantManager)
        {
            _editionAppService = editionAppService;
            _tenantManager = tenantManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Editions_Create)]
        public async Task<PartialViewResult> CreateModal(int? id)
        {
            var output = await _editionAppService.GetEditionForEdit(new NullableIdDto { Id = id });
            var editionItems = await _editionAppService.GetEditionComboboxItems();
            var freeEditionItems = await _editionAppService.GetEditionComboboxItems(output.Edition.ExpiringEditionId, false, true);

            var viewModel = new CreateEditionModalViewModel(output, editionItems, freeEditionItems);

            return PartialView("_CreateModal", viewModel);
        }

        [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Editions_Create, YoyoSoftPermissionNames.Pages_Editions_Edit)]
        public async Task<PartialViewResult> EditModal(int? id)
        {
            var output = await _editionAppService.GetEditionForEdit(new NullableIdDto { Id = id });
            var editionItems = await _editionAppService.GetEditionComboboxItems();
            var freeEditionItems = await _editionAppService.GetEditionComboboxItems(output.Edition.ExpiringEditionId, false, true);

            var viewModel = new EditEditionModalViewModel(output, editionItems, freeEditionItems);

            return PartialView("_EditModal", viewModel);
        }

        public async Task<PartialViewResult> MoveTenantsToAnotherEdition(int id)
        {
            var editionItems = await _editionAppService.GetEditionComboboxItems();
            var tenantCount = _tenantManager.Tenants.Count(t => t.EditionId == id);

            var viewModel = new MoveTenantsToAnotherEditionViwModel
            {
                EditionId = id,
                TenantCount = tenantCount,
                EditionItems = editionItems
            };

            return PartialView("_MoveTenantsToAnotherEdition", viewModel);
        }
    }
}