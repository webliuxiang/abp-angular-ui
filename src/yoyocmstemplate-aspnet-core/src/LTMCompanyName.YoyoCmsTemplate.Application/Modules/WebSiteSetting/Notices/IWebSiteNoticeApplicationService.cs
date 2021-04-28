using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.Dtos;



namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices
{
    /// <summary>
    /// 网站公告应用层服务的接口方法
    ///</summary>
    public interface IWebSiteNoticeAppService : IApplicationService
    {
        /// <summary>
		/// 获取网站公告的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<WebSiteNoticeListDto>> GetPaged(GetWebSiteNoticesInput input);


        /// <summary>
        /// 通过指定id获取网站公告ListDto信息
        /// </summary>
        Task<WebSiteNoticeListDto> GetById(EntityDto<long> input);


        /// <summary>
        /// 返回实体网站公告的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetWebSiteNoticeForEditOutput> GetForEdit(NullableIdDto<long> input);


        /// <summary>
        /// 添加或者修改网站公告的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateWebSiteNoticeInput input);


        /// <summary>
        /// 删除网站公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<long> input);


        /// <summary>
        /// 批量删除网站公告
        /// </summary>
        Task BatchDelete(List<long> input);



        //// custom codes



        //// custom codes end
    }
}
