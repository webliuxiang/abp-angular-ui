using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.ObjectMapping;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.DynamicView.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.DynamicView.Dtos;
using Masuit.Tools;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.DynamicView.Dtos
{
    /// <summary>
    /// 动态页面api
    /// </summary>
    [AbpAuthorize]
    public class DynamicPageAppService : IApplicationService
    {

        static DynamicPageDto s_defaultDynamicPageInfoResult = new DynamicPageDto();

        static ListResultDto<PageFilterItemDto> s_defaultPageFilterResult = new ListResultDto<PageFilterItemDto>(
                new List<PageFilterItemDto>()
            );

        static ListResultDto<ColumnItemDto> s_defaultColumnResult = new ListResultDto<ColumnItemDto>(
              new List<ColumnItemDto>()
          );


        readonly IDynamicViewManager _dynamicViewManager;

        readonly IObjectMapper _objectMapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dynamicViewManager"></param>
        /// <param name="objectMapper"></param>
        public DynamicPageAppService(IDynamicViewManager dynamicViewManager, IObjectMapper objectMapper)
        {
            _dynamicViewManager = dynamicViewManager;
            _objectMapper = objectMapper;
        }

        /// <summary>
        /// 获取页面配置信息
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns>页面配置</returns>
        public async Task<DynamicPageDto> GetDynamicPageInfo(string name)
        {
            var pageInfo = await _dynamicViewManager.GetDynamicPageInfo(name);
            if (pageInfo == null)
            {
                return s_defaultDynamicPageInfoResult;
            }

            return _objectMapper.Map<DynamicPageDto>(pageInfo);
        }

        /// <summary>
        /// 根据名称获取对应的筛选条件配置
        /// </summary>
        /// <param name="name">筛选条件名称</param>
        /// <returns>筛选条件配置</returns>
        public async Task<ListResultDto<PageFilterItemDto>> GetPageFilters(string name)
        {
            var pageFilters = await _dynamicViewManager.GetPageFilters(name);
            if (pageFilters.Count() == 0)
            {
                return s_defaultPageFilterResult;
            }


            return new ListResultDto<PageFilterItemDto>(
                _objectMapper.Map<IReadOnlyList<PageFilterItemDto>>(pageFilters)
                );
        }

        /// <summary>
        /// 根据名称获取对应的筛选条件配置
        /// </summary>
        /// <param name="name">筛选条件名称</param>
        /// <returns>筛选条件配置</returns>
        public async Task<ListResultDto<ColumnItemDto>> GetColumns(string name)
        {
            var columns = await _dynamicViewManager.GetColumns(name);
            if (columns.Count() == 0)
            {
                return s_defaultColumnResult;
            }

            return new ListResultDto<ColumnItemDto>(
                _objectMapper.Map<IReadOnlyList<ColumnItemDto>>(columns)
                );
        }
    }
}
