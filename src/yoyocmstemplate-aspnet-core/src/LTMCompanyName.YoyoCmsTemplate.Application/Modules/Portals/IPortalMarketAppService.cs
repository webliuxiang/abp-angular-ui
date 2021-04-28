using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Portals
{

    /// <summary>
    /// 服务于门户的市场模块功能
    /// </summary>
    public interface IPortalMarketAppService : IApplicationService
    {
        /// <summary>
        /// 服务于发布后的产品列表，服务门户
        /// </summary>
        /// <returns></returns>
        Task<List<ProductListDto>> GetPublishedProductforHomePriceAsync();

        
        /// <summary>
        /// 获取发布后的门户课程内容
        /// </summary>
        /// <returns></returns>
        Task<List<CourseListDto>> GetPublishedCourseListforHomeAsync();


        /// <summary>
        /// 获取课程详情包含章节与课时
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        Task<CourseDetailsDto> GetCourseDetailsIncludeSections(long courseId);



    }
}
