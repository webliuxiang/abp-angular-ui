using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos.version;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects;
using Microsoft.EntityFrameworkCore;
using YoYo.ABPCommunity.Docs.Documents.Services;
using YoYo.ABPCommunity.Docs.Projects.DomainService;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects
{
    /// <summary>
    /// Project应用层服务的接口实现方法  
    ///</summary>
    public class ProjectAppService : YoyoCmsTemplateAppServiceBase, IProjectAppService
    {
        private readonly IRepository<Project, Guid> _projectRepository;

        private readonly IProjectManager _projectManager;
        private readonly ICacheManager _cacheManager;

        private readonly IDocumentStoreFactory _documentStoreFactory;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProjectAppService(
        IRepository<Project, Guid> entityRepository
        , IProjectManager entityManager, ICacheManager cacheManager, IDocumentStoreFactory documentStoreFactory)
        {
            _projectRepository = entityRepository;
            _projectManager = entityManager;
            _cacheManager = cacheManager;
            _documentStoreFactory = documentStoreFactory;
        }


        /// <summary>
        /// 获取Project的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[AbpAuthorize(ProjectPermissions.Query)]
        public async Task<PagedResultDto<ProjectListDto>> GetPaged(GetProjectsInput input)
        {

            var query = _projectRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<ProjectListDto>>(entityList);
            // var entityListDtos = entityList.MapTo<List<ProjectListDto>>();

            return new PagedResultDto<ProjectListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 获取所有的项目列表
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<ProjectDto>> GetListAsync()
        {
            var projects = await _projectRepository.GetAll()
                .AsNoTracking()
                .Where(o => o.Enabled)
                .OrderByDescending(o => o.Sort)
                .ToListAsync();

            var projectDtos = ObjectMapper.Map<List<ProjectDto>>(projects);
            //var projectDtos = projects.MapTo<List<ProjectDto>>();

            return new ListResultDto<ProjectDto>(projectDtos);
        }


        /// <summary>
        /// 通过指定id获取ProjectListDto信息
        /// </summary>
        [AbpAuthorize(ProjectPermissions.Query)]
        public async Task<ProjectListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _projectRepository.GetAsync(input.Id);

            return ObjectMapper.Map<ProjectListDto>(entity);
            //return entity.MapTo<ProjectListDto>();
        }

        /// <summary>
        /// 获取编辑 Project
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ProjectPermissions.Create, ProjectPermissions.Edit)]
        public async Task<GetProjectForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetProjectForEditOutput();
            ProjectEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _projectRepository.GetAsync(input.Id.Value);

                //editDto = entity.MapTo<ProjectEditDto>();

                editDto = ObjectMapper.Map<ProjectEditDto>(entity);

                //projectEditDto = ObjectMapper.Map<List<projectEditDto>>(entity);
            }
            else
            {
                editDto = new ProjectEditDto();
            }

            output.Project = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Project的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ProjectPermissions.Create, ProjectPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateProjectInput input)
        {

            if (input.Project.Id.HasValue)
            {
                await Update(input.Project);
            }
            else
            {
                await Create(input.Project);
            }
        }


        /// <summary>
        /// 新增Project
        /// </summary>
        [AbpAuthorize(ProjectPermissions.Create)]
        protected virtual async Task<ProjectEditDto> Create(ProjectEditDto input)
        {
//TODO:新增前的逻辑判断，是否允许新增

generatorId:
            input.Id = Guid.NewGuid();
            var count = await _projectRepository.GetAll().Where(o => o.Id == input.Id).CountAsync();
            if (count > 0)
            {
                // 如果生成的Id已经存在了,那么不暂停一秒，重新生成
                await Task.Delay(1000);
                goto generatorId;
            }

            var entity = ObjectMapper.Map<Project>(input);
            entity = await _projectRepository.InsertAsync(entity);
            input.Id = entity.Id;
            return input;
        }

        /// <summary>
        /// 编辑Project
        /// </summary>
        [AbpAuthorize(ProjectPermissions.Edit)]
        protected virtual async Task Update(ProjectEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _projectRepository.GetAsync(input.Id.Value);
            //input.MapTo(entity);

            ObjectMapper.Map(input, entity);
            await _projectRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Project信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ProjectPermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _projectRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Project的方法
        /// </summary>
        [AbpAuthorize(ProjectPermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _projectRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        public async Task<ProjectDto> FindByShortNameAsync(string shortName)
        {
            var project = await _projectManager.FindByShortNameAsync(shortName);
            if (project == null)
            {
                throw new EntityNotFoundException($"Project with the name {shortName} not found!");
            }

            return ObjectMapper.Map<ProjectDto>(project);
        }




        /// <summary>
        /// 获取项目的版本信息，带缓存
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        public async Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName)
        {
            var project = await _projectManager.FindByShortNameAsync(shortName);


            var versionCache = _cacheManager.GetCache(shortName).AsTyped<string, List<VersionInfo>>();

            var versionInfos = await versionCache
                .GetOrDefaultAsync(shortName);


            if (versionInfos == null)
            {
                versionInfos = await GetVersionsAsync(project);
                //设置缓存，获取缓存信息
                versionCache.Set(project.ShortName, versionInfos, TimeSpan.FromMinutes(5));
            }



            var dtos = ObjectMapper.Map<List<VersionInfoDto>>(versionInfos);

            return new ListResultDto<VersionInfoDto>(dtos);


        }

        /// <summary>
        /// 获取项目的版本信息
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>

        protected virtual async Task<List<VersionInfo>> GetVersionsAsync(Project project)
        {
            var store = _documentStoreFactory.Create(project.DocumentStoreType);

            var versions = await store.GetVersionsAsync(project);

            if (!versions.Any())
            {
                return versions;
            }

            if (!project.MinimumVersion.IsNullOrEmpty())
            {
                var minVersionIndex = versions.FindIndex(v => v.Name == project.MinimumVersion);
                if (minVersionIndex > -1)
                {
                    versions = versions.GetRange(0, minVersionIndex + 1);
                }
            }

            if (versions.Any() && !string.IsNullOrEmpty(project.LatestVersionBranchName))
            {
                versions.First().Name = project.LatestVersionBranchName;
            }

            return versions;
        }




        private async Task<List<string>> GetVersionsFromCache(string projectShortName)
        {
            var cache = _cacheManager.GetCache(CacheConsts.Portal_Wiki).AsTyped<string, List<string>>();
            return await cache.GetOrDefaultAsync(projectShortName);
        }

        private async Task SetVersionsToCache(string projectShortName, List<string> versions)
        {
            var cache = _cacheManager.GetCache(CacheConsts.Portal_Wiki);



            await cache.SetAsync(projectShortName, versions, TimeSpan.FromMinutes(5));
        }
    }
}


