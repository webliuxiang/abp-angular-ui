

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.DomainService
{
    /// <summary>
    /// 网站公告领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class WebSiteNoticeManager : YoyoCmsTemplateDomainServiceBase, IWebSiteNoticeManager
    {

        private readonly IRepository<WebSiteNotice, long> _webSiteNoticeRepository;

        /// <summary>
        /// WebSiteNotice的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public WebSiteNoticeManager(IRepository<WebSiteNotice, long> webSiteNoticeRepository)
        {
            _webSiteNoticeRepository = webSiteNoticeRepository;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<WebSiteNotice> QueryWebSiteNotices()
        {
            return _webSiteNoticeRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<WebSiteNotice> QueryWebSiteNoticesAsNoTracking()
        {
            return _webSiteNoticeRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebSiteNotice> FindByIdAsync(long id)
        {
            var entity = await _webSiteNoticeRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(long id)
        {
            var result = await _webSiteNoticeRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion



        public async Task<WebSiteNotice> CreateAsync(WebSiteNotice entity)
        {
            entity.Id = await _webSiteNoticeRepository.InsertAndGetIdAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(WebSiteNotice entity)
        {
            await _webSiteNoticeRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(long id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _webSiteNoticeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<long> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _webSiteNoticeRepository.DeleteAsync(a => input.Contains(a.Id));
        }


        //// custom codes



        //// custom codes end







    }
}
