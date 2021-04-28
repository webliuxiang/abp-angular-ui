using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize(YoyoSoftPermissionNames.Pages_Tenant_Dashboard)]
    public class DashboardController : YoyoCmsTemplateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}