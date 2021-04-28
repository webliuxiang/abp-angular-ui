using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.DomainService
{
    public interface IWebSiteNoticeManager : IDomainService
    {


        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        IQueryable<WebSiteNotice> QueryWebSiteNotices();

        /// <summary>
        /// 返回性能更好的IQueryable类型，但不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>

        IQueryable<WebSiteNotice> QueryWebSiteNoticesAsNoTracking();

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebSiteNotice> FindByIdAsync(long id);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> IsExistAsync(long id);


        /// <summary>
        /// 添加网站公告
        /// </summary>
        /// <param name="entity">网站公告实体</param>
        /// <returns></returns>
        Task<WebSiteNotice> CreateAsync(WebSiteNotice entity);

        /// <summary>
        /// 修改网站公告
        /// </summary>
        /// <param name="entity">网站公告实体</param>
        /// <returns></returns>
        Task UpdateAsync(WebSiteNotice entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(long id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input">Id的集合</param>
        /// <returns></returns>
        Task BatchDelete(List<long> input);



        //// custom codes



        //// custom codes end





    }
}
