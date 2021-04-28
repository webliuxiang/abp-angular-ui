

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.DomainService
{
    /// <summary>
    /// 友情链接领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class BlogrollManager : YoyoCmsTemplateDomainServiceBase, IBlogrollManager
    {

        private readonly IRepository<Blogroll, int> _blogrollRepository;

        /// <summary>
        /// Blogroll的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public BlogrollManager(IRepository<Blogroll, int> blogrollRepository)
        {
            _blogrollRepository = blogrollRepository;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<Blogroll> QueryBlogrolls()
        {
            return _blogrollRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<Blogroll> QueryBlogrollsAsNoTracking()
        {
            return _blogrollRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Blogroll> FindByIdAsync(int id)
        {
            var entity = await _blogrollRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(int id)
        {
            var result = await _blogrollRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion



        public async Task<Blogroll> CreateAsync(Blogroll entity)
        {
            entity.Id = await _blogrollRepository.InsertAndGetIdAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(Blogroll entity)
        {
            await _blogrollRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _blogrollRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _blogrollRepository.DeleteAsync(a => input.Contains(a.Id));
        }


        //// custom codes



        //// custom codes end







    }
}
