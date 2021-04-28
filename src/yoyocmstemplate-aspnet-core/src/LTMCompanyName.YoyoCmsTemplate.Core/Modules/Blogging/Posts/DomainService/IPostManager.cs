using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.DomainService
{
    public interface IPostManager : IDomainService
    {


        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        IQueryable<Post> QueryPosts();

        /// <summary>
        /// 返回性能更好的IQueryable类型，但不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>

        IQueryable<Post> QueryPostsAsNoTracking();

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Post> FindByIdAsync(Guid id);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> IsExistAsync(Guid id);


        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="entity">文章实体</param>
        /// <returns></returns>
        Task<Post> CreateAsync(Post entity);

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="entity">文章实体</param>
        /// <returns></returns>
        Task UpdateAsync(Post entity);

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
       




        //// custom codes

        /// <summary>
        /// 根据博客id和url查询文章详情
        /// </summary>
        /// <param name="blogId">博客Id</param>
        /// <param name="url">文章url</param>
        /// <returns></returns>
        Task<Post> GetPostByUrl(Guid blogId, string url);

        /// <summary>
        /// 获取Tag标签下的文章列表
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Task<List<Post>> GetPostsByTagId(Guid tagId);

        //// custom codes end

        /// <summary>
        /// 增加阅读量
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task IncreaseReadCount(Guid postId);


    }
}
