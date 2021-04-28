using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Security.Captcha;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Identity;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy
{
    public class TenantRegistrationAppService : YoyoCmsTemplateAppServiceBase, ITenantRegistrationAppService
    {
        private readonly EditionManager _editionManager;
        private readonly RoleManager _roleManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ICacheManager _cacheManager;

        public TenantRegistrationAppService(
            EditionManager editionManager,
            RoleManager roleManager,
            IAbpZeroDbMigrator abpZeroDbMigrator,
            IPasswordHasher<User> passwordHasher,
            ICacheManager cacheManager)
        {
            _editionManager = editionManager;

            _roleManager = roleManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _passwordHasher = passwordHasher;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 注册租户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RegisterTenantResultDto> RegisterTenantAsync(CreateTenantDto input)
        {
            // 检查验证码
            await CaptchaHelper.CheckVerificationCode(
                this._cacheManager,
                this.SettingManager,
                CaptchaType.HostTenantRegister,
                input.TenancyName,
                input.VerificationCode);


            //创建租户信息
            var tenant = new Tenant(input.TenancyName, input.Name)
            {
                // 激活根据系统配置
                IsActive = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.HostSettings.AllowSelfRegistration)
            };

            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty() ? null : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            Abp.Application.Editions.Edition defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);

            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }
            await TenantManager.CreateAsync(tenant);
            // 保存以获取新租户的Id
            await CurrentUnitOfWork.SaveChangesAsync();
            // 创建租户数据库
            _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);
            //创建成功后，需要设置当前工作单元为当前登录后的租户信息
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                // 给新租户创建角色
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));
                // 保存，获取角色id
                await CurrentUnitOfWork.SaveChangesAsync();
                // 分配权限
                Role adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                // 创建此租户的管理员用户
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress, input.UserName);


                // 如果没有提交密码,那么走的是默认密码 bb123456
                adminUser.Password = _passwordHasher.HashPassword(adminUser, input.TenantAdminPassword.IsNullOrWhiteSpace() ? User.DefaultPassword : input.TenantAdminPassword);
                CheckErrors(await UserManager.CreateAsync(adminUser));

                // 保存，获取角色id
                await CurrentUnitOfWork.SaveChangesAsync();

                // 授权
                CheckErrors(await UserManager.AddToRoleAsync(adminUser, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();


            }


            var resultDto = new RegisterTenantResultDto()
            {
                TenantId = tenant.Id,
                IsActive = tenant.IsActive,
                UseCaptchaOnUserLogin = await UseCaptchaOnLogin(tenant.Id)
            };

            return resultDto;

        }


        /// <summary>
        /// 登陆启用验证码
        /// </summary>
        /// <returns></returns>
        private async Task<bool> UseCaptchaOnLogin(int tenantId)
        {
            var captchaConfig = await SettingManager.GetCaptchaConfig(CaptchaType.TenantUserLogin, tenantId);
            return captchaConfig.Enabled;
        }
    }
}