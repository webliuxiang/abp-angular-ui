using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs
{
    /// <summary>
    /// DownloadLog应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class DownloadLogAppService : YoyoCmsTemplateAppServiceBase, IDownloadLogAppService
    {

        private readonly IDownloadLogManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DownloadLogAppService(
            IDownloadLogManager entityManager
        )
        {
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取DownloadLog的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[AbpAuthorize(DownloadLogPermissions.Node)]
        public async Task<PagedResultDto<DownloadLogListDto>> GetPaged(GetDownloadLogsInput input)
        {

            var query = _entityManager.QueryAsNoTracking;
            // TODO:根据传入的参数添加过滤条件
            query = query
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),
                o => input.FilterText.Contains(o.UserName) || input.FilterText.Contains(o.ProjectName));

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<DownloadLogListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<DownloadLogListDto>>();

            return new PagedResultDto<DownloadLogListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取DownloadLogListDto信息
        /// </summary>
        [AbpAuthorize(DownloadLogPermissions.Node)]
        public async Task<DownloadLogListDto> GetById(EntityDto<long> input)
        {
            var entity = await _entityManager.FindById(input.Id);

            return ObjectMapper.Map<DownloadLogListDto>(entity);

            //return entity.MapTo<DownloadLogListDto>();
        }


        ///// <summary>
        ///// 导出DownloadLog为excel表,等待开发。
        ///// </summary>
        ///// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}

    }
}


