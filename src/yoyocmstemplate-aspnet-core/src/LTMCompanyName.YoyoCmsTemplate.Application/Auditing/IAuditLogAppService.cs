using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos.EntityChange;

namespace LTMCompanyName.YoyoCmsTemplate.Auditing
{
    /// <summary>
    ///     审计日志服务
    /// </summary>
    public interface IAuditLogAppService : IApplicationService
    {

        /// <summary>
        /// 分页获取审计日志内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AuditLogListDto>> GetPagedAuditLogs(GetAuditLogsInput input);


        /// <summary>
        /// 导出审计日志Excel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FileDto> GetAuditLogsToExcelAsync(GetAuditLogsInput input);




        /// <summary>
        /// 获取EntityHistory对象类型
        /// </summary>
        /// <returns></returns>
        List<NameValueDto> GetEntityHistoryObjectTypes();


        Task<PagedResultDto<EntityChangeListDto>> GetEntityChanges(GetEntityChangeInput input);

        Task<FileDto> GetEntityChangesToExcel(GetEntityChangeInput input);



    }
}