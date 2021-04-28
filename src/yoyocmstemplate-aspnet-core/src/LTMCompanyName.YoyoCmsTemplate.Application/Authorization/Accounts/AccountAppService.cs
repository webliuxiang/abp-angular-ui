using System;
using System.Threading.Tasks;
using System.Web;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Impersonation;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Security.Captcha;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.UserEmail;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users.UserLink;
using Microsoft.AspNetCore.Identity;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts
{
    public class AccountAppService : YoyoCmsTemplateAppServiceBase, IAccountAppService
    {
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly ICacheManager _cacheManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IUserEmailer _userEmailer;
        private readonly UserManager _userManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager,
            ICacheManager cacheManager,
            UserManager userManager,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager, 
            IUserEmailer userEmailer
            )
        {
            _userRegistrationManager = userRegistrationManager;
            _cacheManager = cacheManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _userEmailer = userEmailer;
            _userManager = userManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {




            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null) return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);

            if (!tenant.IsActive) return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public Task<int?> ResolveTenantId(ResolveTenantIdInput input)
        {
            if (string.IsNullOrEmpty(input.c))
            {
                return Task.FromResult(AbpSession.TenantId);
            }

            var parameters = SimpleStringCipher.Instance.Decrypt(input.c);
            var query = HttpUtility.ParseQueryString(parameters);

            if (query["tenantId"] == null)
            {
                return Task.FromResult<int?>(null);
            }

            var tenantId = Convert.ToInt32(query["tenantId"]) as int?;
            return Task.FromResult(tenantId);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            // 检查验证码
            await CaptchaHelper.CheckVerificationCode(
                this._cacheManager,
                this.SettingManager,
                CaptchaType.TenantUserRegister,
                input.UserName,
                input.VerificationCode,
                AbpSession.TenantId);

            //// 判断如果是第三方注册进来
            //if (!string.IsNullOrEmpty(input.AuthProvider))
            //{
            //    // 判断该账号 已经存在过
            //    var userLogins = await _userManager.FindByLoginAsync(input.AuthProvider, input.ProviderKey);
            //    if (userLogins != null)
            //    {
            //        throw new UserFriendlyException($"该{input.AuthProvider} 已注册!");
            //    }
            //}

            var user = await _userRegistrationManager.RegisterAsync(
                input.UserName,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );
            if (!string.IsNullOrEmpty(input.AuthProvider)) {
                await _userManager.AddLoginAsync(user, new UserLoginInfo(input.AuthProvider, input.ProviderKey, input.AuthProvider));

            }


            var isEmailConfirmationRequiredForLogin =
                await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                    .IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                // 是否可以登陆 1、用户已激活 并且 2、用户邮箱已确认或未启用邮箱校验 并且 3、没有启用登陆验证码
                CanLogin = user.IsActive
                && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)

            };
        }


        /// <summary>
        /// 发送密码重置Code
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendPasswordResetCode(SendPasswordResetCodeInput input)
        {
            var user = await UserManager.FindByEmailAsync(input.EmailAddress);
            if (user == null) throw new UserFriendlyException(L("InvalidEmailAddress"));

            user.SetNewPasswordResetCode();
            user.Name = user.UserName + "假";
            user.Surname += "假";

            await _userEmailer.SendPasswordResetLinkAsync(
                user, input.link


            );

            // TODO: 发送短信验证码
        }

        /// <summary>
        ///  重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResetPasswordOutput> ResetPasswordAsync(ResetPasswordInput input)
        {

            var user = await UserManager.GetUserByResetPasswordCode(input.ResetCode);

            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != input.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));

            }


            CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;
            user.NeedToChangeThePassword = false;

            await UserManager.UpdateAsync(user);

            return new ResetPasswordOutput
            {
                CanLogin = user.IsActive,
                EmailAddress = user.EmailAddress
            };
        }




        public async Task SendEmailActivationLink(SendEmailActivationLinkInput input)
        {
            await Task.Yield();
            throw new NotImplementedException("SendEmailActivationLink 暂未实现");
        }

        public async Task ActivateEmail(ActivateEmailInput input)
        {
            await Task.Yield();
            throw new NotImplementedException("ActivateEmail 暂未实现");
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_Impersonation)]
        public async Task<ImpersonateOutput> Impersonate(ImpersonateInput input)
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, input.TenantId),
                TenancyName = await GetTenancyNameOrNull(input.TenantId)
            };
        }

        public async Task<ImpersonateOutput> BackToImpersonator()
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetBackToImpersonatorToken(),
                TenancyName = await GetTenancyNameOrNull(AbpSession.ImpersonatorTenantId)
            };
        }

        public async Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input)
        {
            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), input.ToUserIdentifier()))
            {
                throw new Exception(L("This account is not linked to your account"));
            }

            return new SwitchToLinkedAccountOutput
            {
                SwitchAccountToken = await _userLinkManager.GetAccountSwitchToken(input.TargetUserId, input.TargetTenantId),
                TenancyName = await GetTenancyNameOrNull(input.TargetTenantId)
            };
        }

        public async Task SendEmailAddressConfirmCode(SendPasswordResetCodeInput input)
        {
            var emailCache = _cacheManager.GetCache(input.EmailAddress);


 
            var cacheCode = await emailCache
                .AsTyped<string, string>()
                .GetOrDefaultAsync(input.EmailAddress);

            if (string.IsNullOrWhiteSpace(cacheCode))
            {
                var code = RandomHelper.GetRandom(10000, 99999).ToString();
                emailCache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(3);
                // 存值,过期时间3分钟
                await emailCache.SetAsync(input.EmailAddress, code.ToString());
                await _userEmailer.SendEmailAddressConfirmCode(input.EmailAddress, code.ToString());
            }
            else
            {
                throw new UserFriendlyException(222, "请检查邮箱中的验证码，有效期3分钟，可能进入垃圾箱了。");
            }
        }


        public async Task CheckEmailVerificationCode(GetEmailAddressCodeInput input)
        {
            var code = await _cacheManager
                .GetCache(input.EmailAddress)
                .AsTyped<string, string>()
                .GetOrDefaultAsync(input.EmailAddress);

            if (code == input.ConfirmationCode)
            {

            }
            else
            {
                throw new UserFriendlyException(222, "请检查邮箱中的验证码，有效期3分钟，可能进入垃圾箱了。");
            }

        }


        #region 第三方登录


        //public async Task ExternalLoginCallback(UserLoginInfo loginInfo) { 
        
        //}

        #endregion

       

        #region 私有函数

        private async Task<Tenant> GetActiveTenant(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        private async Task<string> GetTenancyNameOrNull(int? tenantId)
        {
            return tenantId.HasValue ? (await GetActiveTenant(tenantId.Value)).TenancyName : null;
        }

        private async Task<User> GetUserByChecking(string inputEmailAddress)
        {
            var user = await UserManager.FindByEmailAsync(inputEmailAddress);
            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidEmailAddress"));
            }

            return user;
        }

        #endregion
    }
}
