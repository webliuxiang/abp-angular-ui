using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Sessions;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Editions;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Administration_Tenant_SubscriptionManagement)]
    public class SubscriptionManagementController : YoyoCmsTemplateControllerBase
    {
        private readonly ISessionAppService _sessionAppService;

        public SubscriptionManagementController(ISessionAppService sessionAppService)
        {
            _sessionAppService = sessionAppService;
        }

        public async Task<ActionResult> Index()
        {
            var loginInfo = await _sessionAppService.GetCurrentLoginInformations();
            var model = new SubscriptionDashboardViewModel
            {
                LoginInformations = loginInfo
            };

            return View(model);
        }
    }
}