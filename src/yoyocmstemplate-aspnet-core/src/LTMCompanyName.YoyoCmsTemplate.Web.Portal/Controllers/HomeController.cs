using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.UI;
using L._52ABP.Application.GitlabAPIs;
using L._52ABP.Application.GitlabAPIs.Dtos;
using L._52ABP.Core.Configs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Markdown;
using LTMCompanyName.YoyoCmsTemplate.Url;
using LTMCompanyName.YoyoCmsTemplate.Web.Portal.Models.Home;
using Masuit.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Controllers
{
    public class HomeController : YoyoCmsTemplateControllerBase
    {
        private readonly IBlogAppService _blogAppService;
        private readonly IPostAppService _postAppService;
        private readonly ITagAppService _tagAppService;
        private readonly IBlogManager _blogManager;
        private readonly IGitlabClientAppService _gitlabClientAppService;
        private readonly IMarkdownConverter _markdownConverter;

        public HomeController(IBlogAppService blogAppService, IPostAppService postAppService, ITagAppService tagAppService, IBlogManager blogManager, IGitlabClientAppService gitlabClientAppService, IMarkdownConverter markdownConverter)
        {
            _blogAppService = blogAppService;
            _postAppService = postAppService;
            _tagAppService = tagAppService;
            _blogManager = blogManager;
            _gitlabClientAppService = gitlabClientAppService;
            _markdownConverter = markdownConverter;
        }

        public async Task<ActionResult> Index(string blogShortName, string tagName = null)
        {



            #region 文章列表

            var blog = await _blogAppService.GetByShortNameAsync(blogShortName);
            if (blog == null)
            {
                var listBlog = await _blogManager.QueryBlogsAsNoTracking().ToListAsync();

                if (listBlog.Count > 0)
                {
                    blog = ObjectMapper.Map<BlogListDto>(listBlog[0]);
                }
            }

            var viewModel = new PostsAndPopularTags
            {
                BlogShortName = blog.ShortName
            };

            //var posts = (await _postAppService.GetListByBlogIdAndTagName(blog.Id, tagName)).Items;
            //var popularTags = await _tagAppService.GetPopularTags(new GetPopularTagsInput { BlogId = blog.Id, ResultCount = 10, MinimumPostCount = 2 });

            //viewModel.PostDetailsDtos = posts;
            //viewModel.TagList = popularTags;
            //ViewBag.postsList = viewModel;

            #endregion 文章列表

            return View();
        }

        [AbpMvcAuthorize]
        public async Task<ActionResult> Contact()
        {
            await Task.Yield();

            return View();
        }

        public ActionResult Soon()
        {
            return View();
        }

        /// <summary>
        /// 上传图片到图床中
        /// </summary>
        /// <param name="content"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> UploadpicturesToPictureBedAsync(string content, GitlabPostsNavInput input, PostItems postConfig)
        {
            if (content == null)
            {
                return null;
            }

            //获取需要被图床的图片列表
            var toDoImgBedList = Regex.Matches(content, @"(<img\s+[^>]*)src=""([^""]*)""([^>]*>)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            var newSourceImagesList = new List<string>();

            foreach (Match itemPic in toDoImgBedList)
            {
                //检查是否为外联，如果是外联按不处理
                if (WebUrlHelper.IsExternalLink(itemPic.Value))
                {
                    //检查图片是否为外部链接
                    return itemPic.Value;
                }

                input.FileName = itemPic.Groups[2].Value;

                var file = await _gitlabClientAppService.GetGitlabFileInfo(input);
                if (file == null)
                {
                    continue;
                }
                //获取当前图片的base64，然后上传到图床中。

                var tags = postConfig.tags.Split(',');

                var directoryPath = $"{postConfig.blogShortName}/{tags[0]}";

                //存放的图床地址
                var pictureToBed = new UploadPictureToBed
                {
                    PathWithNamespace = "52abp/picturebed",
                    file = file,
                    DirectoryPath = directoryPath
                };




                //上传图片到指定图床
                var imgRawUrl = await _gitlabClientAppService.UploadPictureToImgBed(pictureToBed);
                newSourceImagesList.Add(imgRawUrl);
                //install-ubuntu-1.png

                //http://code.52abp.com/52abp/picturebed/raw/master/2020/04/28/install-ubuntu-1.png

            }



            content = Regex.Replace(content, @"(<img\s+[^>]*)src=""([^""]*)""([^>]*>)", (Match match) =>
            {


                var oldImageSource = match.Groups[2].Value;

                oldImageSource = Path.GetFileName(oldImageSource);
                if (WebUrlHelper.IsExternalLink(oldImageSource))
                {
                    //检查图片是否为外部链接
                    return match.Value;
                }

                var newImageSource = newSourceImagesList.Where(a => a.EndsWith(oldImageSource))
                                    .FirstOrDefault();

                var url = match.Groups[1] + " src=\"" + newImageSource + "\" " + match.Groups[3];
                return url;
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            return content;
        }






    }
}
