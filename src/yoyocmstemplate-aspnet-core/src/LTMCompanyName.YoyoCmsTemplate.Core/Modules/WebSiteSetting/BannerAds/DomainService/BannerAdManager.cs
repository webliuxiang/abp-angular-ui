

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds.DomainService
{
    /// <summary>
    /// 轮播图广告领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class BannerAdManager : YoyoCmsTemplateDomainServiceBase, IBannerAdManager
    {

        private readonly IRepository<BannerAd, int> _bannerAdRepository;

        /// <summary>
        /// BannerAd的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public BannerAdManager(IRepository<BannerAd, int> bannerAdRepository)
        {
            _bannerAdRepository = bannerAdRepository;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<BannerAd> QueryBannerAds()
        {
            return _bannerAdRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<BannerAd> QueryBannerAdsAsNoTracking()
        {
            return _bannerAdRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BannerAd> FindByIdAsync(int id)
        {
            var entity = await _bannerAdRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(int id)
        {
            var result = await _bannerAdRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion



        public async Task<BannerAd> CreateAsync(BannerAd entity)
        {
            entity.Id = await _bannerAdRepository.InsertAndGetIdAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(BannerAd entity)
        {
            await _bannerAdRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _bannerAdRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _bannerAdRepository.DeleteAsync(a => input.Contains(a.Id));
        }


        //// custom codes



        //// custom codes end







    }
}
