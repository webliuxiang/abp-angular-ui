using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Areas.Views.Shared.Components.AdminBreadcrumb
{
    public class AdminBreadcrumbViewComponent : YoyoCmsTemplateViewComponent
    {




        public AdminBreadcrumbViewComponent()
        {

        }


        public async Task<IViewComponentResult> InvokeAsync(string currentPageName)
        {
            await Task.Yield();
            var breadcrumbModel = new AdminBreadcrumbViewModel
            {
                ControllerName = HttpContext.GetRouteValue("controller").ToString(),
                AreaName = HttpContext.GetRouteValue("area").ToString(),
                ActionName = HttpContext.GetRouteValue("action").ToString(),
                CurrentPageName = currentPageName

            };



            //     HttpContext.Request.RequestContext.RouteData.Values

            //var dd=    ControllerContext.ActionDescriptor.ActionName;
            // var a33=   ControllerContext.ActionDescriptor.ControllerName;


            return View(breadcrumbModel);

        }




    }
}
