using System.Threading.Tasks;
using L._52ABP.Application.GitlabAPIs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Markdown;
using LTMCompanyName.YoyoCmsTemplate.Web.Portal.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages
{
    public class IndexModel : YoyoCmsTemplatePageModel
    {

        private readonly IBlogAppService _blogAppService;
        private readonly IPostAppService _postAppService;
        private readonly ITagAppService _tagAppService;
        private readonly IBlogManager _blogManager;
        private readonly IGitlabClientAppService _gitlabClientAppService;
        private readonly IMarkdownConverter _markdownConverter;

        public IndexModel(IBlogAppService blogAppService, IPostAppService postAppService, ITagAppService tagAppService, IBlogManager blogManager, IGitlabClientAppService gitlabClientAppService, IMarkdownConverter markdownConverter)
        {
            _blogAppService = blogAppService;
            _postAppService = postAppService;
            _tagAppService = tagAppService;
            _blogManager = blogManager;
            _gitlabClientAppService = gitlabClientAppService;
            _markdownConverter = markdownConverter;
        }

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; } = "yoyomooc"; 
        [BindProperty(SupportsGet = true)]
        public string TagName { get; set; } = null;

        public BlogListDto Blog{ get; set; }
        public PostsAndPopularTags PostsAndPopularTags { get; set; } = new PostsAndPopularTags();

        public async Task OnGetAsync()
		{
            HeaderTitle = L("HomePage");

            #region 文章列表
            Blog = await _blogAppService.GetByShortNameAsync(BlogShortName);

            if (Blog == null)
            {
                var listBlog = await _blogManager.QueryBlogsAsNoTracking().ToListAsync();

                if (listBlog.Count > 0)
                {
                    Blog = ObjectMapper.Map<BlogListDto>(listBlog[0]);
                }
            }

            PostsAndPopularTags.BlogShortName = Blog.ShortName;

         //   var posts = (await _postAppService.GetListByBlogIdAndTagName(Blog.Id, TagName)).Items;

            var popularTags = await _tagAppService.GetPopularTags(new GetPopularTagsInput { BlogId = Blog.Id, ResultCount = 10, MinimumPostCount = 2 });

          //  PostsAndPopularTags.PostDetailsDtos = posts;
            PostsAndPopularTags.TagList = popularTags;
 

            #endregion




        }
    }
}
