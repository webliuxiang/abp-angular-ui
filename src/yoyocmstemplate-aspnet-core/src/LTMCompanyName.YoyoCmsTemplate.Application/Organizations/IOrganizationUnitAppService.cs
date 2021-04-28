using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations
{
    public interface IOrganizationUnitAppService : IApplicationService
    {
        /// <summary>
        ///     获取组织机构（树形结构数据）
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<OrganizationUnitListDto>> GetAllOrganizationUnitList();

        /// <summary>
        ///     新增组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OrganizationUnitListDto> Create(CreateOrganizationUnitInput input);

        /// <summary>
        ///     通过Id更新组织机构名称
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OrganizationUnitListDto> Update(UpdateOrganizationUnitInput input);

        /// <summary>
        ///     删除指定组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<long> input);

        /// <summary>
        ///     将目标组织机构移入到指定组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OrganizationUnitListDto> Move(MoveOrganizationUnitInput input);


        /// <summary>
        ///     分页获取组织机构下的用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrganizationUnitUserListDto>> GetPagedOrganizationUnitUsers(
            GetOrganizationUnitUsersInput input);


        /// <summary>
        /// 获取组织机构下的角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrganizationUnitRoleListDto>> GetPagedOrganizationUnitRolesAsync(GetOrganizationUnitRolesInput input);



        /// <summary>
        ///     将用户移入组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddUsers(UsersToOrganizationUnitInput input);

        /// <summary>
        ///     将用户移除组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task RemoveUser(UserToOrganizationUnitInput input);


        /// <summary>
        ///     用户是否已经在组织机构下面
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> IsInOrganizationUnit(UserToOrganizationUnitInput input);


        /// <summary>
        ///     查找用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        /// <summary>
        ///     批量从组织中移除用户
        /// </summary>
        /// <param name="userIds">用户Id列表</param>
        /// <param name="organizationUnitId">组织机构Id</param>
        /// <returns></returns>
        Task BatchRemoveUserFromOrganizationUnit(List<long> userIds, long organizationUnitId);


        Task AddRoles(RolesToOrganizationUnitInput input);

        Task RemoveRole(RoleToOrganizationUnitInput input);

        Task<PagedResultDto<NameValueDto>> FindRoles(FindUsersInput input);

        Task BatchRemoveRoleFromOrganizationUnit(List<int> roleIds, long organizationUnitId);




    }
}
