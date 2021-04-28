using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.Dtos;



namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls
{
    /// <summary>
    /// 友情链接分类应用层服务的接口方法
    ///</summary>
    public interface IBlogrollTypeAppService : IApplicationService
    {
        /// <summary>
		/// 获取友情链接分类的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<BlogrollTypeListDto>> GetPaged(GetBlogrollTypesInput input);


        /// <summary>
        /// 通过指定id获取友情链接分类ListDto信息
        /// </summary>
        Task<BlogrollTypeListDto> GetById(EntityDto<int> input);


        /// <summary>
        /// 返回实体友情链接分类的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetBlogrollTypeForEditOutput> GetForEdit(NullableIdDto<int> input);


        /// <summary>
        /// 添加或者修改友情链接分类的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateBlogrollTypeInput input);


        /// <summary>
        /// 删除友情链接分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<int> input);


        /// <summary>
        /// 批量删除友情链接分类
        /// </summary>
        Task BatchDelete(List<int> input);



        //// custom codes



        //// custom codes end
    }
}
