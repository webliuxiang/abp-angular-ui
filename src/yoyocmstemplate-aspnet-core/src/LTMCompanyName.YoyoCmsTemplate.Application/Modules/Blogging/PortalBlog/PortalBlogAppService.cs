using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using L._52ABP.Application.GitlabAPIs;
using L._52ABP.Application.GitlabAPIs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.Dtos.Portal;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Markdown;
using LTMCompanyName.YoyoCmsTemplate.Url;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.PortalBlog
{
    public class PortalBlogAppService : YoyoCmsTemplateAppServiceBase, IPortalBlogAppService
    {
        public const int MaxShortContentLength = 128;

        private readonly IMarkdownConverter _markdownConverter;

        private readonly IGitlabClientAppService _gitlabClientAppService;
        private readonly IPostAppService _postAppService;
        private readonly IAppConfigurationAccessor _appConfiguration;
        private readonly IPostManager _postManager;
        private readonly ITagManager _tagManager;
        private readonly BlogManager _blogManager;
        private readonly IRepository<PostTag> _postTagRepository;

        public PortalBlogAppService(IGitlabClientAppService gitlabClientAppService, IMarkdownConverter markdownConverter, IPostAppService postAppService, IAppConfigurationAccessor appConfiguration, IPostManager postManager, ITagManager tagManager, BlogManager blogManager, IRepository<PostTag> postTagRepository)
        {
            _gitlabClientAppService = gitlabClientAppService;

            _markdownConverter = markdownConverter;
            _postAppService = postAppService;
            _appConfiguration = appConfiguration;
            _postManager = postManager;
            _tagManager = tagManager;
            _blogManager = blogManager;
            _postTagRepository = postTagRepository;
        }

        public string GetShortContentByMarkDown(string content)
        {
            var html = RenderMarkdownToString(content);

            var plainText = Regex.Replace(html, "<[^>]*>", "");

            if (string.IsNullOrWhiteSpace(plainText))
            {
                return "";
            }

            var firsParag = plainText.Split(Environment.NewLine).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

            if (firsParag == null)
            {
                return plainText;
            }

            if (firsParag.Length <= MaxShortContentLength)
            {
                return firsParag;
            }

            return firsParag.Substring(0, MaxShortContentLength) + "...";
        }

        public string GetShortContentByHtml(string content)
        {
            if (content.IsNullOrWhiteSpace())
            {
                return "";
            }

            var bytes = Encoding.Default.GetBytes(content);
            var utf8Content = Encoding.UTF8.GetString(bytes);
            var plainText = Regex.Replace(utf8Content, @"<(.|\n)*?>", "");

            if (string.IsNullOrWhiteSpace(plainText))
            {
                return "";
            }

            var firsParag = plainText.Split(Environment.NewLine).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

            if (firsParag == null)
            {
                return plainText;
            }

            if (firsParag.Length <= MaxShortContentLength)
            {
                return firsParag;
            }

            return firsParag.Substring(0, MaxShortContentLength) + "...";
        }

        public IHtmlContent RenderMarkdownToHtml(string content)
        {
            if (content.IsNullOrWhiteSpace())
            {
                return new HtmlString("");
            }

            var bytes = Encoding.Default.GetBytes(content);
            var utf8Content = Encoding.UTF8.GetString(bytes);

            var html = _markdownConverter.ConvertToHtml(utf8Content);

            return new HtmlString(html);
        }

        public string RenderMarkdownToString(string content)
        {
            if (content.IsNullOrWhiteSpace())
            {
                return "";
            }

            var bytes = Encoding.Default.GetBytes(content);
            var utf8Content = Encoding.UTF8.GetString(bytes);

            return _markdownConverter.ConvertToHtml(utf8Content);
        }

        public string ConvertDatetimeToTimeAgo(DateTime dt)
        {
            var timeDiff = DateTime.Now - dt;

            var diffInDays = (int)timeDiff.TotalDays;

            if (diffInDays >= 365)
            {
                return L("YearsAgo", diffInDays / 365);
            }
            if (diffInDays >= 30)
            {
                return L("MonthsAgo", diffInDays / 30);
            }
            if (diffInDays >= 7)
            {
                return L("WeeksAgo", diffInDays / 7);
            }
            if (diffInDays >= 1)
            {
                return L("DaysAgo", diffInDays);
            }

            var diffInSeconds = (int)timeDiff.TotalSeconds;

            if (diffInSeconds >= 3600)
            {
                return L("HoursAgo", diffInSeconds / 3600);
            }
            if (diffInSeconds >= 60)
            {
                return L("MinutesAgo", diffInSeconds / 60);
            }
            if (diffInSeconds >= 1)
            {
                return L("SecondsAgo", diffInSeconds);
            }

            return L("Now");
        }

        public async Task AutoMaticallyPublishMarkdownPostsAsync(GitlabPostsNavInput input)
        {
            // var userId=  AbpSession.UserId;
            var sercertCode = _appConfiguration.GetMarkdownPostSercerCode();

            if (sercertCode.IsNullOrEmpty())
            {
                throw new UserFriendlyException("key丢失，请配置后，再提交。");
            }
            if (input.SercertCode != sercertCode)
            {
                throw new UserFriendlyException("密钥不正确，请重新提交新密钥。");
            }
            var fileContent = await _gitlabClientAppService.GetGitlabFileInfo(input);

            if (fileContent == null)
            {
                 throw new UserFriendlyException($"{input.FilePath}路径下的{input.FileName}文件在Gitlab中不存在，请重试。");
            }
            var dto = JsonConvert.DeserializeObject<RepoPostsDto>(fileContent.ContentDecoded);

            if (dto.items != null)
            {
                var items = dto.items.Where(a => a.Enabled).ToList();

                if (items.Count > 0)
                {
                    //判断第一篇文章发布的博客名称
                    var blogshortName = items[0].blogShortName;

                    var blog = await _blogManager.GetByShortNameAsync(blogshortName);

                    foreach (var item in items)
                    {
                        //如果后续的文章名称和博客名称不一致则发布到新博客文章中去
                        if (item.blogShortName != blogshortName)
                        {
                            blog = await _blogManager.GetByShortNameAsync(blogshortName);
                        }

                        if (item.title.IsNullOrWhiteSpace())
                        {
                            continue;
                        }

                        input.FileName = item.path;
                        //获取文章内容
                        var itemContent = await _gitlabClientAppService.GetGitlabFileInfo(input);

                        if (itemContent == null)
                        {
                            continue;
                        }

                        var name = itemContent.Filename;

                        if (item.URL.IsNullOrEmpty())
                        {
                            item.URL = Path.GetFileNameWithoutExtension(name);
                        }

                        //转换为html内容
                        var htmlContent = _markdownConverter.ConvertToHtml(itemContent.ContentDecoded);
                        //上传图片到图床
                        htmlContent = await UploadpicturesToPictureBedAsync(htmlContent, input, item);

                        //todo find a way to make it on client in prismJS configuration (eg: map C# => csharp)
                        htmlContent = HtmlNormalizer.ReplaceCodeBlocksLanguage(
                            htmlContent,
                            "language-C#",
                            "language-csharp"
                        );
                        htmlContent = HtmlNormalizer.ReplaceCodeLinkUrl(htmlContent);
                        var tags = item.tags.Split(',');

                        var post = new CreatePostDto
                        {
                            BlogId = blog.Id,
                            Title = item.title,
                            Content = htmlContent,
                            Url = item.URL,
                            NewTags = tags.ToList(),
                            CoverImage = item.ConverImage,
                            PostType = PostType.Original
                        };

                        await _postAppService.CreatePostByMakrdown(post);
                    }
                }
            }
        }

        /// <summary>
        /// 上传图片到图床地址中
        /// </summary>
        /// <param name="content"></param>
        /// <param name="input"></param>
        /// <param name="postConfig"></param>
        /// <returns></returns>
        private async Task<string> UploadpicturesToPictureBedAsync(string content, GitlabPostsNavInput input, PostItems postConfig)
        {
            if (content == null)
            {
                return null;
            }

            //获取上传图片列表
            var toDoImgBedList = Regex.Matches(content, @"(<img\s+[^>]*)src=""([^""]*)""([^>]*>)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
            var newSourceImagesList = new List<string>();
            foreach (Match itemPic in toDoImgBedList)
            {
                //检查是否为外联，如果是外联按不处理
                if (WebUrlHelper.IsExternalLink(itemPic.Groups[2].Value))
                {
                    newSourceImagesList.Add(itemPic.Groups[2].Value);
                    //检查图片是否为外部链接
                }
                else
                {
                    input.FileName = itemPic.Groups[2].Value;
                    var file = await _gitlabClientAppService.GetGitlabFileInfo(input);
                    if (file == null)
                    {
                        continue;
                    }
                    //获取当前图片的base64，然后上传到图床中。
                    var tags = postConfig.tags.Split(',');
                    var directoryPath = $"{postConfig.blogShortName}/{tags[0]}";
                    //存放的图床仓库和地址
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
            }

            //将本地图源，替换为远程图床的图源
            content = Regex.Replace(content, @"(<img\s+[^>]*)src=""([^""]*)""([^>]*>)", (Match match) =>
            {
                var oldImageSource = match.Groups[2].Value;

                if (WebUrlHelper.IsExternalLink(oldImageSource))
                {
                    //检查图片是否为外部链接

                    var newurl = match.Groups[1] + " class=\"img-fluid\" src =\"" + oldImageSource + "\" " + match.Groups[3];

                    return newurl;
                    // return match.Value;
                }
                oldImageSource = Path.GetFileName(oldImageSource);

                var newImageSource = newSourceImagesList
                    .FirstOrDefault(a => a.EndsWith(oldImageSource));

                var url = match.Groups[1] + " class=\"img-fluid\" src =\"" + newImageSource + "\" " + match.Groups[3];

                return url;
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            return content;
        }

        /// <summary>
        /// 获取博客列表下的文章内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PostDetailsDto>> GetPostsPagedForHome(GetPortalBlogsInput input)
        {
            var query = _postManager.QueryPostsAsNoTracking()
                          //模糊搜索地址
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Url.Contains(input.FilterText))
                          //模糊搜索标题
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Title.Contains(input.FilterText))
                          //模糊搜索内容
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Content.Contains(input.FilterText));

            // TODO:根据传入的参数添加过滤条件

            var tag = input.TagName.IsNullOrWhiteSpace()
                ? null
                : await _tagManager.QueryTagsAsNoTracking().FirstOrDefaultAsync(a => a.Name == input.TagName);

            if (tag!=null)
            {
               query = query.Where(a => a.Tags.Any(t => t.TagId == tag.Id));

            }





            var count = await query.CountAsync();

            var posts = await query
                .OrderByDescending(d => d.CreationTime)
                    .PageBy(input)
                    .ToListAsync();



            var postDtos = ObjectMapper.Map<List<PostDetailsDto>>(posts);

            var userDictionary = new Dictionary<long, UserListDto>();
            var blogDictionary = new Dictionary<Guid, string>();
            foreach (var postDto in postDtos)
            {
                // 通过文章的创建者userid可以获取关于用户的更多信息


                if (postDto.CreatorUserId.HasValue)
                {
                    if (!userDictionary.ContainsKey(postDto.CreatorUserId.Value))
                    {
                        var tes = AbpSession.TenantId;
                        // AbpSession
                        var creatorUser = await UserManager.Users
                            .IgnoreQueryFilters()
                            .FirstOrDefaultAsync(a => a.Id == postDto.CreatorUserId.Value);
                        if (creatorUser != null)
                        {
                            userDictionary[creatorUser.Id] = ObjectMapper.Map<UserListDto>(creatorUser);
                        }
                    }

                    if (userDictionary.ContainsKey(postDto.CreatorUserId.Value))
                    {
                        postDto.Writer = userDictionary[(long)postDto.CreatorUserId];
                    }


 
                }


                // 获取文章的博客短名称

                if (!blogDictionary.ContainsKey(postDto.BlogId))
                {
                    var blogDto = await _blogManager.FindByIdAsync(postDto.BlogId);

                    blogDictionary[postDto.BlogId] = blogDto.ShortName;

                }


                if (blogDictionary.ContainsKey(postDto.BlogId))
                {
                    postDto.BlogShortName = blogDictionary[postDto.BlogId];
                }

                //获取标签
               var tags = await _tagManager.GetTagsOfPost(postDto.Id);

               postDto.Tags = ObjectMapper.Map<List<TagListDto>>(tags);

            }

            return new PagedResultDto<PostDetailsDto>(count, postDtos);
        }

        public async Task<PagedResultDto<PostDetailsDto>> GetPostsPagedForByTagName(GetPortalBlogsInput input)
        {

            var tag = input.TagName.IsNullOrWhiteSpace()
                ? null
                : await _tagManager.QueryTagsAsNoTracking().FirstOrDefaultAsync(a => a.Name == input.TagName);



 

            //if (tag != null)
            //{

            //    var count =   await _tagManager.QueryTagsAsNoTracking().CountAsync();

            //    //var tagPosts =   _postTagRepository.GetAll().Where(a=>a.TagId==tag.Id).Select(new string());

            //    //var posts =   query
            //    //    .OrderByDescending(d => d.CreationTime)
            //    //    .PageBy(input)
            //    //    .ToListAsync();

            //    var tagPostsquery = _postTagRepository.GetAll().AsNoTracking();
            //    var postsquery = _postManager.QueryPostsAsNoTracking();


            //  var da=  postsquery.Where(a => a.Tags.Any(t => t.TagId == tag.Id)).ToList();



            //    //var postDtos = ObjectMapper.Map<List<PostDetailsDto>>(posts);


            //    //return   posts = await _postManager.GetPostsByTagId(tag.Id);
            //}
              
                return new PagedResultDto<PostDetailsDto>();
            




        }


        /// <summary>
        /// 获取最多阅读的前10篇文章-service
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostDetailsDto>> GetMostViewsPostsList()
        {
      var posts = await _postManager.QueryPostsAsNoTracking().OrderByDescending(a => a.ReadCount).Take(10).ToListAsync();

            var dtos = ObjectMapper.Map<List<PostDetailsDto>>(posts);


            return dtos;
        }
    }
}
