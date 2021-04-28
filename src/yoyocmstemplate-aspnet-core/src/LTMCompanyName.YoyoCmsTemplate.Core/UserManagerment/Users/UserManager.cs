using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.UI;
using Abp.Zero.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Security.PasswordComplexity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly ISettingManager _settingManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserManager(RoleManager roleManager,
            UserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ISettingManager settingManager,
            ILocalizationManager localizationManager)
            : base(
                roleManager,
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger,
                permissionManager,
                unitOfWorkManager,
                cacheManager,
                organizationUnitRepository,
                userOrganizationUnitRepository,
                organizationUnitSettings,
                settingManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
            _localizationManager = localizationManager;

        }

        /// <summary>
        ///     检查是否有重复的用户名和邮箱
        /// </summary>
        /// <param name="expectedUserId">当前已经存在的UserId</param>
        /// <param name="userName"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId,
            string userName, string emailAddress)
        {
            var user = await Users
                .FirstOrDefaultAsync(o => o.UserName == userName || o.EmailAddress == emailAddress);

            if (user != null && user.UserName == userName && user.Id != expectedUserId)
            {
                throw new UserFriendlyException(L("UserAlreadyExists"),
                    string.Format(L("UserAlreadyExists_Msg"), userName));
            }

            if (user != null && user.EmailAddress == emailAddress && user.Id != expectedUserId)
            {
                throw new UserFriendlyException(L("EmailAlreadyExists"),
                    string.Format(L("EmailAlreadyExists_Msg"), user.EmailAddress));
            }

            return IdentityResult.Success;
        }

        /// <summary>
        ///     获取用户user实体
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new Exception("There is no user: " + userIdentifier);
            }


            return user;
        }


        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }


        private new string L(string name)
        {
            return LocalizationManager.GetString(AppConsts.LocalizationSourceName, name);
        }


        /// <summary>
        ///     添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> CreateAsync(User user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            // TODO:不清空邮箱
            //user.EmailAddress = string.Empty;

            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !user.TenantId.HasValue)
            {
                user.TenantId = tenantId.Value;
            }

            try
            {
                return await base.CreateAsync(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [UnitOfWork]
        public virtual async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                return await FindByIdAsync(userIdentifier.UserId.ToString());
            }
        }

        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }

        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }

        /// <summary>
        ///     获取用户的邮箱信息
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string emailAddress)
        {
            var user = await Users.Where(o => o.EmailAddress == emailAddress)
                .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        ///     验证邮箱验证码是否正确
        /// </summary>
        /// <param name="emailConfirmationCode"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmailConfirmationCode(string emailConfirmationCode)
        {
            var user = await Users.Where(o => o.EmailConfirmationCode == emailConfirmationCode)
                .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        ///     修改密码
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="tenantId">租户Id</param>
        /// <returns></returns>
        public virtual Task<IdentityResult> ChangePasswordAsync(User user, string newPassword, int? tenantId)
        {
            base.InitializeOptionsAsync(tenantId);
            return base.ChangePasswordAsync(user, newPassword);
        }
        /// <summary>
        ///     获取重置密码的代码
        /// </summary>
        /// <param name="resetPasswrdCode"></param>
        /// <returns></returns>
        public async Task<User> GetUserByResetPasswordCode(string resetPasswrdCode)
        {
            var user = await Users.Where(o => o.PasswordResetCode == resetPasswrdCode)
                .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        ///     创建随机密码
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateRandomPassword()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireDigit),
                RequireLowercase =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireUppercase),
                RequiredLength =
                    await _settingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequiredLength)
            };

            var upperCaseLetters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            var lowerCaseLetters = "abcdefghijkmnopqrstuvwxyz";
            var digits = "0123456789";
            var nonAlphanumerics = "!@$?_-";

            string[] randomChars = { upperCaseLetters, lowerCaseLetters, digits, nonAlphanumerics };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (passwordComplexitySetting.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    upperCaseLetters[rand.Next(0, upperCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    lowerCaseLetters[rand.Next(0, lowerCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    digits[rand.Next(0, digits.Length)]);
            }

            if (passwordComplexitySetting.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    nonAlphanumerics[rand.Next(0, nonAlphanumerics.Length)]);
            }

            for (var i = chars.Count; i < passwordComplexitySetting.RequiredLength; i++)
            {
                var rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());



        }



        /// <summary>
        /// 获取管理员的信息
        /// </summary>
        /// <returns></returns>

        public async Task<User> GetAdminAsync()
        {

            var user = await FindByNameAsync(AbpUserBase.AdminUserName);

            return user;
        }

        /// <summary>
        ///是否具有管理者权限，否则就是教师
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsHaveAdminRole(long userId)
        {
            var role = await RoleManager.GetRoleByNameAsync(AppConsts.AdminRoleName);
            if (role != null)
            {
                var isHaveAdminRole = Users.Where(e => e.Id == userId).Include(e => e.Roles).Any(e => e.Roles.Any(b => b.RoleId == role.Id));
                return isHaveAdminRole;
            }
            return false;
        }

    }
}
