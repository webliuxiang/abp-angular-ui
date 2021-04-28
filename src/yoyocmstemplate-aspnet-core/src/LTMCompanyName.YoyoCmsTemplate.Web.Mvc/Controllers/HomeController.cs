using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Controllers
{

    public class HomeController : YoyoCmsTemplateControllerBase
    {
        private readonly SignInManager _signInManager;

        public HomeController(SignInManager signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string redirect = "", bool forceNewRegistration = false)
        {
            if (forceNewRegistration)
            {
                await _signInManager.SignOutAsync();
            }

            if (redirect == "TenantRegistration")
            {
                return RedirectToAction("SelectEdition", "TenantRegistration");
            }

            return AbpSession.UserId.HasValue ?
                RedirectToAction("Index", "Home", new { area = "AreasAdminName" }) :
                RedirectToAction("Login", "Account");
        }
    }
}
