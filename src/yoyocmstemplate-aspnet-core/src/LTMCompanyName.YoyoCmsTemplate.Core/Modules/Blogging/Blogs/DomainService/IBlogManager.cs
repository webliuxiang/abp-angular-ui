using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.DomainService
{
    public interface IBlogManager : IDomainService
    {


        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        IQueryable<Blog> QueryBlogs();

        /// <summary>
        /// 返回性能更好的IQueryable类型，但不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>

        IQueryable<Blog> QueryBlogsAsNoTracking();

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Blog> FindByIdAsync(Guid id);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> IsExistAsync(Guid id);


        /// <summary>
        /// 添加博客
        /// </summary>
        /// <param name="entity">博客实体</param>
        /// <returns></returns>
        Task<Blog> CreateAsync(Blog entity);

        /// <summary>
        /// 修改博客
        /// </summary>
        /// <param name="entity">博客实体</param>
        /// <returns></returns>
        Task UpdateAsync(Blog entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input">Id的集合</param>
        /// <returns></returns>
        Task BatchDelete(List<Guid> input);


        /// <summary>
        /// 根据短名称获取博客信息
        /// </summary>
        /// <param name="shortName"> </param>
        /// <returns> </returns>
        Task<Blog> GetByShortNameAsync(string shortName);

        //// custom codes



        //// custom codes end





    }
}
