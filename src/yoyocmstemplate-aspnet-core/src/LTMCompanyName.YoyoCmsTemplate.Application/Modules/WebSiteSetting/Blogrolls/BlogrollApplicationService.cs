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
    ///     友情链接应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize]
    public class BlogrollAppService : YoyoCmsTemplateAppServiceBase, IBlogrollAppService
    {
        private readonly IBlogrollManager _blogrollManager;
        private readonly IRepository<Blogroll, int> _blogrollRepository;

        /// <summary>
        ///     构造函数
        /// </summary>
        public BlogrollAppService(
            IRepository<Blogroll, int> blogrollRepository
            , IBlogrollManager blogrollManager
        )
        {
            _blogrollRepository = blogrollRepository;
            _blogrollManager = blogrollManager;
            ;
        }


        /// <summary>
        ///     获取友情链接的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BlogrollPermissions.Query)]
        public async Task<PagedResultDto<BlogrollListDto>> GetPaged(GetBlogrollsInput input)
        {
            var query = _blogrollRepository.GetAll()

                    //模糊搜索名称
                    .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Name.Contains(input.FilterText))
                    //模糊搜索Url
                    .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Url.Contains(input.FilterText))
                    //模糊搜索Logo
                    .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Logo.Contains(input.FilterText))
                    //模糊搜索图标
                    .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.IconName.Contains(input.FilterText))
                ;
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var blogrollList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var blogrollListDtos = ObjectMapper.Map<List<BlogrollListDto>>(blogrollList);

            return new PagedResultDto<BlogrollListDto>(count, blogrollListDtos);
        }


        /// <summary>
        ///     通过指定id获取BlogrollListDto信息
        /// </summary>
        [AbpAuthorize(BlogrollPermissions.Query)]
        public async Task<BlogrollListDto> GetById(EntityDto<int> input)
        {
            var entity = await _blogrollRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<BlogrollListDto>(entity);
            return dto;
        }

        /// <summary>
        ///     获取编辑 友情链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BlogrollPermissions.Create, BlogrollPermissions.Edit)]
        public async Task<GetBlogrollForEditOutput> GetForEdit(NullableIdDto<int> input)
        {
            var output = new GetBlogrollForEditOutput();
            BlogrollEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _blogrollRepository.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<BlogrollEditDto>(entity);
            }
            else
            {
                editDto = new BlogrollEditDto();
            }


            output.Blogroll = editDto;
            return output;
        }


        /// <summary>
        ///     添加或者修改友情链接的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BlogrollPermissions.Create, BlogrollPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateBlogrollInput input)
        {
            if (input.Blogroll.Id.HasValue)
            {
                await Update(input.Blogroll);
            }
            else
            {
                await Create(input.Blogroll);
            }
        }


        /// <summary>
        ///     删除友情链接信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BlogrollPermissions.Delete)]
        public async Task Delete(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _blogrollManager.DeleteAsync(input.Id);
        }


        /// <summary>
        ///     批量删除Blogroll的方法
        /// </summary>
        [AbpAuthorize(BlogrollPermissions.BatchDelete)]
        public async Task BatchDelete(List<int> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _blogrollManager.BatchDelete(input);
        }


        /// <summary>
        ///     新增友情链接
        /// </summary>
        [AbpAuthorize(BlogrollPermissions.Create)]
        protected virtual async Task<BlogrollEditDto> Create(BlogrollEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Blogroll>(input);
            //调用领域服务
            entity = await _blogrollManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<BlogrollEditDto>(entity);
            return dto;
        }

        /// <summary>
        ///     编辑友情链接
        /// </summary>
        [AbpAuthorize(BlogrollPermissions.Edit)]
        protected virtual async Task Update(BlogrollEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _blogrollRepository.GetAsync(input.Id.Value);
            //  input.MapTo(entity);
            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);
            await _blogrollManager.UpdateAsync(entity);
        }


        //// custom codes


        //// custom codes end
    }
}
