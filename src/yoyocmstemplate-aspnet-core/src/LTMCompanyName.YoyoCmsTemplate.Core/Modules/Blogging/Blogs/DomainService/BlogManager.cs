using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.DomainService
{
    /// <summary>
    /// 博客领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class BlogManager : YoyoCmsTemplateDomainServiceBase, IBlogManager
    {

        private readonly IRepository<Blog, Guid> _blogRepository;

        /// <summary>
        /// Blog的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public BlogManager(IRepository<Blog, Guid> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<Blog> QueryBlogs()
        {
            return _blogRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<Blog> QueryBlogsAsNoTracking()
        {
            return _blogRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Blog> FindByIdAsync(Guid id)
        {
            var entity = await _blogRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(Guid id)
        {
            var result = await _blogRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion



        public async Task<Blog> CreateAsync(Blog entity)
        {
            await ValidateBlogAsync(entity);

            entity.Id = await _blogRepository.InsertAndGetIdAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(Blog entity)
        {
            await ValidateBlogAsync(entity);
            await _blogRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {


            //TODO:删除前的逻辑判断，是否允许删除
            await _blogRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _blogRepository.DeleteAsync(a => input.Contains(a.Id));
        }

        public async Task<Blog> GetByShortNameAsync(string shortName)
        {
            var blog = await _blogRepository.FirstOrDefaultAsync(b => b.ShortName == shortName);
            return blog;
        }


        //// custom codes

        /// <summary>
        /// 验证文件是否符合
        /// </summary>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        public async Task ValidateBlogAsync(Blog entity)
        {
            await Task.Yield();

            //var siblings = (await FindChildrenAsync(entity.ParentId))
            //     .Where(ou => ou.Id != entity.Id)
            //     .ToList();

            var result = QueryBlogs().Any(a => a.Name == entity.Name && a.Id != entity.Id);

            if (result)
            {
                throw new UserFriendlyException(L("BlogDuplicateNameWarning", entity.Name, entity.ShortName));
            }

            if (QueryBlogs().Any(a => a.ShortName == entity.ShortName && a.Id != entity.Id))
            {
                throw new UserFriendlyException(L("BlogDuplicateNameWarning", entity.ShortName));
            }




            //对文件进行验证//todo：
        }

        //// custom codes end







    }
}
