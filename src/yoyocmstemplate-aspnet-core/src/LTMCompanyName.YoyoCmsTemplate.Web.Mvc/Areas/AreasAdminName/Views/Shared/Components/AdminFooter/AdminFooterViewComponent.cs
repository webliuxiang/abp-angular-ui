using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Session;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Views;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Views.Shared.Components.AdminFooter
{
    public class AdminFooterViewComponent : YoyoCmsTemplateViewComponent
    {

        private readonly IWebSessionCache _webSessionCache;


        public AdminFooterViewComponent(IWebSessionCache webSessionCache)
        {
            _webSessionCache = webSessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var footerModel = new AdminFooterViewModel
            {
                LoginInformations = await _webSessionCache.GetCurrentLoginInformationsAsync(),

            };

            return View(footerModel);
        }
    }
}
