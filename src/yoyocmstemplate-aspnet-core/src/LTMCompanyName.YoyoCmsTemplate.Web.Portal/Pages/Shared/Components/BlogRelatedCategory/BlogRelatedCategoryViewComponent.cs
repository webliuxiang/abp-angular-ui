using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Shared.Components.BlogRelatedCategory.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Shared.Components.BlogRelatedCategory
{
    public class BlogRelatedCategoryViewComponent : YoyoCmsTemplateViewComponent
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;

        public BlogRelatedCategoryViewComponent(IPostAppService postAppService, IBlogAppService blogAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(GetBlogRelatedInput input)
        {

            var blog = await _blogAppService.GetByShortNameAsync(input.BlogShortName);

            var postsViews = await _postAppService.GetMostViewsListByBlogId(blog.Id);

            var blogRelatedCategory = new BlogRelatedCategoryDto();
            blogRelatedCategory.BlogShortName = blog.ShortName;

            blogRelatedCategory.postsDto = postsViews;



            return View(blogRelatedCategory);
        }





    }
}
