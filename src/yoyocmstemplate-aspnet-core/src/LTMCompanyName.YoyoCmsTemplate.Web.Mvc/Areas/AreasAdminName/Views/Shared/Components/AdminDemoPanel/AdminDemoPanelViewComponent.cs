using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Views;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Views.Shared.Components.AdminDemoPanel
{
    public class AdminDemoPanelViewComponent : YoyoCmsTemplateViewComponent
    {


        public AdminDemoPanelViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
