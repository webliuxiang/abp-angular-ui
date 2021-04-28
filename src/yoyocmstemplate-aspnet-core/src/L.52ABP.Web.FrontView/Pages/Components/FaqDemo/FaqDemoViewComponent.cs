using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE1006 // 命名样式
namespace L._52ABP.Web.FrontView.Pages.Components.FaqDemo
#pragma warning restore IDE1006 // 命名样式
{
	public class FaqDemo : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
            await Task.Yield();
			//  var home=new HomePricesViewModel();


			return View();
		}
	}
}
