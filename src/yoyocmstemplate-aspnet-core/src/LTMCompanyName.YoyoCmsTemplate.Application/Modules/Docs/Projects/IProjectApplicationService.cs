using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos.version;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects
{
    /// <summary>
    /// Project应用层服务的接口方法
    ///</summary>
    public interface IProjectAppService : IApplicationService
    {
        /// <summary>
		/// 获取Project的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProjectListDto>> GetPaged(GetProjectsInput input);



		/// <summary>
		/// 通过指定id获取ProjectListDto信息
		/// </summary>
		Task<ProjectListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Project的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateProjectInput input);


        /// <summary>
        /// 删除Project信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Project
        /// </summary>
        Task BatchDelete(List<Guid> input);



        /// <summary>
        /// 获取所有Project
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<ProjectDto>> GetListAsync();

        /// <summary>
        /// 获取项目的版本
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        Task<ProjectDto> FindByShortNameAsync(string shortName);


        ///// <summary>
        ///// 导出Project为excel表
        ///// </summary>
        ///// <returns></returns>
        //Task<FileDto> GetToExcel();


        #region 扩展功能


     



        #endregion




    }
}
