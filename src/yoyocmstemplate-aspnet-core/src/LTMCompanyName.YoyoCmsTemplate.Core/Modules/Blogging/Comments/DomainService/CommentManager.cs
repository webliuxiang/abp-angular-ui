using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments.DomainService
{
    /// <summary>
    /// 评论领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class CommentManager : YoyoCmsTemplateDomainServiceBase, ICommentManager
    {





        private readonly IRepository<Comment, Guid> _commentRepository;

        /// <summary>
        /// Comment的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public CommentManager(IRepository<Comment, Guid> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<Comment> QueryComments()
        {
            return _commentRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<Comment> QueryCommentsAsNoTracking()
        {
            return _commentRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Comment> FindByIdAsync(Guid id)
        {
            var entity = await _commentRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(Guid id)
        {
            var result = await _commentRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion



        public async Task<Comment> CreateAsync(Comment entity)
        {
            entity.Id = await _commentRepository.InsertAndGetIdAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(Comment entity)
        {
            await _commentRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _commentRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _commentRepository.DeleteAsync(a => input.Contains(a.Id));
        }




        //// custom codes

        /// <summary>
        /// 获取文章中的评论总数
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<int> GetCommentCountOfPostAsync(Guid postId)
        {
            return await _commentRepository.CountAsync(a => a.PostId == postId);
        }

        /// <summary>
        /// 删除文章下面的评论内容
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task DeleteCommentsOfPost(Guid postId)
        {
            var commentIds = await _commentRepository.GetAll().Where(a => a.PostId == postId).Select(a => a.Id).ToListAsync();

            await BatchDelete(commentIds);



        }


        //// custom codes end







    }
}
