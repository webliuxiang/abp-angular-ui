using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Shared.Components.ProductPricesFaq
{
    public class ProductPricesFaqViewComponent : YoyoCmsTemplateViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Yield();
            //  var home=new HomePricesViewModel();


            return View();
        }
    }
}
