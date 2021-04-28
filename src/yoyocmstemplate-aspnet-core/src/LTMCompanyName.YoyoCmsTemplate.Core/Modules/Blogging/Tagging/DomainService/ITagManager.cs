using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging.DomainService
{
    public interface ITagManager : IDomainService
    {


        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        IQueryable<Tag> QueryTags();

        /// <summary>
        /// 返回性能更好的IQueryable类型，但不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>

        IQueryable<Tag> QueryTagsAsNoTracking();

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Tag> FindByIdAsync(Guid id);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> IsExistAsync(Guid id);


        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="entity">标签实体</param>
        /// <returns></returns>
        Task<Tag> CreateAsync(Tag entity);

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="entity">标签实体</param>
        /// <returns></returns>
        Task<Tag> UpdateAsync(Tag entity);

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
        /// 根据博客id和标签名称查询tag是否存在
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        Task<Tag> FindByNameAndBlogIdAsync(Guid blogId, string tagName);

        /// <summary>
        /// 根据博客id和标签名称查询tag是否存在
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        Task<Tag> FindByNameAndPostIdAsync(Guid postId, string tagName);

        /// <summary>
        /// 标签ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DecreaseUsageCountOfTagsAsync(List<Guid> ids);



        /// <summary>
        /// 保存Tag
        /// </summary>
        /// <param name="newTags"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        Task SaveTags(List<Guid> newTags, Post post);


        /// <summary>
        /// 保存Tag
        /// </summary>
        /// <param name="newTags"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        Task SaveTags(List<Guid> newTags, Blog post);

        /// <summary>
        /// 保存Tag
        /// </summary>
        /// <param name="newTags"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        Task SaveTagsByName(List<string> newTags, Post post);



        /// <summary>
        /// 获取文章下面的Tag标签数量
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<List<Tag>> GetTagsOfPost(Guid postId);

       


        //// custom codes end





    }
}
