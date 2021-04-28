using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls
{
    /// <summary>
    ///     友情链接分类应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize]
    public class BlogrollTypeAppService : YoyoCmsTemplateAppServiceBase, IBlogrollTypeAppService
    {
        private readonly IBlogrollTypeManager _blogrollTypeManager;
        private readonly IRepository<BlogrollType, int> _blogrollTypeRepository;

        /// <summary>
        ///     构造函数
        /// </summary>
        public BlogrollTypeAppService(
            IRepository<BlogrollType, int> blogrollTypeRepository
            , IBlogrollTypeManager blogrollTypeManager
        )
        {
            _blogrollTypeRepository = blogrollTypeRepository;
            _blogrollTypeManager = blogrollTypeManager;
            ;
        }


        /// <summary>
        ///     获取友情链接分类的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BlogrollTypePermissions.Query)]
        public async Task<PagedResultDto<BlogrollTypeListDto>> GetPaged(GetBlogrollTypesInput input)
        {
            var query = _blogrollTypeRepository.GetAll()

                    //模糊搜索分类名称
                    .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Name.Contains(input.FilterText))
                ;
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var blogrollTypeList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var blogrollTypeListDtos = ObjectMapper.Map<List<BlogrollTypeListDto>>(blogrollTypeList);

            return new PagedResultDto<BlogrollTypeListDto>(count, blogrollTypeListDtos);
        }


        /// <summary>
        ///     通过指定id获取BlogrollTypeListDto信息
        /// </summary>
        [AbpAuthorize(BlogrollTypePermissions.Query)]
        public async Task<BlogrollTypeListDto> GetById(EntityDto<int> input)
        {
            var entity = await _blogrollTypeRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<BlogrollTypeListDto>(entity);
            return dto;
        }

        /// <summary>
        ///     获取编辑 友情链接分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BlogrollTypePermissions.Create, BlogrollTypePermissions.Edit)]
        public async Task<GetBlogrollTypeForEditOutput> GetForEdit(NullableIdDto<int> input)
        {
            var output = new GetBlogrollTypeForEditOutput();
            BlogrollTypeEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _blogrollTypeRepository.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<BlogrollTypeEditDto>(entity);
            }
            else
            {
                editDto = new BlogrollTypeEditDto();
            }


            output.BlogrollType = editDto;
            return output;
        }


        /// <summary>
        ///     添加或者修改友情链接分类的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BlogrollTypePermissions.Create, BlogrollTypePermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateBlogrollTypeInput input)
        {
            if (input.BlogrollType.Id.HasValue)
            {
                await Update(input.BlogrollType);
            }
            else
            {
                await Create(input.BlogrollType);
            }
        }


        /// <summary>
        ///     删除友情链接分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BlogrollTypePermissions.Delete)]
        public async Task Delete(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _blogrollTypeManager.DeleteAsync(input.Id);
        }


        /// <summary>
        ///     批量删除BlogrollType的方法
        /// </summary>
        [AbpAuthorize(BlogrollTypePermissions.BatchDelete)]
        public async Task BatchDelete(List<int> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _blogrollTypeManager.BatchDelete(input);
        }


        /// <summary>
        ///     新增友情链接分类
        /// </summary>
        [AbpAuthorize(BlogrollTypePermissions.Create)]
        protected virtual async Task<BlogrollTypeEditDto> Create(BlogrollTypeEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<BlogrollType>(input);
            //调用领域服务
            entity = await _blogrollTypeManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<BlogrollTypeEditDto>(entity);
            return dto;
        }

        /// <summary>
        ///     编辑友情链接分类
        /// </summary>
        [AbpAuthorize(BlogrollTypePermissions.Edit)]
        protected virtual async Task Update(BlogrollTypeEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _blogrollTypeRepository.GetAsync(input.Id.Value);
            //  input.MapTo(entity);
            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);
            await _blogrollTypeManager.UpdateAsync(entity);
        }


        //// custom codes


        //// custom codes end
    }
}
