﻿using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Authorization;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Host
{
    public class HostRoleAndUserCreator
    {
        private readonly YoyoCmsTemplateDbContext _context;

        public HostRoleAndUserCreator(YoyoCmsTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
        }

        private void CreateHostRoleAndUsers()
        {
            // 创建宿主的管理员的角色
            var adminRoleForHost = _context.Roles.IgnoreQueryFilters()
                .FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin);

            if (adminRoleForHost == null)
            {
                adminRoleForHost = _context.Roles
                    .Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin)
                    {
                        IsStatic = true,
                        IsDefault = true
                    }).Entity;
                _context.SaveChanges();
            }

            // 给宿主的管理员赋予所有的权限

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == adminRoleForHost.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(
                    new AppProAuthorizationProvider(true),
                    new WechatAppConfigAuthorizationProvider(true),
                    new BloggingAuthorizationProvider(true),
                    new SysFileAuthorizationProvider(true),
                    new WebSiteSettingAuthorizationProvider(true),
                    new MoocAuthorizationProvider(true),
                    new ProjectAuthorizationProvider(true)
                )
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRoleForHost.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user for host

            var adminUserForHost = _context.Users.IgnoreQueryFilters()
                .FirstOrDefault(u => u.TenantId == null && u.UserName == AbpUserBase.AdminUserName);
            if (adminUserForHost == null)
            {
                var user = new User
                {
                    TenantId = null,
                    UserName = AbpUserBase.AdminUserName,
                    Name = "admin",
                    Surname = "admin",
                    EmailAddress = "ltm@ddxc.org",
                    IsEmailConfirmed = true,
                    NeedToChangeThePassword = false,
                    IsActive = true
                };

                user.Password =
                    new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions()))
                        .HashPassword(user, User.DefaultPassword);
                user.SetNormalizedNames();

                adminUserForHost = _context.Users.Add(user).Entity;

                _context.UserAccounts.Add(new UserAccount
                {
                    TenantId = adminUserForHost.TenantId,
                    UserId = adminUserForHost.Id,
                    UserName = adminUserForHost.UserName,
                    EmailAddress = adminUserForHost.EmailAddress
                });

                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));
                _context.SaveChanges();

                _context.SaveChanges();
            }
        }
    }
}
