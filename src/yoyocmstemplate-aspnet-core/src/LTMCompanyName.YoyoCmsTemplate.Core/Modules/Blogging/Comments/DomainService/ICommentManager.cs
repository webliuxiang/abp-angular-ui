using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments.DomainService
{
    public interface ICommentManager : IDomainService
    {


        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        IQueryable<Comment> QueryComments();

        /// <summary>
        /// 返回性能更好的IQueryable类型，但不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>

        IQueryable<Comment> QueryCommentsAsNoTracking();

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Comment> FindByIdAsync(Guid id);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> IsExistAsync(Guid id);


        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="entity">评论实体</param>
        /// <returns></returns>
        Task<Comment> CreateAsync(Comment entity);

        /// <summary>
        /// 修改评论
        /// </summary>
        /// <param name="entity">评论实体</param>
        /// <returns></returns>
        Task UpdateAsync(Comment entity);

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
        /// 获取当前文章中的评论总数
        /// </summary>
        /// <param name="postId">文章id</param>
        /// <returns></returns>
        Task<int> GetCommentCountOfPostAsync(Guid postId);

        Task DeleteCommentsOfPost(Guid postId);



        //// custom codes



        //// custom codes end





    }
}
