using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Threading;
using Ganss.XSS;
using L._52ABP.Application.Dtos;
using L._52ABP.Common.Extensions.Enums;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Exporting;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.Dtos.Portal;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging.DomainService;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts
{
    /// <summary>
    /// 文章应用层服务的接口实现方法
    ///</summary>
   // [AbpAuthorize]
    public class PostAppService : YoyoCmsTemplateAppServiceBase, IPostAppService
    {
        private readonly IRepository<Post, Guid> _postRepository;

        private readonly IPostListExcelExporter _postListExcelExporter;
        private readonly IEnumExtensionsAppService _enumExtensionsAppService;
        private readonly IPostManager _postManager;
        private readonly ITagManager _tagManager;
        private readonly ICommentManager _commentManager;
        private readonly IRepository<PostTag, int> _postTagRepository;
        private readonly IBlogManager _blogManager;

        /// <summary>
        /// 构造函数
        ///</summary>
        public PostAppService(IRepository<Post, Guid> postRepository, IPostManager postManager, PostListExcelExporter postListExcelExporter, IEnumExtensionsAppService enumExtensionsAppService, ITagManager tagManager, ICommentManager commentManager, IRepository<PostTag, int> postTagRepository, IBlogManager blogManager)
        {
            _postRepository = postRepository;
            _postManager = postManager; ;
            _postListExcelExporter = postListExcelExporter;
            _enumExtensionsAppService = enumExtensionsAppService;
            _tagManager = tagManager;
            _commentManager = commentManager;
            _postTagRepository = postTagRepository;
            _blogManager = blogManager;
        }


        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <returns> </returns>
        [AbpAuthorize(PostPermissions.Query)]
        public async Task<List<PostListDto>> GetPosts()
        {

            var models = await _postRepository.GetAll().AsNoTracking().ToListAsync();

            var dtos = ObjectMapper.Map<List<PostListDto>>(models);
            return dtos;
        }

        /// <summary>
        /// 获取文章的分页列表信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(PostPermissions.Query)]
        public async Task<PagedResultDto<PostListDto>> GetPaged(GetPostsInput input)
        {
            var query = _postRepository.GetAll()

                          //模糊搜索地址
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Url.Contains(input.FilterText))
                          //模糊搜索封面
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.CoverImage.Contains(input.FilterText))
                          //模糊搜索标题
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Title.Contains(input.FilterText))
                          //模糊搜索内容
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Content.Contains(input.FilterText))
            ;
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var postList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var postListDtos = ObjectMapper.Map<List<PostListDto>>(postList);

            foreach (var item in postListDtos)
            {
                var tags = await GetTagsOfPost(item.Id);

                item.Tags = string.Join(", ", tags.Select(p => p.Name).ToArray());

                item.BlogName = AsyncHelper.RunSync(() => _blogManager.FindByIdAsync(item.BlogId)).Name;
            }

            return new PagedResultDto<PostListDto>(count, postListDtos);
        }

        /// <summary>
        /// 通过指定id获取PostListDto信息
        /// </summary>
        [AbpAuthorize(PostPermissions.Query)]
        public async Task<PostListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _postRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<PostListDto>(entity);

            var tags = await GetTagsOfPost(dto.Id);

            dto.Tags = string.Join(", ", tags.Select(p => p.Name).ToArray());
            return dto;
        }

        /// <summary>
        /// 获取编辑 文章
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(PostPermissions.Create, PostPermissions.Edit)]
        public async Task<GetPostForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetPostForEditOutput();
            PostEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _postRepository.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<PostEditDto>(entity);
                var tagsList = await GetTagsOfPost(entity.Id);
                if (tagsList.Count > 0)
                {
                    editDto.TagIds = tagsList.Select(t => t.Id).ToList();
                }
            }
            else
            {
                editDto = new PostEditDto();
            }

            output.PostTypeTypeEnum = _enumExtensionsAppService.GetEntityDoubleStringKeyValueList<PostType>();

            output.Post = editDto;
            return output;
        }

        /// <summary>
        /// 添加或者修改文章的公共方法
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(PostPermissions.Create, PostPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdatePostInput input)
        {
            if (input.Post.Id.HasValue)
            {
                await Update(input.Post);
            }
            else
            {
                await Create(input.Post);
            }
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        [AbpAuthorize(PostPermissions.Create)]
        protected virtual async Task<PostEditDto> Create(PostEditDto input)
        {
            input.Url = await RenameUrlIfItAlreadyExistAsync(input.Url);
            var entity = ObjectMapper.Map<Post>(input);
            //调用领域服务
            entity = await _postManager.CreateAsync(entity);

            //保存Tag标签内容

            if (input.TagIds != null && input.TagIds.Count > 0)
            {
                await _tagManager.SaveTags(input.TagIds, entity);
            }

            var dto = ObjectMapper.Map<PostEditDto>(entity);
            return dto;
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        [AbpAuthorize(PostPermissions.Edit)]
        protected virtual async Task<Post> Update(PostEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await UpdateByMakrdown(input);
            return entity;
        }

        private async Task<Post> UpdateByMakrdown(PostEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _postRepository.GetAsync(input.Id.Value);

            input.Url = await RenameUrlIfItAlreadyExistAsync(input.Url, entity);

            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);

            await _postManager.UpdateAsync(entity);
            if (input.TagIds?.Count > 0)
            {
                await _tagManager.SaveTags(input.TagIds, entity);
            }
            return entity;
        }

        /// <summary>
        /// 删除文章信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(PostPermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            var postId = input.Id;
            var post = await _postRepository.GetAsync(postId);
            var tags = await GetTagsOfPost(postId);
            await _tagManager.DecreaseUsageCountOfTagsAsync(tags.Select(t => t.Id).ToList());
            await _commentManager.DeleteCommentsOfPost(postId);
            await _postManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Post的方法
        /// </summary>
        [AbpAuthorize(PostPermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            foreach (var id in input)
            {
                await Delete(new EntityDto<Guid>(id));
            }
        }

        /// <summary>
        /// 导出文章为excel文件
        /// </summary>
        /// <returns> </returns>
        [AbpAuthorize(PostPermissions.ExportExcel)]
        public async Task<FileDto> GetToExcelFile()
        {
            var posts = await _postManager.QueryPosts().ToListAsync();
            var postListDtos = ObjectMapper.Map<List<PostListDto>>(posts);
            return _postListExcelExporter.ExportToExcelFile(postListDtos);
        }

        //// custom codes

        /// <summary>
        /// 根据博客id或者标签查询文章列表
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task<PagedResultDto<PostDetailsDto>> GetListByBlogIdAndTagName(GetPortalBlogsInput input)
        {
            var query = _postManager.QueryPostsAsNoTracking()
                          //模糊搜索地址
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Url.Contains(input.FilterText))
                          //模糊搜索封面
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.CoverImage.Contains(input.FilterText))
                          //模糊搜索标题
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Title.Contains(input.FilterText))
                          //模糊搜索内容
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Content.Contains(input.FilterText));
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var posts = await query.Where(a => a.BlogId == input.BlogId)
                .OrderByDescending(d => d.CreationTime)
                    .PageBy(input)
                    .ToListAsync();

            var tag = input.TagName.IsNullOrWhiteSpace() ? null : await _tagManager.FindByNameAndBlogIdAsync(input.BlogId, input.TagName);



             

            var postDtos = ObjectMapper.Map<List<PostDetailsDto>>(posts);



            var userDictionary = new Dictionary<long, UserListDto>();

            foreach (var postDto in postDtos)
            {
                postDto.Tags = await GetTagsOfPost(postDto.Id);
                //获取文章中的评论总数
                postDto.CommentCount = await _commentManager.GetCommentCountOfPostAsync(postDto.Id);
                // 通过文章的创建者userid可以获取关于用户的更多信息

                if (postDto.CreatorUserId.HasValue)
                {
                    if (!userDictionary.ContainsKey(postDto.CreatorUserId.Value))
                    {
                        var tes = AbpSession.TenantId;
                        // AbpSession
                        var creatorUser = await UserManager.Users
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(a => a.Id == postDto.CreatorUserId.Value);
                        if (creatorUser != null)
                        {
                            userDictionary[creatorUser.Id] = ObjectMapper.Map<UserListDto>(creatorUser);
                        }
                    }

                    if (userDictionary.ContainsKey(postDto.CreatorUserId.Value))
                    {
                        postDto.Writer = userDictionary[(long)postDto.CreatorUserId];
                    }
                }
            }

            if (tag != null)
            {
                postDtos = await FilterPostsByTag(postDtos, tag);
            }
            return new PagedResultDto<PostDetailsDto>(count, postDtos);

         }

        /// <summary>
        /// 获取阅读文章的地址
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task<PostDetailsDto> GetForReadingAsync(GetReadPostInput input)
        {
            var post = await _postManager.GetPostByUrl(input.BlogId, input.Url);

            if (post == null)
            {
                return null;
            }

            post.IncreaseReadCount();

            var postDto = ObjectMapper.Map<PostDetailsDto>(post);
            var sanitizer = new HtmlSanitizer
            {
                KeepChildNodes = true,     
        };
            sanitizer.AllowedAttributes.Add("class");


            postDto.Content = sanitizer.Sanitize(postDto.Content);

            postDto.Tags = await GetTagsOfPost(postDto.Id);

            var creatorUser = await UserManager.Users
                       .IgnoreQueryFilters()
                       .FirstOrDefaultAsync(a => a.Id == postDto.CreatorUserId.Value);
            postDto.Writer = ObjectMapper.Map<UserListDto>(creatorUser);

            return postDto;
        }

        #region 私有方法

        /// <summary>
        /// 获取当前文章下的标签列表
        /// </summary>
        /// <param name="id"> 文章Id </param>
        /// <returns> </returns>
        private async Task<List<TagListDto>> GetTagsOfPost(Guid id)
        {
            var postTags = await _postTagRepository.GetAllListAsync(a => a.PostId == id);

            if (postTags.Count > 0)
            {
                var tagIds = postTags.Select(s => s.TagId).ToList();
                var tags = await _tagManager.QueryTagsAsNoTracking().Where(t => tagIds.Contains(t.Id)).ToListAsync();
                return ObjectMapper.Map<List<TagListDto>>(tags);
            }
            else
            {
                 
                return new List<TagListDto>();
            }
        }

        /// <summary>
        /// 查询出包含该标签的文章列表
        /// </summary>
        /// <param name="allPostDtos"> </param>
        /// <param name="tag"> </param>
        /// <returns> </returns>
        private Task<List<PostDetailsDto>> FilterPostsByTag(List<PostDetailsDto> allPostDtos, Tag tag)
        {
            var filteredPostDtos = allPostDtos.Where(p => p.Tags?.Any(t => t.Id == tag.Id) ?? false).ToList();

            return Task.FromResult(filteredPostDtos);
        }

        /// <summary>
        /// 如果文章的阅读url已经存在，则对它进行重命名
        /// </summary>
        /// <param name="url"> 文章阅读url </param>
        /// <param name="existingPost"> 已存在的文章信息 </param>
        /// <returns> </returns>
        private async Task<string> RenameUrlIfItAlreadyExistAsync(string url, Post existingPost = null)
        {
            var postList = await _postManager.QueryPostsAsNoTracking().ToListAsync();
            //检查url是否存在，如果存在，且不是当前这篇文档的id，则对本篇文章添加5位数的guid值
            if (postList.Where(p => p.Url == url).WhereIf(existingPost != null, p => existingPost.Id != p.Id).Any())
            {
                return url + "-" + Guid.NewGuid().ToString().Substring(0, 5);
            }

            return url;
        }

        #endregion 私有方法

        /// <summary>
        /// 根据博客id获取阅读量最多的10条文章
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public async Task<List<PostDetailsDto>> GetMostViewsListByBlogId(Guid? blogId)
        {
            var posts = await _postManager.QueryPostsAsNoTracking().WhereIf(blogId!=null,a => a.BlogId == blogId).OrderByDescending(a => a.ReadCount).Take(10).ToListAsync();

            var dtos = ObjectMapper.Map<List<PostDetailsDto>>(posts);
            return dtos;
        }

        public async Task CreatePostByMakrdown(CreatePostDto input)
        {
            //检查下当前文章是否存在
            //如果存在则替换下内容。将以前的文章存放在历史内容中
           
            var oldpost = await _postManager.GetPostByUrl(input.BlogId, input.Url);

            if (oldpost != null)
            {
                var dto = ObjectMapper.Map<PostEditDto>(oldpost);                

                if (input.Content!=dto.Content)
                {
                    dto.HistoryContent = dto.Content;
                    dto.Title = input.Title;
                    dto.PostType = input.PostType;
                    dto.CoverImage = input.CoverImage;
                    dto.Content = input.Content;

                    oldpost = await UpdateByMakrdown(dto).ConfigureAwait(false);

                    //保存Tag标签内容
                    if (input.NewTags != null && input.NewTags.Count > 0)
                    {
                        await _tagManager.SaveTagsByName(input.NewTags, oldpost);
                    }
                    await CurrentUnitOfWork.SaveChangesAsync().ConfigureAwait(false);
                }               
            }
            else
            {
                input.Url = await RenameUrlIfItAlreadyExistAsync(input.Url).ConfigureAwait(false);
                var entity = ObjectMapper.Map<Post>(input);

                if (AbpSession.UserId==null)
                {
                    entity.CreatorUserId = 2;
                }
                

                 //调用领域服务
                entity = await _postManager.CreateAsync(entity).ConfigureAwait(false);

                //保存Tag标签内容
                if (input.NewTags != null && input.NewTags.Count > 0)
                {
                    await _tagManager.SaveTagsByName(input.NewTags, entity);
                }
                await CurrentUnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            }

        }
        /// <summary>
        /// 获取为文章下的标签列表
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task<List<TagListDto>> GetTagsOfPostId(EntityDto<Guid> input)
        {
            var post = await _postRepository.GetAllIncluding(x => x.Tags).FirstOrDefaultAsync(x => x.Id == input.Id);
            if (post != null)
            {
                var tags = post.Tags.ToList();
                return ObjectMapper.Map<List<TagListDto>>(tags);
            }
            else
            {
                return new List<TagListDto>();
            }
        }

        public IQueryable<PostDetailsViewDto> GetPostDetatil()
        {
            var query = from entity in _postRepository.GetAll().AsNoTracking()
                join blog in _blogManager.QueryBlogsAsNoTracking()
                    .Include(x => x.BlogUser) on entity.BlogId equals blog.Id into blogJoind
                from blog in blogJoind.DefaultIfEmpty()
                select new PostDetailsViewDto()
                {
                    PostId = entity.Id,
                    Url = entity.Url,
                    CoverImage = entity.CoverImage,
                    Title = entity.Title,
                    Content = entity.Content,
                    ReadCount = entity.ReadCount,
                    ProfilePictureId = blog.BlogUser.ProfilePictureId,
                    Name = blog.Name,
                    ShortName = blog.ShortName,
                    CreateTime = entity.CreationTime,
                    BlogUserId = blog.BlogUser.Id
                };

            return query;
        }
    }
}
