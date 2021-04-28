
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
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices
{
    /// <summary>
    /// 网站公告应用层服务的接口实现方法
    ///</summary>
    [AbpAuthorize]
    public class WebSiteNoticeAppService : YoyoCmsTemplateAppServiceBase, IWebSiteNoticeAppService
    {
        private readonly IRepository<WebSiteNotice, long> _webSiteNoticeRepository;



        private readonly IWebSiteNoticeManager _webSiteNoticeManager;
        /// <summary>
        /// 构造函数
        ///</summary>
        public WebSiteNoticeAppService(
        IRepository<WebSiteNotice, long> webSiteNoticeRepository
              , IWebSiteNoticeManager webSiteNoticeManager

             )
        {
            _webSiteNoticeRepository = webSiteNoticeRepository;
            _webSiteNoticeManager = webSiteNoticeManager; ;


        }


        /// <summary>
        /// 获取网站公告的分页列表信息
        ///      </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(WebSiteNoticePermissions.Query)]
        public async Task<PagedResultDto<WebSiteNoticeListDto>> GetPaged(GetWebSiteNoticesInput input)
        {

            var query = _webSiteNoticeRepository.GetAll()

                          //模糊搜索标题
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Title.Contains(input.FilterText))
                          //模糊搜索内容
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Content.Contains(input.FilterText))
            ;
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var webSiteNoticeList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var webSiteNoticeListDtos = ObjectMapper.Map<List<WebSiteNoticeListDto>>(webSiteNoticeList);

            return new PagedResultDto<WebSiteNoticeListDto>(count, webSiteNoticeListDtos);
        }


        /// <summary>
        /// 通过指定id获取WebSiteNoticeListDto信息
        /// </summary>
        [AbpAuthorize(WebSiteNoticePermissions.Query)]
        public async Task<WebSiteNoticeListDto> GetById(EntityDto<long> input)
        {
            var entity = await _webSiteNoticeRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<WebSiteNoticeListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 获取编辑 网站公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(WebSiteNoticePermissions.Create, WebSiteNoticePermissions.Edit)]
        public async Task<GetWebSiteNoticeForEditOutput> GetForEdit(NullableIdDto<long> input)
        {
            var output = new GetWebSiteNoticeForEditOutput();
            WebSiteNoticeEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _webSiteNoticeRepository.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<WebSiteNoticeEditDto>(entity);
            }
            else
            {
                editDto = new WebSiteNoticeEditDto();
            }



            output.WebSiteNotice = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改网站公告的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(WebSiteNoticePermissions.Create, WebSiteNoticePermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateWebSiteNoticeInput input)
        {

            if (input.WebSiteNotice.Id.HasValue)
            {
                await Update(input.WebSiteNotice);
            }
            else
            {
                await Create(input.WebSiteNotice);
            }
        }


        /// <summary>
        /// 新增网站公告
        /// </summary>
        [AbpAuthorize(WebSiteNoticePermissions.Create)]
        protected virtual async Task<WebSiteNoticeEditDto> Create(WebSiteNoticeEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<WebSiteNotice>(input);
            //调用领域服务
            entity = await _webSiteNoticeManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<WebSiteNoticeEditDto>(entity);
            return dto;
        }

        /// <summary>
        /// 编辑网站公告
        /// </summary>
        [AbpAuthorize(WebSiteNoticePermissions.Edit)]
        protected virtual async Task Update(WebSiteNoticeEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _webSiteNoticeRepository.GetAsync(input.Id.Value);
            //  input.MapTo(entity);
            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);
            await _webSiteNoticeManager.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除网站公告信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(WebSiteNoticePermissions.Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _webSiteNoticeManager.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除WebSiteNotice的方法
        /// </summary>
        [AbpAuthorize(WebSiteNoticePermissions.BatchDelete)]
        public async Task BatchDelete(List<long> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _webSiteNoticeManager.BatchDelete(input);
        }




        //// custom codes



        //// custom codes end

    }
}


