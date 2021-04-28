using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using L._52ABP.Application.GitlabAPIs.Dtos;
using L._52ABP.Core.Configs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.Dtos.Portal;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.PortalBlog.Dtos;
using Microsoft.AspNetCore.Html;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging
{


    /// <summary>
    /// 服务于门户端控制器中Blog模块的服务方法
    /// </summary>
    public interface IPortalBlogAppService
    {

        string GetShortContentByMarkDown(string content);
        string GetShortContentByHtml(string content);

        /// <summary>
        /// 转换markdown为html格式
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        IHtmlContent RenderMarkdownToHtml(string content);

        string RenderMarkdownToString(string content);


        string ConvertDatetimeToTimeAgo(DateTime dt);

        /// <summary>
        /// 自动化发布
        /// </summary>
        Task AutoMaticallyPublishMarkdownPostsAsync(GitlabPostsNavInput input);




        /// <summary>
        /// 获取门户博客下的文章首页内容
        /// </summary>    
        /// <returns></returns>
        Task<PagedResultDto<PostDetailsDto>> GetPostsPagedForHome(GetPortalBlogsInput input);

        /// <summary>
        /// 获取门户博客下通过tagname名称的文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PostDetailsDto>> GetPostsPagedForByTagName(GetPortalBlogsInput input);

        /// <summary>
        /// 获取最多阅读的前10篇文章
        /// </summary>
        /// <returns></returns>
        Task<List<PostDetailsDto>> GetMostViewsPostsList();


    }
}
