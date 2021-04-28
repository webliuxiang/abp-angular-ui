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
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Exporting;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging.DomainService;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging
{
    /// <summary>
    /// 标签应用层服务的接口实现方法
    ///</summary>
    public class TagAppService : YoyoCmsTemplateAppServiceBase, ITagAppService
    {
        private readonly IRepository<Tag, Guid> _tagRepository;

        private readonly ITagListExcelExporter _tagListExcelExporter;

        private readonly ITagManager _tagManager;

        private readonly IRepository<PostTag, int> _postTagRepository;

        private readonly IRepository<BlogTag, int> _blogTagRepository;

        /// <summary>
        /// 构造函数
        ///</summary>
        public TagAppService(IRepository<Tag, Guid> tagRepository, ITagManager tagManager, TagListExcelExporter tagListExcelExporter, IRepository<PostTag, int> postTagRepository, IRepository<BlogTag, int> blogTagRepository)
        {
            _tagRepository = tagRepository;
            _tagManager = tagManager;
            _tagListExcelExporter = tagListExcelExporter;
            _postTagRepository = postTagRepository;
            _blogTagRepository = blogTagRepository;
        }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>

        [AbpAuthorize(TagPermissions.Query)]
        public async Task<List<TagListDto>> GetAll()
        {
            var query = await _tagRepository.GetAll().OrderByDescending(x => x.UsageCount).ToListAsync();
            var tagListDtos = ObjectMapper.Map<List<TagListDto>>(query);

            return tagListDtos;
        }


        /// <summary>
        /// 获取标签的分页列表信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(TagPermissions.Query)]
        public async Task<PagedResultDto<TagListDto>> GetPaged(GetTagsInput input)
        {
            var query = _tagRepository.GetAll()

                          //模糊搜索标签名称
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Name.Contains(input.FilterText))
                          //模糊搜索标签描述
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Description.Contains(input.FilterText))
            ;

            var count = await query.CountAsync();

            var tagList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var tagListDtos = ObjectMapper.Map<List<TagListDto>>(tagList);

            return new PagedResultDto<TagListDto>(count, tagListDtos);
        }

        /// <summary>
        /// 通过指定id获取TagListDto信息
        /// </summary>
        [AbpAuthorize(TagPermissions.Query)]
        public async Task<TagListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _tagRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<TagListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 获取编辑 标签
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(TagPermissions.Create, TagPermissions.Edit)]
        public async Task<GetTagForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetTagForEditOutput();
            TagEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _tagRepository.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<TagEditDto>(entity);
            }
            else
            {
                editDto = new TagEditDto();
            }

            output.Tag = editDto;
            return output;
        }

        /// <summary>
        /// 添加或者修改标签的公共方法
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(TagPermissions.Create, TagPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateTagInput input)
        {
            if (input.Tag.Id.HasValue)
            {
                await Update(input.Tag);
            }
            else
            {
                await Create(input.Tag);
            }
        }

        /// <summary>
        /// 新增标签
        /// </summary>
        [AbpAuthorize(TagPermissions.Create)]
        protected virtual async Task<TagEditDto> Create(TagEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Tag>(input);

            //调用领域服务
            entity = await _tagManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<TagEditDto>(entity);
            dto.PostId = input.PostId;
            dto.BlogId = input.BlogId;

            await BlogOrPostOfTag(dto,entity);
            return dto;
        }

        /// <summary>
        /// 编辑标签
        /// </summary>
        [AbpAuthorize(TagPermissions.Edit)]
        protected virtual async Task Update(TagEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _tagRepository.GetAsync(input.Id.Value);
            //  input.MapTo(entity);
            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);
            await _tagManager.UpdateAsync(entity);


            await BlogOrPostOfTag(input,entity, true);
        }

        /// <summary>
        /// 博客或者文章的标签更新
        /// </summary>
        /// <returns></returns>
        protected virtual async Task BlogOrPostOfTag(TagEditDto input,Tag tag,bool isUpdate = false)
        {
            //如果是修改，把以前的删了重建
            if (isUpdate)
            {
                tag.DecreaseUsageCount();

                await _blogTagRepository.DeleteAsync(x => x.TagId == input.Id);

                await _postTagRepository.DeleteAsync(x => x.TagId == input.Id);
            }


       
            if (input.BlogId != null)
            {
                tag.IncreaseUsageCount();
                await _blogTagRepository.InsertAsync(new BlogTag((Guid)input.BlogId, (Guid)input.Id));               
            }
            if (input.PostId != null)
            {
                tag.IncreaseUsageCount();
                await _postTagRepository.InsertAsync(new PostTag((Guid)input.PostId, (Guid)input.Id));
            }
        }


        /// <summary>
        /// 删除标签信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(TagPermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _tagManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Tag的方法
        /// </summary>
        [AbpAuthorize(TagPermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _tagManager.BatchDelete(input);
        }

        /// <summary>
        /// 导出标签为excel文件
        /// </summary>
        /// <returns> </returns>
        [AbpAuthorize(TagPermissions.ExportExcel)]
        public async Task<FileDto> GetToExcelFile()
        {
            var tags = await _tagManager.QueryTags().ToListAsync();
            var tagListDtos = ObjectMapper.Map<List<TagListDto>>(tags);
            return _tagListExcelExporter.ExportToExcelFile(tagListDtos);
        }

        /// <summary>
        /// 获取流行的标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<TagListDto>> GetPopularTags(GetPopularTagsInput input)
        {

            var blogTags = await _tagRepository.GetAllListAsync();
            var postTags = blogTags.OrderByDescending(t => t.UsageCount)
                 .WhereIf(input.MinimumPostCount != null, t => t.UsageCount >= input.MinimumPostCount)
                 .Take(input.ResultCount).ToList();

            var dtos = ObjectMapper.Map<List<TagListDto>>(postTags);

            return dtos;

        }

        public IQueryable<TagListDto> GetPostTag(Guid postId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TagListDto> GetBlogTag(Guid blogId)
        {

            var tagQuery = from entity in _blogTagRepository.GetAll().AsNoTracking().Where(x => x.BlogId == blogId)
                join tag in _tagRepository.GetAll().AsNoTracking() on entity.TagId equals tag.Id
                select new TagListDto()
                {
                    Name = tag.Name,
                    Description = tag.Description,
                    UsageCount = tag.UsageCount
                };

            return tagQuery;
        }

        //// custom codes

        //// custom codes end
    }
}
