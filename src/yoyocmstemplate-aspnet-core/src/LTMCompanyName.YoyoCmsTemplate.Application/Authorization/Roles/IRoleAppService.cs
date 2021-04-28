using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        /// <summary>
        ///     根据权限取用户角色 默认全量数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ListResultDto<RoleListDto>> GetAll(GetRolesInput input);

        /// <summary>
        ///     根据权限分页取用户角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<RoleListDto>> GetPaged(GetRolePagedInput input);

        /// <summary>
        ///     取指定角色信息（包含授权列表、拥有权限列表）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetRoleForEditOutput> GetForEdit(NullableIdDto input);

        /// <summary>
        ///     更新或新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateRoleInput input);


        /// <summary>
        ///     删除指定角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto input);

        /// <summary>
        ///     更新指定角色的授权信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdatePermissions(UpdateRolePermissionsInput input);


        /// <summary>
        ///     批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task BatchDeleteAsync(List<int> ids);


        Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input);
    }
}