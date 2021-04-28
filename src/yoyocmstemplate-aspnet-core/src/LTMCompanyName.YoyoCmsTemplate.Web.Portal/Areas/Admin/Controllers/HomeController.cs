using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Areas.Admin.Controllers
{

    [Area(AppConsts.PortalAdminName)]
    [AbpMvcAuthorize]
    public class HomeController : YoyoCmsTemplateControllerBase
    {

        public HomeController()
        {
        }

        public async Task<IActionResult> Index()
        {
            await Task.Yield();

            if (AbpSession.UserId.HasValue)
            {

              //  return RedirectToAction("Index", "Home", new { area = "Admin" });

            }
            else
            {
                RedirectToAction("Login", "Account");
            }


            




            return View();
        }
    }
}
