using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        /// <summary>
        ///     获取所有权限
        /// </summary>
        /// <returns></returns>
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();

        /// <summary>
        ///     获取树状权限
        /// </summary>
        /// <returns></returns>
        ListResultDto<TreePermissionDto> GetAllPermissionsTree();
    }
}