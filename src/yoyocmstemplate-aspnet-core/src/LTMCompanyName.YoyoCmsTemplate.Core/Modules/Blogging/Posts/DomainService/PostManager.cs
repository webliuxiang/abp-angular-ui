using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.DomainService
{
    /// <summary>
    /// 文章领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class PostManager : YoyoCmsTemplateDomainServiceBase, IPostManager
    {

        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IRepository<PostTag> _postTagRepository;

        /// <summary>
        /// Post的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public PostManager(IRepository<Post, Guid> postRepository, IRepository<PostTag> postTagRepository)
        {
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<Post> QueryPosts()
        {
            return _postRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<Post> QueryPostsAsNoTracking()
        {
            return _postRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Post> FindByIdAsync(Guid id)
        {
            var entity = await _postRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(Guid id)
        {
            var result = await _postRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion



        public async Task<Post> CreateAsync(Post entity)
        {

            entity.Id = await _postRepository.InsertAndGetIdAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(Post entity)
        {
            await _postRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _postRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _postRepository.DeleteAsync(a => input.Contains(a.Id));
        }



         

        public async Task<Post> GetPostByUrl(Guid blogId, string url)
        {
            var post = await _postRepository.FirstOrDefaultAsync(p => p.BlogId == blogId && p.Url == url);

           

            //if (post == null)
            //{
            //    throw new EntityNotFoundException(typeof(Post), nameof(post));
            //}

            return post;
        }

        /// <summary>
        /// 增加阅读量
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task IncreaseReadCount(Guid postId)
        {
            var post = await _postRepository.FirstOrDefaultAsync(postId);

            if (post != null)
            {
                post.IncreaseReadCount();

                _postRepository.Update(post);
            }        
        }

        /// <summary>
        /// 获取Tag标签下的文章列表
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task<List<Post>> GetPostsByTagId(Guid tagId)
        {
            var tagPosts = await _postTagRepository.GetAllListAsync(a => a.TagId == tagId);


            if (tagPosts.Count <= 0)
            {
                return new List<Post>();

            }

            var postIds = tagPosts.Select(s => s.PostId).ToList();
            return await QueryPostsAsNoTracking().Where(t => postIds.Contains(t.Id)).ToListAsync();



        }

    }
}
