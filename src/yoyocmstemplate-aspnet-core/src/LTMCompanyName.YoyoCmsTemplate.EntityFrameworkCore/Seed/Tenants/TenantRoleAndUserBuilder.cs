using System.Linq;
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

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly YoyoCmsTemplateDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(YoyoCmsTemplateDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();

            //CreateMemberRoles();
        }

        private void CreateRolesAndUsers()
        {
            // Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters()
                .FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles
                    .Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin)
                    {
                        IsStatic = true,
                        IsDefault = false
                    }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(
                    new AppProAuthorizationProvider(false),   
                    new WechatAppConfigAuthorizationProvider(false),
                    
                    new BloggingAuthorizationProvider(false),
                    new SysFileAuthorizationProvider(false),
                    new WebSiteSettingAuthorizationProvider(false),
                    new MoocAuthorizationProvider(false)
                )
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user

            var adminUser = _context.Users.IgnoreQueryFilters()
                .FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "ltm@ddxc.org");
                adminUser.Password =
                    new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions()))
                        .HashPassword(adminUser, User.DefaultPassword);
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;
                adminUser.NeedToChangeThePassword = false;

                var adminUserForHost = _context.Users.Add(adminUser).Entity;

                _context.UserAccounts.Add(new UserAccount
                {
                    TenantId = adminUserForHost.TenantId,
                    UserId = adminUserForHost.Id,
                    UserName = adminUserForHost.UserName,
                    EmailAddress = adminUserForHost.EmailAddress
                });
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
        }





    }
}
