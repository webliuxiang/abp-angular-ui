using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Models.Home;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Pages.Public
{
    public class IndexBase:AbpComponentBase
    {

      [Inject]  private   IBlogAppService _blogAppService { get; set; }
        [Inject] private   IPostAppService _postAppService { get; set; }
        [Inject] private   ITagAppService _tagAppService { get; set; }
        [Inject] private   IBlogManager _blogManager { get; set; }

        [Parameter]
        public string blogShortName { get; set; }
        [Parameter]
        public string tagName { get; set; }

        public PostsAndPopularTags PostsAndTags{ get; set; }

        protected override async Task OnInitializedAsync()
        {
            #region 文章列表
            var blog = await _blogAppService.GetByShortNameAsync(blogShortName).ConfigureAwait(false);
            if (blog == null)
            {
                var listBlog = await _blogManager.QueryBlogsAsNoTracking().ToListAsync();

                if (listBlog.Count > 0)
                {
                    blog = ObjectMapper.Map<BlogListDto>(listBlog[0]);
                }
            }

            PostsAndTags = new PostsAndPopularTags
            {
                BlogShortName = blog.ShortName
            };

            //var posts = (await _postAppService.GetListByBlogIdAndTagName(blog.Id, tagName).ConfigureAwait(false)).Items;

            var popularTags = await _tagAppService.GetPopularTags(new GetPopularTagsInput { BlogId = blog.Id, ResultCount = 10, MinimumPostCount = 2 }).ConfigureAwait(false);


           // PostsAndTags.posts = posts;
            PostsAndTags.tags = popularTags;
         
            #endregion



        }


    }
}
