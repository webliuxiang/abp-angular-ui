

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.DomainService
{
    /// <summary>
    /// 友情链接分类领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class BlogrollTypeManager : YoyoCmsTemplateDomainServiceBase, IBlogrollTypeManager
    {

        private readonly IRepository<BlogrollType, int> _blogrollTypeRepository;

        /// <summary>
        /// BlogrollType的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public BlogrollTypeManager(IRepository<BlogrollType, int> blogrollTypeRepository)
        {
            _blogrollTypeRepository = blogrollTypeRepository;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<BlogrollType> QueryBlogrollTypes()
        {
            return _blogrollTypeRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<BlogrollType> QueryBlogrollTypesAsNoTracking()
        {
            return _blogrollTypeRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BlogrollType> FindByIdAsync(int id)
        {
            var entity = await _blogrollTypeRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(int id)
        {
            var result = await _blogrollTypeRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion



        public async Task<BlogrollType> CreateAsync(BlogrollType entity)
        {
            entity.Id = await _blogrollTypeRepository.InsertAndGetIdAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(BlogrollType entity)
        {
            await _blogrollTypeRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _blogrollTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _blogrollTypeRepository.DeleteAsync(a => input.Contains(a.Id));
        }


        //// custom codes



        //// custom codes end







    }
}
