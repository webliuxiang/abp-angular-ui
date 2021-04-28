using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Blog
{
    public class DetailsModel : YoyoCmsTemplatePageModel
    {
        private readonly IBlogAppService _blogAppService;
        private readonly IPostAppService _postAppService;

        public DetailsModel(IBlogAppService blogAppService, IPostAppService postAppService)
        {
            _blogAppService = blogAppService;
            _postAppService = postAppService;
        }

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Posturl { get; set; }

        public PostDetailsDto PostDetailsDto { get; set; } = new PostDetailsDto();

        public async Task<IActionResult> OnGet()
        {
            var blog = await _blogAppService.GetByShortNameAsync(BlogShortName);

            if (blog == null)
            {
                return RedirectToAction("E404", "Error");
            }

            PostDetailsDto = await _postAppService.GetForReadingAsync(new GetReadPostInput
            {
                BlogId = blog.Id,
                Url = Posturl
            });

            if (PostDetailsDto == null)
            {
                return RedirectToAction("E404", "Error");
            }

            return Page();
        }
    }
}
