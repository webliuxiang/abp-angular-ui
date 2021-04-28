using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Web.Portal.Models.Home;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Components
{
    public class HomePostsListViewComponent : YoyoCmsTemplateViewComponent
    {


        public async Task<IViewComponentResult> InvokeAsync(PostsAndPopularTags postsAndPopularTags)
        {
            await Task.Yield();


            return View(postsAndPopularTags);
        }





    }
}
