using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace L._52ABP.Web.FrontView.Pages.Components.PriorityList
{
	public class PriorityListViewComponent : ViewComponent
	{

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Yield();
            //  var home=new HomePricesViewModel();


            return View();
        }

	}
}
