using System.Globalization;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap.Configuration;
using L._52ABP.Core.VerificationCodeStore;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Security.PasswordComplexity;
using LTMCompanyName.YoyoCmsTemplate.Timing;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings
{
    [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Tenant_Settings)]
    public class TenantSettingsAppService : SettingsAppServiceBase, ITenantSettingsAppService
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IDataFileObjectManager _dataFileObjectManager;
        private readonly IAbpZeroLdapModuleConfig _ldapModuleConfig;

        public TenantSettingsAppService(
            IAbpZeroLdapModuleConfig ldapModuleConfig,
            IMultiTenancyConfig multiTenancyConfig,
            ITimeZoneService timeZoneService,
            IEmailSender emailSender,
            IDataFileObjectManager dataFileObjectManager) : base(emailSender)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _ldapModuleConfig = ldapModuleConfig;
            _timeZoneService = timeZoneService;
            _dataFileObjectManager = dataFileObjectManager;
        }

        #region 获取设置

        /// <summary>
        /// 获取所有设置
        /// </summary>
        /// <returns></returns>
        public async Task<TenantSettingsEditDto> GetAllSettings()
        {
            var settings = new TenantSettingsEditDto
            {
                UserManagement = await GetUserManagementSettings(),
                Security = await GetSecuritySettings(),
                Email = await GetEmailSettings(),
                Theme = await this.GetTeantThemeSettingsAsync()
                //Billing = await GetBillingSettingsAsync()
            };

            if (!_multiTenancyConfig.IsEnabled || Clock.SupportsMultipleTimezone)
            {
                settings.General = await GetGeneralSettings();
            }

            if (!_multiTenancyConfig.IsEnabled)
            {
                settings.Email = await GetEmailSettings();

                if (_ldapModuleConfig.IsEnabled)
                {
                    settings.Ldap = await GetLdapSettings();
                }
                else
                {
                    settings.Ldap = new LdapSettingsEditDto { IsModuleEnabled = false };
                }
            }

            return settings;
        }

        private async Task<ThemeEditDto> GetTeantThemeSettingsAsync()
        {
            var tenantTheme = new ThemeEditDto();

            tenantTheme.Layout = await SettingManager.GetSettingValueForTenantAsync(AppSettingNames.ApplicationAndTenant.ThemeLayout,AbpSession.TenantId.Value);

            return tenantTheme;

        }

        /// <summary>
        /// 获取Ldap设置
        /// </summary>
        /// <returns></returns>
        private async Task<LdapSettingsEditDto> GetLdapSettings()
        {
            return new LdapSettingsEditDto
            {
                IsModuleEnabled = true,
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(LdapSettingNames.IsEnabled),
                Domain = await SettingManager.GetSettingValueAsync(LdapSettingNames.Domain),
                UserName = await SettingManager.GetSettingValueAsync(LdapSettingNames.UserName),
                Password = await SettingManager.GetSettingValueAsync(LdapSettingNames.Password),
            };
        }

        /// <summary>
        /// 获取邮箱设置
        /// </summary>
        /// <returns></returns>
        private async Task<EmailSettingsEditDto> GetEmailSettings()
        {
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);

            return new EmailSettingsEditDto
            {
                DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
                SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port),
                SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName),
                SmtpPassword = SimpleStringCipher.Instance.Decrypt(smtpPassword),
                SmtpDomain = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl),
                SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials)
            };
        }

        /// <summary>
        /// 获取一般设置
        /// </summary>
        /// <returns></returns>
        private async Task<GeneralSettingsEditDto> GetGeneralSettings()
        {
            var settings = new GeneralSettingsEditDto();

            if (Clock.SupportsMultipleTimezone)
            {
                var timezone = await SettingManager.GetSettingValueForTenantAsync(TimingSettingNames.TimeZone, AbpSession.GetTenantId());

                settings.Timezone = timezone;
                settings.TimezoneForComparison = timezone;
            }

            var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Tenant, AbpSession.TenantId);

            if (settings.Timezone == defaultTimeZoneId)
            {
                settings.Timezone = string.Empty;
            }

            return settings;
        }

        /// <summary>
        /// 获取用户管理设置
        /// </summary>
        /// <returns></returns>
        private async Task<TenantUserManagementSettingsEditDto> GetUserManagementSettings()
        {
            return new TenantUserManagementSettingsEditDto
            {
                AllowSelfRegistration = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.ApplicationAndTenant.AllowSelfRegistrationUser),
                IsNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.ApplicationAndTenant.IsNewRegisteredUserActiveByDefault),
                IsEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin),
                // 用户注册验证码配置
                UseCaptchaOnUserRegistration = await SettingManager.GetSettingValueForTenantAsync<bool>(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserRegistration, AbpSession.TenantId.Value),
                CaptchaOnUserRegistrationType = (ValidateCodeType)await SettingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationType),
                CaptchaOnUserRegistrationLength = await SettingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationLength),
                // 用户登陆验证码配置
                UseCaptchaOnUserLogin = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin),
                CaptchaOnUserLoginType = (ValidateCodeType)await SettingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType),
                CaptchaOnUserLoginLength = await SettingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength),
            };
        }

        /// <summary>
        /// 获取安全设置
        /// </summary>
        /// <returns></returns>
        private async Task<SecuritySettingsEditDto> GetSecuritySettings()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit),
                RequireLowercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase),
                RequiredLength = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
            };

            var defaultPasswordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit),
                RequireLowercase = await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric = await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase = await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase),
                RequiredLength = await SettingManager.GetSettingValueForApplicationAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
            };

            return new SecuritySettingsEditDto
            {
                UseDefaultPasswordComplexitySettings = passwordComplexitySetting.Equals(defaultPasswordComplexitySetting),
                PasswordComplexity = passwordComplexitySetting,
                DefaultPasswordComplexity = defaultPasswordComplexitySetting,
                UserLockOut = await GetUserLockOutSettingsAsync(),
                TwoFactorLogin = await GetTwoFactorLoginSettings()
            };
        }

        //private async Task<TenantBillingSettingsEditDto> GetBillingSettingsAsync()
        //{
        //    return new TenantBillingSettingsEditDto()
        //    {
        //        LegalName = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.BillingLegalName),
        //        Address = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.BillingAddress),
        //        TaxVatNo = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.BillingTaxVatNo)
        //    };
        //}

        /// <summary>
        /// 获取用户锁定设置
        /// </summary>
        /// <returns></returns>
        private async Task<UserLockOutSettingsEditDto> GetUserLockOutSettingsAsync()
        {
            return new UserLockOutSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled),
                MaxFailedAccessAttemptsBeforeLockout = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout),
                DefaultAccountLockoutSeconds = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds)
            };
        }

        /// <summary>
        /// 是否启用登陆双重验证
        /// </summary>
        /// <returns></returns>
        private Task<bool> IsTwoFactorLoginEnabledForApplication()
        {
            return SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled);
        }

        /// <summary>
        /// 获取登陆双重验证设置
        /// </summary>
        /// <returns></returns>
        private async Task<TwoFactorLoginSettingsEditDto> GetTwoFactorLoginSettings()
        {
            var settings = new TwoFactorLoginSettingsEditDto
            {
                IsEnabledForApplication = await IsTwoFactorLoginEnabledForApplication()
            };

            if (_multiTenancyConfig.IsEnabled && !settings.IsEnabledForApplication)
            {
                return settings;
            }

            settings.IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled);
            settings.IsRememberBrowserEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled);

            if (!_multiTenancyConfig.IsEnabled)
            {
                settings.IsEmailProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled);
                settings.IsSmsProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled);
            }

            return settings;
        }

        #endregion

        #region 更新设置

        /// <summary>
        /// 更新所有设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAllSettings(TenantSettingsEditDto input)
        {
            await UpdateUserManagementSettings(input.UserManagement);
            await UpdateSecuritySettings(input.Security);
            await UpdateThemeSettings(input.Theme);
            //await UpdateBillingSettingsAsync(input.Billing);


            //Time Zone
            if (Clock.SupportsMultipleTimezone)
            {
                if (input.General.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Tenant, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), TimingSettingNames.TimeZone, input.General.Timezone);
                }
            }

            if (!_multiTenancyConfig.IsEnabled)
            {
                input.ValidateHostSettings();

                await UpdateEmailSettingsAsync(input.Email);

                if (_ldapModuleConfig.IsEnabled)
                {
                    await UpdateLdapSettingsAsync(input.Ldap);
                }
            }
        }

        private async Task UpdateBillingSettingsAsync(TenantBillingSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AppSettingNames.TenantSettings.BillingLegalName, input.LegalName);
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AppSettingNames.TenantSettings.BillingAddress, input.Address);
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AppSettingNames.TenantSettings.BillingTaxVatNo, input.TaxVatNo);

        }

        private async Task UpdateLdapSettingsAsync(LdapSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), LdapSettingNames.IsEnabled, input.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), LdapSettingNames.Domain, input.Domain.IsNullOrWhiteSpace() ? null : input.Domain);
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), LdapSettingNames.UserName, input.UserName.IsNullOrWhiteSpace() ? null : input.UserName);
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), LdapSettingNames.Password, input.Password.IsNullOrWhiteSpace() ? null : input.Password);
        }

        private async Task UpdateEmailSettingsAsync(EmailSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.DefaultFromAddress,
                input.DefaultFromAddress
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.DefaultFromAddress,
                input.DefaultFromAddress
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.DefaultFromDisplayName,
                input.DefaultFromDisplayName
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.Smtp.Host,
                input.SmtpHost
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.Smtp.Port,
                input.SmtpPort.ToString(CultureInfo.InvariantCulture)
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.Smtp.UserName,
                input.SmtpUserName
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.Smtp.Password,
                SimpleStringCipher.Instance.Encrypt(input.SmtpPassword)
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.Smtp.Domain,
                input.SmtpDomain
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.Smtp.EnableSsl,
                input.SmtpEnableSsl.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                EmailSettingNames.Smtp.UseDefaultCredentials,
                input.SmtpUseDefaultCredentials.ToString().ToLowerInvariant()
            );
        }

        /// <summary>
        /// 更新用户管理设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateUserManagementSettings(TenantUserManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettingNames.ApplicationAndTenant.AllowSelfRegistrationUser,
                settings.AllowSelfRegistration.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettingNames.ApplicationAndTenant.IsNewRegisteredUserActiveByDefault,
                settings.IsNewRegisteredUserActiveByDefault.ToString().ToLowerInvariant()
            );


            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                settings.IsEmailConfirmationRequiredForLogin.ToString().ToLowerInvariant()
            );
            // 用户注册验证码配置
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserRegistration,
                settings.UseCaptchaOnUserRegistration.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationType,
                ((int)settings.CaptchaOnUserRegistrationType).ToString()
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationLength,
                settings.CaptchaOnUserRegistrationLength.ToString()
            );

            // 用户登陆验证码配置
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin,
                settings.UseCaptchaOnUserLogin.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType,
                ((int)settings.CaptchaOnUserLoginType).ToString()
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength,
                settings.CaptchaOnUserLoginLength.ToString()
            );
        }

        /// <summary>
        /// 更新安全设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateSecuritySettings(SecuritySettingsEditDto settings)
        {
            if (settings.UseDefaultPasswordComplexitySettings)
            {
                await UpdatePasswordComplexitySettings(settings.DefaultPasswordComplexity);
            }
            else
            {
                await UpdatePasswordComplexitySettings(settings.PasswordComplexity);
            }

            await UpdateUserLockOutSettings(settings.UserLockOut);
            await UpdateTwoFactorLoginSettings(settings.TwoFactorLogin);
        }

        /// <summary>
        /// 更新密码校验设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdatePasswordComplexitySettings(PasswordComplexitySetting settings)
        {
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit,
                settings.RequireDigit.ToString()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase,
                settings.RequireLowercase.ToString()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric,
                settings.RequireNonAlphanumeric.ToString()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase,
                settings.RequireUppercase.ToString()
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength,
                settings.RequiredLength.ToString()
            );
        }

        /// <summary>
        /// 更新用户锁定设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateUserLockOutSettings(UserLockOutSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled, settings.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds, settings.DefaultAccountLockoutSeconds.ToString());
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout, settings.MaxFailedAccessAttemptsBeforeLockout.ToString());
        }

        /// <summary>
        /// 更新登陆双重校验设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateTwoFactorLoginSettings(TwoFactorLoginSettingsEditDto settings)
        {
            if (_multiTenancyConfig.IsEnabled &&
                !await IsTwoFactorLoginEnabledForApplication()) //Two factor login can not be used by tenants if disabled by the host
            {
                return;
            }

            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled, settings.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled, settings.IsRememberBrowserEnabled.ToString().ToLowerInvariant());

            if (!_multiTenancyConfig.IsEnabled)
            {
                //These settings can only be changed by host, in a multitenant application.
                await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled, settings.IsEmailProviderEnabled.ToString().ToLowerInvariant());
                await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled, settings.IsSmsProviderEnabled.ToString().ToLowerInvariant());
            }
        }

        /// <summary>
        /// 修改主题设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task UpdateThemeSettings(ThemeEditDto input)
        {
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(), 
                AppSettingNames.ApplicationAndTenant.ThemeLayout,
                input.Layout
                );
        }



        #endregion

        #region 其它

        public async Task ClearLogo()
        {
            var tenant = await GetCurrentTenantAsync();

            if (!tenant.HasLogo())
            {
                return;
            }

            var logoObject = await _dataFileObjectManager.GetOrNullAsync(tenant.LogoId.Value);
            if (logoObject != null)
            {
                await _dataFileObjectManager.DeleteAsync(tenant.LogoId.Value);
            }

            tenant.ClearLogo();
        }

        public async Task ClearCustomCss()
        {
            var tenant = await GetCurrentTenantAsync();

            if (!tenant.CustomCssId.HasValue)
            {
                return;
            }

            var cssObject = await _dataFileObjectManager.GetOrNullAsync(tenant.CustomCssId.Value);
            if (cssObject != null)
            {
                await _dataFileObjectManager.DeleteAsync(tenant.CustomCssId.Value);
            }

            tenant.CustomCssId = null;
        }

        #endregion
    }
}
