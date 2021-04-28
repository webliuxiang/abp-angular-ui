using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using L._52ABP.Application.GitlabAPIs.Dtos;
using L._52ABP.Core.Configs;
using LTMCompanyName.YoyoCmsTemplate.Blogging;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Controllers
{


    /// <summary>
    /// 博客及文章详情的导航内容
    /// </summary>
    public class BlogController : YoyoCmsTemplateControllerBase
    {

        private readonly IBlogAppService _blogAppService;
        private readonly IBlogManager _blogManager;
        private readonly IPostAppService _postAppService;

        private readonly IPortalBlogAppService _portalBlogAppService;

        private readonly IAppConfigurationAccessor _appConfigurationAccessor;


        public BlogController(IBlogAppService blogAppService, IPostAppService postAppService, IBlogManager blogManager, IPortalBlogAppService portalBlogAppService, IAppConfigurationAccessor appConfigurationAccessor)
        {
            _blogAppService = blogAppService;
            _postAppService = postAppService;
            _blogManager = blogManager;
            _portalBlogAppService = portalBlogAppService;
            _appConfigurationAccessor = appConfigurationAccessor;
        }



        [Route("{blogshortName}")]
        public async Task<ActionResult> BlogList(string blogshortName)
        {

            var dto = await _blogAppService.GetByShortNameAsync(blogshortName);
            if (dto == null)
            {
                return RedirectToAction("E404", "Error");
            }
          //  var postDtos = await _postAppService.GetListByBlogIdAndTagName(dto.Id, null);


            return View();


        }


        [Route("{blogshortName}/{url}")]

        public async Task<ActionResult> Details(string blogshortName, string url)
        {



            var blog = await _blogAppService.GetByShortNameAsync(blogshortName);

            if (blog == null)
            {
                return RedirectToAction("E404", "Error");
            }


            var postDetails = await _postAppService.GetForReadingAsync(new GetReadPostInput
            {
                BlogId = blog.Id,
                Url = url
            });

            if (postDetails == null)
            {
                return RedirectToAction("E404", "Error");
            }


            ViewBag.BlogShortName = blogshortName;


            return View(postDetails);

        }

        [AbpMvcAuthorize]
        public async Task<string> BlogActiveAsync(string secretCode)
        {

            AbpAppConfig.PostCategoryConfigs = new List<PostCategoryConfig>();
            _appConfigurationAccessor.Configuration.Bind("MarkdownPosts:Categories", AbpAppConfig.PostCategoryConfigs);

            foreach (var config in AbpAppConfig.PostCategoryConfigs.Where(a => a.Enabled))
            {
                var input = new GitlabPostsNavInput
                {
                    PathWithNamespace = config.ReposName,
                    FileName = config.FileName,
                    FilePath = config.Filepath
                };
                input.SercertCode = secretCode;
                await _portalBlogAppService.AutoMaticallyPublishMarkdownPostsAsync(input).ConfigureAwait(false);

            }

            return "abcc";

        }


    }
}
