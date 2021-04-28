using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations
{
    /// <summary>
    /// 组织单元服务
    /// </summary>
    [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits)]
    public class OrganizationUnitAppService : YoyoCmsTemplateAppServiceBase, IOrganizationUnitAppService
    {
        #region OrganizationUnit private

        private async Task<OrganizationUnitListDto> CreateOrganizationUnit(OrganizationUnit organizationUnit)
        {
            var dto = ObjectMapper.Map<OrganizationUnitListDto>(organizationUnit);
            dto.MemberCount =
                await _userOrganizationUnitRepository.CountAsync(uou => uou.OrganizationUnitId == organizationUnit.Id);
            return dto;
        }

        #endregion OrganizationUnit private

        #region 初始化

        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly RoleManager _roleManager;

        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
          RoleManager roleManager,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _roleManager = roleManager;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
        }

        #endregion 初始化

        #region OrganizationUnit public

        public async Task<ListResultDto<OrganizationUnitListDto>> GetAllOrganizationUnitList()
        {
            var organizationUnits = await _organizationUnitRepository.GetAllListAsync();

            var ouMemberCounts = await _userOrganizationUnitRepository.GetAll()
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupedUsers => new
                {
                    organizationUnitId = groupedUsers.Key,
                    count = groupedUsers.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);

            return new ListResultDto<OrganizationUnitListDto>(
                           organizationUnits.Select(ou =>
                           {
                               var organizationUnitDto = ObjectMapper.Map<OrganizationUnitListDto>(ou);
                               organizationUnitDto.MemberCount = ouMemberCounts.ContainsKey(ou.Id) ? ouMemberCounts[ou.Id] : 0;

                               return organizationUnitDto;
                           }).ToList());
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitListDto> Create(CreateOrganizationUnitInput input)
        {
            var organizationUnit = new OrganizationUnit(AbpSession.TenantId, input.DisplayName, input.ParentId);
            await _organizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnitListDto>(organizationUnit);
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task Delete(EntityDto<long> input)
        {
            await _organizationUnitManager.DeleteAsync(input.Id);
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitListDto> Update(UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);

            if (organizationUnit == null)
                throw new UserFriendlyException("指定用户不存在");

            organizationUnit.DisplayName = input.DisplayName;

            await _organizationUnitManager.UpdateAsync(organizationUnit);

            return await CreateOrganizationUnit(organizationUnit);
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitListDto> Move(MoveOrganizationUnitInput input)
        {
            await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return await CreateOrganizationUnit(
                await _organizationUnitRepository.GetAsync(input.Id)
            );
        }

        #endregion OrganizationUnit public

        #region Organization User public

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public async Task<PagedResultDto<OrganizationUnitUserListDto>> GetPagedOrganizationUnitUsers(
            GetOrganizationUnitUsersInput input)
        {
            try
            {
                var query = from uou in _userOrganizationUnitRepository.GetAll()
                            join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                            join user in UserManager.Users.WhereIf(!input.FilterText.IsNullOrWhiteSpace(), u => u.UserName.Contains(input.FilterText)) on uou.UserId equals user.Id
                            where uou.OrganizationUnitId == input.Id
                            select new { uou, user };

                var totalCount = await query.CountAsync();
                var items = await query.OrderBy(o => input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<OrganizationUnitUserListDto>(
                    totalCount,
                    items.Select(item =>
                    {
                        var dto = ObjectMapper.Map<OrganizationUnitUserListDto>(item.user);
                        dto.AddedTime = item.uou.CreationTime;
                        return dto;
                    }).ToList());
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public async Task AddUsers(UsersToOrganizationUnitInput input)
        {
            foreach (var uids in input.UserIds)
                await UserManager.AddToOrganizationUnitAsync(uids, input.OrganizationUnitId);
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public async Task RemoveUser(UserToOrganizationUnitInput input)
        {
            await UserManager.RemoveFromOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }

        public async Task<bool> IsInOrganizationUnit(UserToOrganizationUnitInput input)
        {
            return await UserManager.IsInOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        //[AbpAllowAnonymous]
        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            var userIdsInOrganizationUnit = _userOrganizationUnitRepository.GetAll()
                .Where(uou => uou.OrganizationUnitId == input.OrganizationUnitId)
                .Select(uou => uou.UserId);

            var query = UserManager.Users
                .Where(user => !userIdsInOrganizationUnit.Contains(user.Id))
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),
                    user =>
                        user.UserName.Contains(input.FilterText) ||
                        user.EmailAddress.Contains(input.FilterText)
                );

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.UserName)
                .PageBy(input)
                .ToListAsync();
            return new PagedResultDto<NameValueDto>(
                userCount,
                users.Select(user =>
                    new NameValueDto(
                        $"{user.UserName} ({user.EmailAddress})",
                        user.Id.ToString()
                    )
                ).ToList()
            );
        }

        /// <summary>
        /// 批量从组织中移除用户
        /// </summary>
        /// <param name="userIds"> 用户Id列表 </param>
        /// <param name="organizationUnitId"> 组织机构Id </param>
        /// <returns> </returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageUsers)]
        public async Task BatchRemoveUserFromOrganizationUnit(List<long> userIds, long organizationUnitId)
        {
            foreach (var userId in userIds)
            {
                await UserManager.RemoveFromOrganizationUnitAsync(userId,organizationUnitId);

            }


      
        }

        #endregion Organization User public

        #region Organization Role public

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task<PagedResultDto<OrganizationUnitRoleListDto>> GetPagedOrganizationUnitRolesAsync(GetOrganizationUnitRolesInput input)
        {
            var query = from ouRole in _organizationUnitRoleRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on ouRole.OrganizationUnitId equals ou.Id
                        join role in _roleManager.Roles.WhereIf(!input.FilterText.IsNullOrWhiteSpace(), r => r.DisplayName.Contains(input.FilterText)) on ouRole.RoleId equals role.Id
                        where ouRole.OrganizationUnitId == input.Id
                        select new
                        {
                            ouRole,
                            role
                        };

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<OrganizationUnitRoleListDto>(
                totalCount,
                items.Select(item =>
                {
                    var organizationUnitRoleDto = ObjectMapper.Map<OrganizationUnitRoleListDto>(item.role);
                    organizationUnitRoleDto.AddedTime = item.ouRole.CreationTime;
                    return organizationUnitRoleDto;
                }).ToList());
        }

        /// <summary>
        /// 添加角色 组织关联
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task AddRoles(RolesToOrganizationUnitInput input)
        {
            foreach (var rids in input.RoleIds)
                await _roleManager.AddToOrganizationUnitAsync(rids, input.OrganizationUnitId, AbpSession.TenantId);
        }

        /// <summary>
        /// 删除 角色 组织关联
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task RemoveRole(RoleToOrganizationUnitInput input)
        {
            await _roleManager.RemoveFromOrganizationUnitAsync(input.RoleId, input.OrganizationUnitId);
        }

        /// <summary>
        /// 判断角色是否在当前组织
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task<bool> RolesIsInOrganizationUnit(RoleToOrganizationUnitInput input)
        {
            return await _roleManager.IsInOrganizationUnitAsync(input.RoleId, input.OrganizationUnitId);
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task<PagedResultDto<NameValueDto>> FindRoles(FindUsersInput input)
        {
            var roleIdsInOrganizationUnit = _organizationUnitRoleRepository.GetAll()
                .Where(uou => uou.OrganizationUnitId == input.OrganizationUnitId)
                .Select(uou => uou.RoleId);

            var query = _roleManager.Roles
                .Where(role => !roleIdsInOrganizationUnit.Contains(role.Id))
                .WhereIf(
                    !input.FilterText.IsNullOrWhiteSpace(),
                    role =>
                        role.DisplayName.Contains(input.FilterText)
                );

            var userCount = await query.CountAsync();
            var users = await query
                .OrderBy(r => r.DisplayName)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<NameValueDto>(
                userCount,
                users.Select(role =>
                    new NameValueDto(
                        $"{role.DisplayName}",
                        role.Id.ToString()
                    )
                ).ToList()
            );
        }

        /// <summary>
        /// 批量从组织中移除角色
        /// </summary>
        /// <param name="roleIds"> </param>
        /// <param name="organizationUnitId"> </param>
        /// <returns> </returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task BatchRemoveRoleFromOrganizationUnit(List<int> roleIds, long organizationUnitId)
        {

            foreach (var rId in roleIds)
            {
                await _roleManager.RemoveFromOrganizationUnitAsync(rId,organizationUnitId);

                
            }



             
        }

        #endregion Organization Role public
    }
}
