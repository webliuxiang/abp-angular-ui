using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Controllers
{
    [Area(AppConsts.AreasAdminName)]
    [AbpMvcAuthorize]
    public class HomeController : YoyoCmsTemplateControllerBase
    {
        public async Task<ActionResult> Index()
        {
            if (AbpSession.MultiTenancySide == MultiTenancySides.Host)
            {
                if (await IsGrantedAsync(YoyoSoftPermissionNames.Pages_Administration_Host_Dashboard))
                {
                    return RedirectToAction("Index", "HostDashboard");
                }

                if (await IsGrantedAsync(YoyoSoftPermissionNames.Pages_Tenants))
                {
                    return RedirectToAction("Index", "Tenants");
                }
            }
            else
            {
                if (await IsGrantedAsync(YoyoSoftPermissionNames.Pages_Tenant_Dashboard))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            //如果没有对以上页面的权限，则进入欢迎页面

            return RedirectToAction("Index", "Welcome");
        }
    }
}