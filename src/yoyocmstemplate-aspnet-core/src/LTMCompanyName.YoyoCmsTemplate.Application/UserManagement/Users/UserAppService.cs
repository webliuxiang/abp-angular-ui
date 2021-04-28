using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Exporting;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users
{
    /// <summary>
    /// 用户信息服务实现
    /// </summary>
    [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users)]
    public class UserAppService : YoyoCmsTemplateAppServiceBase, IUserAppService
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        private readonly IPasswordHasher<User> _passwordHasher;

        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;

        private readonly RoleManager _roleManager;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IUserListExcelExporter _userListExcelExporter;




        /// <summary>
        /// 构造方法
        /// </summary>
        public UserAppService(IRepository<User, long> userRepository,
            IRepository<RolePermissionSetting, long> rolePermissionRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher,

            IEnumerable<IPasswordValidator<User>> passwordValidators, IUserListExcelExporter userListExcelExporter)
        {
            _userRepository = userRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _organizationUnitRepository = organizationUnitRepository;

            _roleManager = roleManager;

            _passwordHasher = passwordHasher;

            _passwordValidators = passwordValidators;
            _userListExcelExporter = userListExcelExporter;
        }

        public async Task CreateOrUpdate(CreateOrUpdateUserInput input)
        {
            if (input.User.Id.HasValue)
                await UpdateUserAsync(input);
            else
                await CreateUserAsync(input);
        }

        /// <summary>
        /// 分页获取所有用户
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users)]
        public async Task<PagedResultDto<UserListDto>> GetPaged(GetUsersInput input)
        {
            var query = UserManager.Users
                .WhereIf(input.Role != null && input.Role.Count > 0,
                    u => u.Roles.Any(r => input.Role.Contains(r.RoleId)))
                .WhereIf(input.IsEmailConfirmed.HasValue, u => u.IsEmailConfirmed == input.IsEmailConfirmed)
                .WhereIf(input.IsActive.HasValue, u => u.IsActive == input.IsActive)
                .WhereIf(input.OnlyLockedUsers, u => u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow)
                .WhereIf(
                    !input.FilterText.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(input.FilterText) ||
                        u.EmailAddress.Contains(input.FilterText)
                );

            if (input.Permission != null && input.Permission.Count > 0)
                query = from user in query
                        join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                        from ur in urJoined.DefaultIfEmpty()
                        join up in _userPermissionRepository.GetAll() on new { UserId = user.Id } equals new { up.UserId } into
                            upJoined
                        from up in upJoined.DefaultIfEmpty()
                        join rp in _rolePermissionRepository.GetAll() on new { ur.RoleId } equals new { rp.RoleId } into
                            rpJoined
                        from rp in rpJoined.DefaultIfEmpty()
                        where (up != null && up.IsGranted || up == null && rp != null) &&
                              (input.Permission.Contains(up.Name) || input.Permission.Contains(rp.Name))
                        group user by user
                    into userGrouped
                        select userGrouped.Key;

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();
            try
            {
                var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);

                await FillRoleNamesAsync(userListDtos);

                return new PagedResultDto<UserListDto>(
                    userCount,
                    userListDtos
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 用户的权限编辑
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_ChangePermissions)]
        public async Task<GetUserPermissionsTreeForEditOutput> GetPermissionsTreeForEdit(EntityDto<long> input)
        {
            var permissions = PermissionManager.GetAllPermissions();

            var user = await UserManager.GetUserByIdAsync(input.Id);
            var grantedPermissions = await UserManager.GetGrantedPermissionsAsync(user);

            return new GetUserPermissionsTreeForEditOutput
            {
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName)
                    .ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_ChangePermissions)]
        public async Task UpdatePermissions(UpdateUserPermissionsInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var grantedPermissions =
                PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await UserManager.SetGrantedPermissionsAsync(user, grantedPermissions);
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            if (input.Id == AbpSession.GetUserId()) throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));

            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_ChangePermissions)]
        public async Task ResetSpecificPermissions(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            await UserManager.ResetAllPermissionsAsync(user);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_Create,
            YoyoSoftPermissionNames.Pages_Administration_Users_Edit)]
        public async Task<GetUserForEditTreeOutput> GetForEditTree(NullableIdDto<long> input)
        {
            //Getting all available roles
            var userRoleDtos = await _roleManager.Roles
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToListAsync();

            var allOrganizationUnits = await _organizationUnitRepository.GetAll().ToListAsync();

            var output = new GetUserForEditTreeOutput
            {
                Roles = userRoleDtos,
                AllOrganizationUnits = ObjectMapper.Map<List<OrganizationUnitListDto>>(allOrganizationUnits),
                MemberedOrganizationUnits = new List<string>()
            };

            if (!input.Id.HasValue)
            {
                //Creating a new user
                output.User = new UserEditDto
                {
                    IsActive = true,
                    IsLockoutEnabled = true
                };

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null) defaultUserRole.IsAssigned = true;
                }
            }
            else
            {
                //Editing an existing user
                var user = await UserManager.GetUserByIdAsync(input.Id.Value);

                output.User = ObjectMapper.Map<UserEditDto>(user);

                foreach (var userRoleDto in userRoleDtos)
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(user, userRoleDto.RoleName);

                var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);
                output.MemberedOrganizationUnits = organizationUnits.Select(a => a.Code).ToList();
            }

            return output;
        }

        public async Task Unlock(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            user.Unlock();
        }

        /// <summary>
        /// 获取用户导出信息
        /// </summary>
        /// <returns> </returns>
        public async Task<FileDto> GetUsersToExcel()
        {
            var users = await UserManager.Users.ToListAsync();

            var dtos = ObjectMapper.Map<List<UserListDto>>(users);

            var file = _userListExcelExporter.ExportToExcel(dtos);
            return file;
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_Create, YoyoSoftPermissionNames.Pages_Administration_Users_Edit)]
        public async Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input)
        {
            //Getting all available roles
            var userRoleDtos = await _roleManager.Roles
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToArrayAsync();

            var allOrganizationUnits = await _organizationUnitRepository.GetAllListAsync();

            var output = new GetUserForEditOutput
            {
                Roles = userRoleDtos,
                AllOrganizationUnits = ObjectMapper.Map<List<OrganizationUnitListDto>>(allOrganizationUnits),
                MemberedOrganizationUnits = new List<string>()
            };

            if (!input.Id.HasValue)
            {
                //Creating a new user
                output.User = new UserEditDto
                {
                    IsActive = true,
                    NeedToChangeThePassword = true,
                    IsTwoFactorEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                    IsLockoutEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled)
                };

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null)
                    {
                        defaultUserRole.IsAssigned = true;
                    }
                }
            }
            else
            {
                //Editing an existing user
                var user = await UserManager.GetUserByIdAsync(input.Id.Value);

                output.User = ObjectMapper.Map<UserEditDto>(user);
                output.ProfilePictureId = user.ProfilePictureId;

                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(user, userRoleDto.RoleName);
                }

                var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);
                output.MemberedOrganizationUnits = organizationUnits.Select(ou => ou.Code).ToList();
            }

            return output;
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_ChangePermissions)]
        public async Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = await UserManager.GetGrantedPermissionsAsync(user);

            return new GetUserPermissionsForEditOutput
            {
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        public async Task<string> ResetPassword(NullableIdDto<long> input)
        {
            var user = await _userRepository.GetAsync(input.Id.Value);
            if (user == null)
                throw new UserFriendlyException("用户异常");

            user.Password = new PasswordHasher<User>().HashPassword(user, User.DefaultPassword);
            return User.DefaultPassword;
        }

        private async Task FillRoleNames(List<UserListDto> userListDtos)
        {
            /* This method is optimized to fill role names to given list. */

            var distinctRoleIds = (
                from userListDto in userListDtos
                from userListRoleDto in userListDto.Roles
                select userListRoleDto.RoleId
            ).Distinct();

            var roleNames = new Dictionary<int, string>();
            foreach (var roleId in distinctRoleIds)
                roleNames[roleId] = (await _roleManager.GetRoleByIdAsync(roleId)).DisplayName;

            foreach (var userListDto in userListDtos)
            {
                foreach (var userListRoleDto in userListDto.Roles)
                    userListRoleDto.RoleName = roleNames[userListRoleDto.RoleId];

                userListDto.Roles = userListDto.Roles.OrderBy(r => r.RoleName).ToList();
            }
        }

        private async Task FillRoleNamesAsync(List<UserListDto> userListDtos)
        {
            var userIds = userListDtos.Select(a => a.Id);

            var userRoles = await _userRoleRepository.GetAll()
                .Where(userRole => userIds.Contains(userRole.UserId))
                .Select(userRole => userRole).ToListAsync();

            var distinctRoleIds = userRoles.Select(userRole => userRole.RoleId).Distinct();

            foreach (var user in userListDtos)
            {
                var rolesOfUser = userRoles.Where(userRole => userRole.UserId == user.Id).ToList();

                user.Roles = ObjectMapper.Map<List<UserListRoleDto>>(rolesOfUser);
            }

            var roleNames = new Dictionary<int, string>();
            foreach (var roleId in distinctRoleIds)
                roleNames[roleId] = (await _roleManager.GetRoleByIdAsync(roleId)).DisplayName;

            foreach (var userListDto in userListDtos)
            {
                foreach (var userListRoleDto in userListDto.Roles)
                    userListRoleDto.RoleName = roleNames[userListRoleDto.RoleId];

                userListDto.Roles = userListDto.Roles.OrderBy(r => r.RoleName).ToList();
            }
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

            var user = await UserManager.FindByIdAsync(input.User.Id.Value.ToString());
           

            // 更新相关属性 密码不会进行处理，会在下面进行处理
            ObjectMapper.Map(input.User, user);

            if (input.SetRandomPassword) input.User.Password = User.CreateRandomPassword();

            if (!input.User.Password.IsNullOrEmpty())
            {
                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.User.Password));
            }
            // 如果用户被激活 就 默认跳过邮箱激活验证
            if (user.IsActive)
            {
                user.IsEmailConfirmed = true;
            }

            //更新用户基本信息
            CheckErrors(await UserManager.UpdateAsync(user));

            // 更新用户拥有角色
            CheckErrors(await UserManager.SetRolesAsync(user, input.AssignedRoleNames));

            // 更新所属组织机构
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            //todo:是否发送激活邮件功能。
            if (input.SendActivationEmail)
            {
                //user.SetNewEmailConfirmationCode();
                //await _userEmailer.SendEmailActivationLinkAsync(
                //    user,
                //    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                //    input.User.Password
                //);
            }
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_Create)]
        protected virtual async Task CreateUserAsync(CreateOrUpdateUserInput input)
        {
            var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
            user.TenantId = AbpSession.TenantId;

            //Set password
            if (input.SetRandomPassword || input.User.Password.IsNullOrEmpty())
            {
                input.User.Password = User.CreateRandomPassword();
            }
            else
            {
                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                foreach (var validator in _passwordValidators)
                    CheckErrors(await validator.ValidateAsync(UserManager, user, input.User.Password));
            }

            user.Password = _passwordHasher.HashPassword(user, input.User.Password);
            user.NeedToChangeThePassword = input.User.NeedToChangeThePassword;
            // 如果用户被激活 就 默认跳过邮箱激活验证
            if (user.IsActive)
            {
                user.IsEmailConfirmed = true;
            }


            // 设置角色
            user.Roles = new List<UserRole>();
            foreach (var roleName in input.AssignedRoleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
            }

            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

            //todo: 通过通知服务，通知相关业务方信息

            // 设置组织机构
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            //todo:是否发送激活邮件功能。
            if (input.SendActivationEmail)
            {
                //user.SetNewEmailConfirmationCode();
                //await _userEmailer.SendEmailActivationLinkAsync(
                //    user,
                //    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                //    input.User.Password
                //);
            }
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids"> 用户Id列表 </param>
        /// <returns> </returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_BatchDelete)]
        public async Task BatchDelete(List<long> ids)
        {
            foreach (var id in ids)
            {
                if (id == AbpSession.GetUserId()) continue;
                var user = await UserManager.GetUserByIdAsync(id);
                CheckErrors(await UserManager.DeleteAsync(user));
            }
        }
    }
}
