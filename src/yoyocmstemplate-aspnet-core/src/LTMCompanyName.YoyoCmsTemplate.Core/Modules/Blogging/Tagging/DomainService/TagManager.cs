using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using Microsoft.EntityFrameworkCore;
using static Abp.SequentialGuidGenerator;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging.DomainService
{
    /// <summary>
    /// 标签领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class TagManager : YoyoCmsTemplateDomainServiceBase, ITagManager
    {
        private readonly IRepository<Tag, Guid> _tagRepository;
        private readonly IRepository<PostTag> _postTagRepository;
        private readonly IRepository<BlogTag> _blogTagRepository;



        /// <summary>
        /// Tag的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public TagManager(IRepository<Tag, Guid> tagRepository, IRepository<PostTag> postTagRepository, IRepository<BlogTag> blogTagRepository)
        {
            _tagRepository = tagRepository;
            _postTagRepository = postTagRepository;
            _blogTagRepository = blogTagRepository;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<Tag> QueryTags()
        {
            return _tagRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<Tag> QueryTagsAsNoTracking()
        {
            return _tagRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Tag> FindByIdAsync(Guid id)
        {
            var entity = await _tagRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(Guid id)
        {
            var result = await _tagRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion 查询判断的业务

        public async Task<Tag> CreateAsync(Tag entity)
        {
        var oldentity=  await  _tagRepository.FirstOrDefaultAsync(a => a.Name == entity.Name);

        if (oldentity != null)
        {
            entity = oldentity;
        }
        else
        {
            entity.Id = Instance.Create(SequentialGuidDatabaseType.SqlServer);
            entity.Id = await _tagRepository.InsertAndGetIdAsync(entity);
        }


        return entity;
        }

        public async Task<Tag> UpdateAsync(Tag entity)
        {
            return await _tagRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _tagRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _tagRepository.DeleteAsync(a => input.Contains(a.Id));
        }

        //// custom codes

        public async Task<Tag> FindByNameAndBlogIdAsync(Guid blogId, string tagName)
        {
            return await _tagRepository.FirstOrDefaultAsync(a => a.Name == tagName);
        }

        public async Task<Tag> FindByNameAndPostIdAsync(Guid postId, string tagName)
        {
            return await _tagRepository.FirstOrDefaultAsync(a => a.Name == tagName);
        }

        /// <summary>
        /// 减去标签使用的数量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DecreaseUsageCountOfTagsAsync(List<Guid> ids)
        {
            var tags = await _tagRepository.GetAll().Where(a => ids.Any(id => id == a.Id)).ToListAsync();

            foreach (var tag in tags)
            {
                tag.DecreaseUsageCount();
            }
        }

        /// <summary>
        /// 保存Tag标签
        /// </summary>
        /// <param name="newTags"> </param>
        /// <param name="post"> </param>
        /// <returns> </returns>
        public async Task SaveTags(List<Guid> newTags, Post post)
        {
            if (post.Tags == null)
            {
                post.Tags = new List<PostTag>();
            }

            await RemoveOldTags(newTags, post);

            await AddNewTags(newTags, post);
        }
        public async Task SaveTags(List<Guid> newTags, Blog blog)
        {
            var tags = await QueryTags().ToListAsync();

            foreach (var newTag in newTags)
            {
                var tag = tags.FirstOrDefault(t => t.Id == newTag);

                tag.IncreaseUsageCount();
                tag = await UpdateAsync(tag);

                blog.AddTag(tag.Id);
            }
        }

        public async Task SaveTagsByName(List<string> newTags, Post post)
        {
            //if (post.Tags == null)
            //{
            //    post.Tags = new List<PostTag>();
            //}

            await  RemoveOldTagsByName(newTags, post).ConfigureAwait(false);

            await AddNewTagsByName(newTags, post).ConfigureAwait(false);

        }

        public async Task<List<Tag>> GetTagsOfPost(Guid postId)
        {
            var postTags = await _postTagRepository.GetAllListAsync(a => a.PostId == postId);

            if (postTags.Count <= 0)
            {
                return new List<Tag>();
            }

            var tagIds = postTags.Select(s => s.TagId).ToList();
            return  await QueryTagsAsNoTracking().Where(t => tagIds.Contains(t.Id)).ToListAsync();

        }

       
        //// custom codes end

        /// <summary>
        /// 移除旧的标签
        /// </summary>
        /// <param name="newTags"> </param>
        /// <param name="post"> </param>
        /// <returns> </returns>
        private async Task RemoveOldTags(List<Guid> newTags, Post post)
        {
            if (post.Tags != null)
            {
                foreach (var oldTag in post.Tags)
                {
                    var tag = await FindByIdAsync(oldTag.TagId);

                    var oldTagNameInNewTags = newTags.FirstOrDefault(t => t == tag.Id);

                    if (oldTagNameInNewTags == null)
                    {
                        post.RemoveTag(oldTag.TagId);

                        tag.DecreaseUsageCount();
                        await UpdateAsync(tag);
                    }
                    else
                    {
                        newTags.Remove(oldTagNameInNewTags);
                    }
                }
            }
            else
            {
                var postTags = await _postTagRepository.GetAllListAsync(a => a.PostId == post.Id);

                if (postTags != null)
                {
                    foreach (var postTag in postTags)
                    {
                        var tag = await FindByIdAsync(postTag.TagId);
                        tag.DecreaseUsageCount();
                        await UpdateAsync(tag);
                    }

                    //删除掉文章原来的所有标签
                    await _postTagRepository.DeleteAsync(a => a.PostId == post.Id);


                }

                // post.Tags = new ICollection<PostTag>();
            }
        }

        /// <summary>
        /// 添加新标签信息
        /// </summary>
        /// <param name="newTags"> </param>
        /// <param name="post"> </param>
        /// <returns> </returns>
        private async Task AddNewTags(List<Guid> newTags, Post post)
        {
            var tags = await QueryTags().ToListAsync();

            foreach (var newTag in newTags)
            {
                var tag = tags.FirstOrDefault(t => t.Id == newTag);

                tag.IncreaseUsageCount();
                tag = await UpdateAsync(tag);

                post.AddTag(tag.Id);
            }
        }


        private async Task RemoveOldTagsByName(List<string> newTags, Post post)
        {

            if (post.Tags != null && post.Tags.Count > 0)
            {
                foreach (var oldTag in post.Tags)
                {
                    var tag = await FindByIdAsync(oldTag.TagId);

                    var oldTagNameInNewTags = newTags.FirstOrDefault(t => t == tag.Name);

                    if (oldTagNameInNewTags == null)
                    {
                        post.RemoveTag(oldTag.TagId);

                        tag.DecreaseUsageCount();
                        await UpdateAsync(tag);
                    }
                    else
                    {
                        newTags.Remove(oldTagNameInNewTags);
                    }
                }
            }
            else
            {

             var postTags= await  _postTagRepository.GetAllListAsync(a=>a.PostId==post.Id);

                if (postTags.Count>0)
                {
                    foreach (var postTag in postTags)
                    {
                        var tag = await FindByIdAsync(postTag.TagId);
                        tag.DecreaseUsageCount();
                        await UpdateAsync(tag);
                    }

                    //删除掉文章原来的所有标签
                    await _postTagRepository.DeleteAsync(a => a.PostId == post.Id);


                }



                // post.Tags = new ICollection<PostTag>();
            }
        }

        /// <summary>
        /// 根据tag的name创建
        /// </summary>
        /// <param name="newTags"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        private async Task AddNewTagsByName(List<string> newTags, Post post)
        {
            foreach (var newTag in newTags)
            {
                var entity = new Tag(newTag);
                
                entity = await CreateAsync(entity);

                entity.IncreaseUsageCount();

                post.AddTag(entity.Id);
            }
        }
    }
}
