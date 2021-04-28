using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.Dtos;



namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls
{
    /// <summary>
    /// 友情链接应用层服务的接口方法
    ///</summary>
    public interface IBlogrollAppService : IApplicationService
    {
        /// <summary>
		/// 获取友情链接的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<BlogrollListDto>> GetPaged(GetBlogrollsInput input);


        /// <summary>
        /// 通过指定id获取友情链接ListDto信息
        /// </summary>
        Task<BlogrollListDto> GetById(EntityDto<int> input);


        /// <summary>
        /// 返回实体友情链接的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetBlogrollForEditOutput> GetForEdit(NullableIdDto<int> input);


        /// <summary>
        /// 添加或者修改友情链接的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateBlogrollInput input);


        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<int> input);


        /// <summary>
        /// 批量删除友情链接
        /// </summary>
        Task BatchDelete(List<int> input);



        //// custom codes



        //// custom codes end
    }
}
