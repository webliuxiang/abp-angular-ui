using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs
{
    /// <summary>
    /// 博客应用层服务的接口方法
    ///</summary>
    public interface IBlogAppService : IApplicationService
    {

        Task<List<UserListOutput>> GetUserList(string name);

        /// <summary>
		/// 获取博客的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<BlogListDto>> GetPaged(GetBlogsInput input);

        /// <summary>
        /// 通过指定id获取博客ListDto信息
        /// </summary>
        Task<BlogListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 返回实体博客的EditDto
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task<GetBlogForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加或者修改博客的公共方法
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task CreateOrUpdate(CreateOrUpdateBlogInput input);

        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除博客
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 导出博客为excel文件
        /// </summary>
        /// <returns> </returns>
        Task<FileDto> GetToExcelFile();

        /// <summary>
        /// 获取当前博客下的标签列表
        /// </summary>
        /// <returns> </returns>
        Task<List<TagListDto>> GetTagsOfBlog(Guid id);

        //// custom codes

        /// <summary>
        /// 根据短名称获取博客信息
        /// </summary>
        /// <param name="shortName"> </param>
        /// <returns> </returns>
        Task<BlogListDto> GetByShortNameAsync(string shortName);

        Task<List<BlogListDto>> GetBlogs();

        //// custom codes end
    }
}
