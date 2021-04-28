using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users
{
    /// <summary>
    ///     用户信息服务接口
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        ///     创建或更新用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateUserInput input);

        /// <summary>
        ///     重置用户密码为默认密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> ResetPassword(NullableIdDto<long> input);


        /// <summary>
        ///     分页获取所有用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserListDto>> GetPaged(GetUsersInput input);


        /// <summary>
        ///     用户的权限编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserPermissionsTreeForEditOutput> GetPermissionsTreeForEdit(EntityDto<long> input);

        /// <summary>
        ///     修改用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserForEditTreeOutput> GetForEditTree(NullableIdDto<long> input);

        /// <summary>
        ///     重置用户权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ResetSpecificPermissions(EntityDto<long> input);

        /// <summary>
        ///     更新用户的权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdatePermissions(UpdateUserPermissionsInput input);

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<long> input);

        /// <summary>
        ///     解锁用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Unlock(EntityDto<long> input);

        /// <summary>
        /// 获取用户导出信息获取用户导出信息
        /// </summary>
        /// <returns></returns>

        Task<FileDto> GetUsersToExcel();

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="nullableIdDto"></param>
        /// <returns></returns>
        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> nullableIdDto);

        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> entityDto);
    }
}
