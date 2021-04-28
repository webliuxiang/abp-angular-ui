using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Identity;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        private readonly UserClaimsPrincipalFactory _claimsPrincipalFactory;

        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            IPasswordHasher<User> passwordHasher,
            RoleManager roleManager,
            UserClaimsPrincipalFactory claimsPrincipalFactory)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  passwordHasher,
                  roleManager,
                  claimsPrincipalFactory)
        {
            _claimsPrincipalFactory = claimsPrincipalFactory;
        }

        /// <summary>
        /// 直接根据用户信息登陆
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>用户身份信息</returns>
        public async Task<ClaimsIdentity> LoginAsync(User user)
        {
            ClaimsIdentity identity = (ClaimsIdentity)(await _claimsPrincipalFactory.CreateAsync(user)).Identity;
            return identity;
        }




    }
}
