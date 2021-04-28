
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using L._52ABP.Application.Dtos;
using L._52ABP.Application.GitlabAPIs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.Dtos.Portal;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Dtos;

// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts
{
    /// <summary>
    /// 文章应用层服务的接口方法
    ///</summary>
    public interface IPostAppService : IApplicationService
    {
        /// <summary>
		/// 获取文章的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PostListDto>> GetPaged(GetPostsInput input);


        /// <summary>
        /// 通过指定id获取文章ListDto信息
        /// </summary>
        Task<PostListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体文章的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetPostForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改文章的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdatePostInput input);


        Task CreatePostByMakrdown(CreatePostDto input);


        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除文章
        /// </summary>
        Task BatchDelete(List<Guid> input);




        /// <summary>
        /// 导出文章为excel文件
        /// </summary>
        /// <returns></returns>
        Task<FileDto> GetToExcelFile();

        //// custom codes

        /// <summary>
        /// 根据博客id或者标签查询文章列表
        /// </summary>      
        /// <returns></returns>
        Task<PagedResultDto<PostDetailsDto>> GetListByBlogIdAndTagName(GetPortalBlogsInput input);

        /// <summary>
        /// 获取最多阅读的文章列表
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        Task<List<PostDetailsDto>> GetMostViewsListByBlogId(Guid? blogId);

 
        /// <summary>
        /// 获取阅读文章的地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task<PostDetailsDto> GetForReadingAsync(GetReadPostInput input);




        /// <summary>
        /// 获取当前文章下的标签列表
        /// </summary>
        /// <returns> </returns>
        Task<List<TagListDto>> GetTagsOfPostId(EntityDto<Guid> input);



        IQueryable<PostDetailsViewDto> GetPostDetatil();

        //// custom codes end
    }
}
