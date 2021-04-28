using LTMCompanyName.YoyoCmsTemplate.Controllers;
using Microsoft.AspNetCore.Antiforgery;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Controllers
{
    public class AntiForgeryController : YoyoCmsTemplateControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
