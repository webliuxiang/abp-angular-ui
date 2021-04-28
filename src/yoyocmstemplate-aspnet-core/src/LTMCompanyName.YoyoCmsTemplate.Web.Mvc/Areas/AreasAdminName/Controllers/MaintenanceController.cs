using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Cachings;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Maintenance;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_Host_Maintenance)]
    public class MaintenanceController : YoyoCmsTemplateControllerBase
    {
        private readonly IHostCachingAppService _cachingAppService;

        public MaintenanceController(IHostCachingAppService cachingAppService)
        {
            _cachingAppService = cachingAppService;
        }

        public IActionResult Index()
        {
            MaintenanceViewModel model = new MaintenanceViewModel
            {
                Caches = _cachingAppService.GetAllCaches().Items
            };

            return View(model);
        }
    }
}