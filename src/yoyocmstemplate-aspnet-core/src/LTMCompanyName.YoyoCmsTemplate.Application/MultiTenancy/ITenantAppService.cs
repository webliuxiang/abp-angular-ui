using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        /// <summary>
        ///     分页获取租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TenantListDto>> GetPaged(GetTenantsInput input);

        /// <summary>
        ///     创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Create(CreateTenantInput input);

        /// <summary>
        ///     获取编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TenantEditDto> GetForEdit(EntityDto input);

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Update(TenantEditDto input);

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto input);

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchDelete(List<EntityDto> input);

        /// <summary>
        ///     解锁租户的Admin用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UnlockTenantAdmin(EntityDto input);

        /// <summary>
        /// 获取租户‘功能’配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input);

        /// <summary>
        /// 更新租户‘功能’配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateTenantFeatures(UpdateTenantFeaturesInput input);

        /// <summary>
        /// 重置租户‘功能’配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ResetTenantSpecificFeatures(EntityDto input);

    }
}