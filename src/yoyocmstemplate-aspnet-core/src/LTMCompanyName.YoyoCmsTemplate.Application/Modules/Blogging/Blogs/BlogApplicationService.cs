using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using L._52ABP.Application.Dtos;

using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Exporting;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging.DomainService;

using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs
{
    /// <summary>
    /// 博客应用层服务的接口实现方法
    ///</summary>
    public class BlogAppService : YoyoCmsTemplateAppServiceBase, IBlogAppService
    {
        private readonly IRepository<Blog, Guid> _blogRepository;
        private readonly IBlogListExcelExporter _blogListExcelExporter;
        private readonly IBlogManager _blogManager;
        private readonly ITagManager _tagManager;
        private readonly IRepository<BlogTag, int> _blogTagRepository;

        /// <summary>
        /// 构造函数
        ///</summary>
        public BlogAppService(IRepository<Blog, Guid> blogRepository, IBlogManager blogManager, BlogListExcelExporter blogListExcelExporter, ITagManager tagManager, IRepository<BlogTag, int> blogTagRepository)
        {
            _blogRepository = blogRepository;
            _blogManager = blogManager; ;
            _blogListExcelExporter = blogListExcelExporter;
            _tagManager = tagManager;
            _blogTagRepository = blogTagRepository;
        }

        [AbpAuthorize]
        public async Task<List<UserListOutput>> GetUserList(string name)
        {
            var query = await UserManager.Users
             .WhereIf(
                 !name.IsNullOrWhiteSpace(),
                 u =>
                     u.UserName.Contains(name) ||
                     u.EmailAddress.Contains(name)
             )
             .WhereIf(PermissionChecker.IsGranted(BlogPermissions.BlogSelectUser) == false  ,x=>x.Id == AbpSession.UserId)
             .ToListAsync();

            return ObjectMapper.Map<List<UserListOutput>>(query);
        }

        /// <summary>
        /// 获取博客的分页列表信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<BlogListDto>> GetPaged(GetBlogsInput input)
        {
            var query = _blogRepository.GetAllIncluding(x=>x.BlogUser)

                          //模糊搜索博客名称
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Name.Contains(input.FilterText))
                          //模糊搜索博客短名称
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.ShortName.Contains(input.FilterText))
                          //模糊搜索博客描述
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Description.Contains(input.FilterText))
            ;
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var blogList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var blogListDtos = ObjectMapper.Map<List<BlogListDto>>(blogList);


            foreach (var item in blogListDtos)
            {
                var tags = await GetTagsOfBlog(item.Id);

                item.Tags = string.Join(", ", tags.Select(p => p.Name).ToArray());
            }

            return new PagedResultDto<BlogListDto>(count, blogListDtos);
        }

        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns> </returns>
        [AbpAuthorize(BlogPermissions.Query)]
        public async Task<List<BlogListDto>> GetBlogs()
        {


          //  CurrentUnitOfWork.Options.IsTransactional = true;



            var models = await _blogManager.QueryBlogsAsNoTracking().ToListAsync();

            var dtos = ObjectMapper.Map<List<BlogListDto>>(models);
            return dtos;
        }

        /// <summary>
        /// 通过指定id获取BlogListDto信息
        /// </summary>
        [AbpAuthorize(BlogPermissions.Query)]
        public async Task<BlogListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _blogRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<BlogListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 获取编辑 博客
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(BlogPermissions.Create, BlogPermissions.Edit)]
        public async Task<GetBlogForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetBlogForEditOutput();
            BlogEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _blogRepository.GetAllIncluding(x=>x.BlogUser).FirstOrDefaultAsync(x=>x.Id == input.Id.Value);
                editDto = ObjectMapper.Map<BlogEditDto>(entity);

                //var tagsList = await GetTagsOfPost(entity.Id);
                //if (tagsList.Count > 0)
                //{
                //    editDto.TagIds = tagsList.Select(t => t.Id).ToList();
                //}
            }
            else
            {
                editDto = new BlogEditDto();
            }

            output.Blog = editDto;
            return output;
        }

        /// <summary>
        /// 添加或者修改博客的公共方法
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(BlogPermissions.Create, BlogPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateBlogInput input)
        {
            if (input.Blog.Id.HasValue)
            {
                await Update(input.Blog);
            }
            else
            {
                await Create(input.Blog);
            }
        }

        /// <summary>
        /// 新增博客
        /// </summary>
        [AbpAuthorize(BlogPermissions.Create)]
        protected virtual async Task<BlogEditDto> Create(BlogEditDto input)
        {

            var user = await UserManager.GetUserByIdAsync(input.BlogUserId);

            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Blog>(input);

            entity.BlogUser = user;
            // AsyncHelper.RunSync

            //调用领域服务
            entity = await _blogManager.CreateAsync(entity);

            if (input.TagIds != null && input.TagIds.Count > 0)
            {
                await _tagManager.SaveTags(input.TagIds, entity);
            }


            var dto = ObjectMapper.Map<BlogEditDto>(entity);
            return dto;
        }

        /// <summary>
        /// 编辑博客
        /// </summary>
        [AbpAuthorize(BlogPermissions.Edit)]
        protected virtual async Task Update(BlogEditDto input)
        {
            var user = await UserManager.GetUserByIdAsync(input.BlogUserId);


            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _blogRepository.GetAsync(input.Id.Value);
            entity.SetName(input.Name);
            entity.SetShortName(input.ShortName);
            entity.BlogUser = user;
            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);
            await _blogManager.UpdateAsync(entity);

            if (input.TagIds?.Count > 0)
            {
                await _tagManager.SaveTags(input.TagIds, entity);
            }
        }

        /// <summary>
        /// 删除博客信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(BlogPermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            var tags = await GetTagsOfBlog(input.Id);

            await _tagManager.DecreaseUsageCountOfTagsAsync(tags.Select(t => t.Id).ToList());

            await _blogManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Blog的方法
        /// </summary>
        [AbpAuthorize(BlogPermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            foreach (var id in input)
            {
                await Delete(new EntityDto<Guid>(id));
            }
        }

        /// <summary>
        /// 导出博客为excel文件
        /// </summary>
        /// <returns> </returns>
        [AbpAuthorize(BlogPermissions.ExportExcel)]
        public async Task<FileDto> GetToExcelFile()
        {
            var blogs = await _blogManager.QueryBlogs().ToListAsync();
            var blogListDtos = ObjectMapper.Map<List<BlogListDto>>(blogs);
            return _blogListExcelExporter.ExportToExcelFile(blogListDtos);
        }

        //// custom codes

        /// <summary>
        /// 根据短名称获取博客信息
        /// </summary>
        /// <param name="shortName"> </param>
        /// <returns> </returns>
        public async Task<BlogListDto> GetByShortNameAsync(string shortName)
        {

            var blog = await   _blogManager.GetByShortNameAsync(shortName);

            if (blog == null)
            {
                return null;
            }

            return ObjectMapper.Map<BlogListDto>(blog);
        }

        /// <summary>
        /// 获取博客下的标签列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<TagListDto>> GetTagsOfBlog(Guid id)
        {
            var blogTags = await _blogTagRepository.GetAllListAsync(a=>a.BlogId == id);
            if (blogTags.Count>0)
            {
                var tagIds = blogTags.Select(s => s.TagId).ToList();
                var tags = await _tagManager.QueryTagsAsNoTracking().Where(t => tagIds.Contains(t.Id)).ToListAsync();
                return ObjectMapper.Map<List<TagListDto>>(tags);
            }
            else
            {
                return new List<TagListDto>();
            }
        }

        //// custom codes end
        #region 私有方法

        ///// <summary>
        ///// 获取当前文章下的标签列表
        ///// </summary>
        ///// <param name="id"> 文章Id </param>
        ///// <returns> </returns>
        //private async Task<List<TagListDto>> GetTagsOfPost(Guid id)
        //{
        //    var postTags = await _blogRepository.GetAllListAsync(a => a.PostId == id);

        //    if (postTags.Count > 0)
        //    {
        //        var tagIds = postTags.Select(s => s.TagId).ToList();
        //        var tags = await _tagManager.QueryTagsAsNoTracking().Where(t => tagIds.Contains(t.Id)).ToListAsync();
        //        return ObjectMapper.Map<List<TagListDto>>(tags);
        //    }
        //    else
        //    {
        //        return new List<TagListDto>();
        //    }
        //}

        ///// <summary>
        ///// 查询出包含该标签的文章列表
        ///// </summary>
        ///// <param name="allPostDtos"> </param>
        ///// <param name="tag"> </param>
        ///// <returns> </returns>
        //private Task<List<PostDetailsDto>> FilterPostsByTag(List<PostDetailsDto> allPostDtos, Tag tag)
        //{
        //    var filteredPostDtos = allPostDtos.Where(p => p.Tags?.Any(t => t.Id == tag.Id) ?? false).ToList();

        //    return Task.FromResult(filteredPostDtos);
        //}

        ///// <summary>
        ///// 如果文章的阅读url已经存在，则对它进行重命名
        ///// </summary>
        ///// <param name="url"> 文章阅读url </param>
        ///// <param name="existingPost"> 已存在的文章信息 </param>
        ///// <returns> </returns>
        //private async Task<string> RenameUrlIfItAlreadyExistAsync(string url, Post existingPost = null)
        //{
        //    var postList = await _postManager.QueryPostsAsNoTracking().ToListAsync();
        //    //检查url是否存在，如果存在，且不是当前这篇文档的id，则对本篇文章添加5位数的guid值
        //    if (postList.Where(p => p.Url == url).WhereIf(existingPost != null, p => existingPost.Id != p.Id).Any())
        //    {
        //        return url + "-" + Guid.NewGuid().ToString().Substring(0, 5);
        //    }

        //    return url;
        //}

        #endregion 私有方法
    }
}
