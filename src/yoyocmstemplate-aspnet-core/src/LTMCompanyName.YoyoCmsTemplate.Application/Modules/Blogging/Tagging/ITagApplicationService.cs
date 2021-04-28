
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;



namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging
{
    /// <summary>
    /// 标签应用层服务的接口方法
    ///</summary>
    public interface ITagAppService : IApplicationService
    {
        /// <summary>
		/// 获取标签的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TagListDto>> GetPaged(GetTagsInput input);


        /// <summary>
        /// 通过指定id获取标签ListDto信息
        /// </summary>
        Task<TagListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体标签的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetTagForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改标签的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateTagInput input);



        /// <summary>
        /// 获取受欢迎的标签列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<TagListDto>> GetPopularTags(GetPopularTagsInput input);


        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除标签
        /// </summary>
        Task BatchDelete(List<Guid> input);




        /// <summary>
        /// 导出标签为excel文件
        /// </summary>
        /// <returns></returns>
        Task<FileDto> GetToExcelFile();

        /// <summary>
        /// 文章标签
        /// </summary>
        /// <returns></returns>
        IQueryable<TagListDto> GetPostTag(Guid postId);


        /// <summary>
        /// 博客标签
        /// </summary>
        /// <returns></returns>
        IQueryable<TagListDto> GetBlogTag(Guid blogId);
    }
}
