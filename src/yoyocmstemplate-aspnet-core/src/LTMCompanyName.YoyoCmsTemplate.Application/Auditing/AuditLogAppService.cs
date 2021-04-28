using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Extensions;
using Abp.Linq.Extensions;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos.EntityChange;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Exporting;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.EntityHistory;
using LTMCompanyName.YoyoCmsTemplate.NamespaceHelper;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.EntityFrameworkCore;
using EntityHistoryHelper = LTMCompanyName.YoyoCmsTemplate.EntityHistory.EntityHistoryHelper;

namespace LTMCompanyName.YoyoCmsTemplate.Auditing
{
    [DisableAuditing]
    [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_AuditLogs)]
    public class AuditLogAppService : YoyoCmsTemplateAppServiceBase, IAuditLogAppService
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly NamespaceHelperManager _namespaceHelperManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAuditLogListExcelExporter _auditLogListExcelExporter;
        private readonly IAbpStartupConfiguration _abpStartupConfiguration;
        private readonly IRepository<EntityChange, long> _entityChangeRepository;
        private readonly IRepository<EntityChangeSet, long> _entityChangeSetRepository;

        private readonly IRepository<EntityPropertyChange, long> _entityPropertyChangeRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="auditLogRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="namespaceHelperManager"></param>
        /// <param name="auditLogListExcelExporter"></param>
        /// <param name="abpStartupConfiguration"></param>
        /// <param name="entityChangeRepository"></param>
        /// <param name="entityChangeSetRepository"></param>
        /// <param name="entityPropertyChangeRepository"></param>
        public AuditLogAppService(IRepository<AuditLog, long> auditLogRepository,
            IRepository<User, long> userRepository, NamespaceHelperManager namespaceHelperManager, IAuditLogListExcelExporter auditLogListExcelExporter, IAbpStartupConfiguration abpStartupConfiguration, IRepository<EntityChange, long> entityChangeRepository, IRepository<EntityChangeSet, long> entityChangeSetRepository, IRepository<EntityPropertyChange, long> entityPropertyChangeRepository)
        {
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            _namespaceHelperManager = namespaceHelperManager;
            _auditLogListExcelExporter = auditLogListExcelExporter;
            _abpStartupConfiguration = abpStartupConfiguration;
            _entityChangeRepository = entityChangeRepository;
            _entityChangeSetRepository = entityChangeSetRepository;
            _entityPropertyChangeRepository = entityPropertyChangeRepository;
        }


        [DisableAuditing]
        public async Task<PagedResultDto<AuditLogListDto>> GetPagedAuditLogs(GetAuditLogsInput input)
        {
            var query = CreateAuditLogAndUsersQuery(input);

            var resultCount = await query.CountAsync();
            var results = await query
                .AsNoTracking()
                .OrderBy(input.Sorting)// TODO: OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var auditLogListDtos = ConvertToAuditLogListDtos(results);

            //   _userRepository.GetAll().OrderByDescending()

            return new PagedResultDto<AuditLogListDto>(resultCount, auditLogListDtos);
        }

        [DisableAuditing]
        public async Task<FileDto> GetAuditLogsToExcelAsync(GetAuditLogsInput input)
        {
            var auditLogs = await CreateAuditLogAndUsersQuery(input)
                .AsNoTracking()
                .OrderByDescending(al => al.AuditLogInfo.ExecutionTime)
                .ToListAsync();

            var auditLogListDtos = ConvertToAuditLogListDtos(auditLogs);

            return _auditLogListExcelExporter.ExportAuditLogToFile(auditLogListDtos);
        }






        #region EntityHistory 服务
        public List<NameValueDto> GetEntityHistoryObjectTypes()
        {
            var entityHistoryObjectTypes = new List<NameValueDto>();
            var enabledEntities = (_abpStartupConfiguration.GetCustomConfig()
                                      .FirstOrDefault(x => x.Key == EntityHistoryHelper.EntityHistoryConfigurationName)
                                      .Value as EntityHistoryUiSetting)?.EnabledEntities ?? new List<string>();

            if (AbpSession.TenantId == null)
            {
                enabledEntities = EntityHistoryHelper.HostSideTrackedTypes.Select(t => t.FullName).Intersect(enabledEntities).ToList();
            }
            else
            {
                enabledEntities = EntityHistoryHelper.TenantSideTrackedTypes.Select(t => t.FullName).Intersect(enabledEntities).ToList();
            }

            foreach (var enabledEntity in enabledEntities)
            {
                entityHistoryObjectTypes.Add(new NameValueDto(L(enabledEntity), enabledEntity));
            }

            return entityHistoryObjectTypes;
        }
        public async Task<PagedResultDto<EntityChangeListDto>> GetEntityChanges(GetEntityChangeInput input)
        {
            var query = CreateEntityChangesAndUsersQuery(input);

            var resultCount = await query.CountAsync();
            var results = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var entityChangeListDtos = ConvertToEntityChangeListDtos(results);

            return new PagedResultDto<EntityChangeListDto>(resultCount, entityChangeListDtos);
        }

        public async Task<FileDto> GetEntityChangesToExcel(GetEntityChangeInput input)
        {
            var entityChanges = await CreateEntityChangesAndUsersQuery(input)
                .AsNoTracking()
                .OrderByDescending(ec => ec.EntityChange.EntityChangeSetId)
                .ThenByDescending(ec => ec.EntityChange.ChangeTime)
                .ToListAsync();

            var entityChangeListDtos = ConvertToEntityChangeListDtos(entityChanges);

            return _auditLogListExcelExporter.ExportEntityChangeToFile(entityChangeListDtos);
        }



        #region EntityChange私有服务方法

        private IQueryable<EntityChangeAndUser> CreateEntityChangesAndUsersQuery(GetEntityChangeInput input)
        {
            var query = from entityChangeSet in _entityChangeSetRepository.GetAll()
                        join entityChange in _entityChangeRepository.GetAll() on entityChangeSet.Id equals entityChange.EntityChangeSetId
                        join user in _userRepository.GetAll() on entityChangeSet.UserId equals user.Id
                        where entityChange.ChangeTime >= input.StartDate && entityChange.ChangeTime <= input.EndDate
                        select new EntityChangeAndUser
                        {
                            EntityChange = entityChange,
                            User = user
                        };

            query = query
                .WhereIf(!input.UserName.IsNullOrWhiteSpace(), item => item.User.UserName.Contains(input.UserName))
                .WhereIf(!input.EntityTypeFullName.IsNullOrWhiteSpace(), item => item.EntityChange.EntityTypeFullName.Contains(input.EntityTypeFullName));

            return query;
        }


        private List<EntityChangeListDto> ConvertToEntityChangeListDtos(List<EntityChangeAndUser> results)
        {
            return results.Select(
                result =>
                {
                    var entityChangeListDto = ObjectMapper.Map<EntityChangeListDto>(result.EntityChange);
                    entityChangeListDto.UserName = result.User?.UserName;
                    return entityChangeListDto;
                }).ToList();
        }

        #endregion


        #endregion





        #region 审计日志私有服务

        /// <summary>
        ///     创建审计日志用户的查询服务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IQueryable<AuditLogAndUser> CreateAuditLogAndUsersQuery(GetAuditLogsInput input)
        {
            var auditLogQuery = _auditLogRepository.GetAll().AsNoTracking().IgnoreQueryFilters();

            var userQuery = _userRepository.GetAll().AsNoTracking().IgnoreQueryFilters();


            var query = from auditLog in auditLogQuery

                        join user in userQuery on auditLog.UserId equals user.Id into userJoin
                        from joinedUser in userJoin.DefaultIfEmpty()

                        select new AuditLogAndUser
                        {
                            AuditLogInfo = auditLog,
                            UserInfo = joinedUser
                        };

            query = query
                .Where(o => !o.UserInfo.IsDeleted && o.UserInfo.TenantId == AbpSession.TenantId
                 && o.AuditLogInfo.TenantId == AbpSession.TenantId)

                .WhereIf(input.StartDate.HasValue && input.EndDate.HasValue, o => o.AuditLogInfo.ExecutionTime >= input.StartDate && o.AuditLogInfo.ExecutionTime <= input.EndDate)
                .WhereIf(!input.UserName.IsNullOrWhiteSpace(), item => item.UserInfo.UserName.Contains(input.UserName))
                .WhereIf(!input.ServiceName.IsNullOrWhiteSpace(),
                    item => item.AuditLogInfo.ServiceName.Contains(input.ServiceName))
                .WhereIf(!input.MethodName.IsNullOrWhiteSpace(),
                    item => item.AuditLogInfo.MethodName.Contains(input.MethodName))
                .WhereIf(!input.BrowserInfo.IsNullOrWhiteSpace(),
                    item => item.AuditLogInfo.BrowserInfo.Contains(input.BrowserInfo))
                .WhereIf(input.MinExecutionDuration.HasValue && input.MinExecutionDuration > 0,
                    item => item.AuditLogInfo.ExecutionDuration >= input.MinExecutionDuration.Value)
                .WhereIf(input.MaxExecutionDuration.HasValue && input.MaxExecutionDuration < int.MaxValue,
                    item => item.AuditLogInfo.ExecutionDuration <= input.MaxExecutionDuration.Value)
                .WhereIf(input.HasException == true,
                    item => item.AuditLogInfo.Exception != null && item.AuditLogInfo.Exception != "")
                .WhereIf(input.HasException == false,
                    item => item.AuditLogInfo.Exception == null || item.AuditLogInfo.Exception == "");
            return query;
        }

        private List<AuditLogListDto> ConvertToAuditLogListDtos(List<AuditLogAndUser> results)
        {
            return results.Select(
                result =>
                {
                    var auditLogListDto = ObjectMapper.Map<AuditLogListDto>(result.AuditLogInfo);
                    auditLogListDto.UserName = result.UserInfo?.UserName;
                    auditLogListDto.ServiceName = _namespaceHelperManager.SplitNameSpace(auditLogListDto.ServiceName);
                    return auditLogListDto;
                }).ToList();
        }



        #endregion
    }

}
