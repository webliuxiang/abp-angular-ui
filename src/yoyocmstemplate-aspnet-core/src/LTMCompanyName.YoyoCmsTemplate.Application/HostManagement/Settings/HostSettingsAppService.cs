using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero.Configuration;
using L._52ABP.Core.VerificationCodeStore;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Security.PasswordComplexity;
using LTMCompanyName.YoyoCmsTemplate.Timing;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings
{
    /// <summary>
    ///     宿主设置管理
    /// </summary>
    [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Host_Settings)]
    public class HostSettingsAppService : SettingsAppServiceBase, IHostSettingsAppService
    {
        private readonly EditionManager _editionManager;
        private readonly ITimeZoneService _timeZoneService;
        readonly ISettingDefinitionManager _settingDefinitionManager;

        public HostSettingsAppService(
            IEmailSender emailSender,
            EditionManager editionManager,
            ITimeZoneService timeZoneService,
            ISettingDefinitionManager settingDefinitionManager) : base(emailSender)
        {
            _editionManager = editionManager;
            _timeZoneService = timeZoneService;
            _settingDefinitionManager = settingDefinitionManager;
        }

        #region 获取设置

        public async Task<HostSettingsEditDto> GetAllSettings()
        {
            return new HostSettingsEditDto
            {
                General = await GetGeneralSettingsAsync(),
                TenantManagement = await GetTenantManagementSettingsAsync(),
                UserManagement = await GetUserManagementAsync(),
                Email = await GetEmailSettingsAsync(),
                Security = await GetSecuritySettingsAsync(),
                Billing = await GetBillingSettingsAsync(),
                Theme = await GetHostThemeSettingsAsync()
            };
        }

        private async Task<ThemeEditDto> GetHostThemeSettingsAsync()
        {
            var hostTheme = new ThemeEditDto();

            hostTheme.Layout = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ApplicationAndTenant.ThemeLayout);

            return hostTheme;
        }

        private async Task<GeneralSettingsEditDto> GetGeneralSettingsAsync()
        {
            var timezone = await SettingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);
            var settings = new GeneralSettingsEditDto
            {
                Timezone = timezone,
                TimezoneForComparison = timezone
            };

            var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
            if (settings.Timezone == defaultTimeZoneId)
            {
                settings.Timezone = string.Empty;
            }

            return settings;
        }

        private async Task<TenantManagementSettingsEditDto> GetTenantManagementSettingsAsync()
        {
            var settings = new TenantManagementSettingsEditDto
            {
                // 宿主注册租户
                AllowSelfRegistration = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.HostSettings.AllowSelfRegistration),
                IsNewRegisteredTenantActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.HostSettings.IsNewRegisteredTenantActiveByDefault),
                // 宿主租户注册验证码
                UseCaptchaOnTenantRegistration = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.HostSettings.UseCaptchaOnTenantRegistration),
                CaptchaOnTenantRegistrationType = (ValidateCodeType)await SettingManager.GetSettingValueAsync<int>(AppSettingNames.HostSettings.CaptchaOnTenantRegistrationType),
                CaptchaOnTenantRegistrationLength = await SettingManager.GetSettingValueAsync<int>(AppSettingNames.HostSettings.CaptchaOnTenantRegistrationLength),
            };

            var defaultEditionId = await SettingManager.GetSettingValueAsync(AppSettingNames.HostSettings.DefaultEdition);
            if (!string.IsNullOrEmpty(defaultEditionId) && (await _editionManager.FindByIdAsync(Convert.ToInt32(defaultEditionId)) != null))
            {
                settings.DefaultEditionId = Convert.ToInt32(defaultEditionId);
            }

            return settings;
        }

        private async Task<HostUserManagementSettingsEditDto> GetUserManagementAsync()
        {
            return new HostUserManagementSettingsEditDto
            {
                IsEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin),
                //SmsVerificationEnabled = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.UserManagement.SmsVerificationEnabled),
                // 宿主用户登陆验证码
                UseCaptchaOnUserLogin = await SettingManager.GetSettingValueAsync<bool>(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin),
                CaptchaOnUserLoginType = (ValidateCodeType)await SettingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType),
                CaptchaOnUserLoginLength = await SettingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength),
                ExternalLoginProviders = JsonConvert.DeserializeObject<List<string>>(SettingManager.GetSettingValue
                    (AppSettingNames.UserManagement.ExternalLoginProviders)),
            };
        }

        private async Task<EmailSettingsEditDto> GetEmailSettingsAsync()
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

        private async Task<SecuritySettingsEditDto> GetSecuritySettingsAsync()
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
                RequireDigit = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit).DefaultValue),
                RequireLowercase = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase).DefaultValue),
                RequireNonAlphanumeric = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric).DefaultValue),
                RequireUppercase = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase).DefaultValue),
                RequiredLength = Convert.ToInt32(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength).DefaultValue)
            };

            return new SecuritySettingsEditDto
            {
                UseDefaultPasswordComplexitySettings = passwordComplexitySetting.Equals(defaultPasswordComplexitySetting),
                PasswordComplexity = passwordComplexitySetting,
                DefaultPasswordComplexity = defaultPasswordComplexitySetting,
                UserLockOut = await GetUserLockOutSettingsAsync(),
                TwoFactorLogin = await GetTwoFactorLoginSettingsAsync()
            };
        }

        private async Task<HostBillingSettingsEditDto> GetBillingSettingsAsync()
        {
            return new HostBillingSettingsEditDto
            {
                LegalName = await SettingManager.GetSettingValueAsync(AppSettingNames.HostSettings.BillingLegalName),
                Address = await SettingManager.GetSettingValueAsync(AppSettingNames.HostSettings.BillingAddress)
            };
        }

        private async Task<UserLockOutSettingsEditDto> GetUserLockOutSettingsAsync()
        {
            return new UserLockOutSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled),
                MaxFailedAccessAttemptsBeforeLockout = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout),
                DefaultAccountLockoutSeconds = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds)
            };
        }

        private async Task<TwoFactorLoginSettingsEditDto> GetTwoFactorLoginSettingsAsync()
        {
            var twoFactorLoginSettingsEditDto = new TwoFactorLoginSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                IsEmailProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled),
                IsSmsProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled),
                IsRememberBrowserEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled),
            };
            return twoFactorLoginSettingsEditDto;
        }

        #endregion

        #region Update Settings

        public async Task UpdateAllSettings(HostSettingsEditDto input)
        {
            await UpdateGeneralSettingsAsync(input.General);
            await UpdateTenantManagementAsync(input.TenantManagement);
            await UpdateUserManagementSettingsAsync(input.UserManagement);
            await UpdateSecuritySettingsAsync(input.Security);
            await UpdateEmailSettingsAsync(input.Email);
            await UpdateBillingSettingsAsync(input.Billing);
            await UpdateThemeSettings(input.Theme);
        }

        private async Task UpdateBillingSettingsAsync(HostBillingSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.HostSettings.BillingLegalName, input.LegalName);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.HostSettings.BillingAddress, input.Address);

        }

        private async Task UpdateGeneralSettingsAsync(GeneralSettingsEditDto settings)
        {
            if (Clock.SupportsMultipleTimezone)
            {
                if (settings.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, settings.Timezone);
                }
            }
        }

        private async Task UpdateTenantManagementAsync(TenantManagementSettingsEditDto settings)
        {
            // 租户注册配置
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettingNames.HostSettings.AllowSelfRegistration,
                settings.AllowSelfRegistration.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettingNames.HostSettings.IsNewRegisteredTenantActiveByDefault,
                settings.IsNewRegisteredTenantActiveByDefault.ToString().ToLowerInvariant()
            );

            // 宿主租户注册验证码配置
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettingNames.HostSettings.UseCaptchaOnTenantRegistration,
                settings.UseCaptchaOnTenantRegistration.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettingNames.HostSettings.CaptchaOnTenantRegistrationType,
                ((int)settings.CaptchaOnTenantRegistrationType).ToString()
            );
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettingNames.HostSettings.CaptchaOnTenantRegistrationLength,
                settings.CaptchaOnTenantRegistrationLength.ToString()
            );

            // 创建租户的默认版本
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettingNames.HostSettings.DefaultEdition,
                settings.DefaultEditionId?.ToString() ?? ""
            );
        }

        private async Task UpdateUserManagementSettingsAsync(HostUserManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                settings.IsEmailConfirmationRequiredForLogin.ToString().ToLowerInvariant()
            );
            //await SettingManager.ChangeSettingForApplicationAsync(
            //    AppSettingNames.UserManagement.SmsVerificationEnabled,
            //    settings.SmsVerificationEnabled.ToString().ToLowerInvariant()
            //);
            // 宿主用户登陆验证码配置
            await SettingManager.ChangeSettingForApplicationAsync(
               AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin,
               settings.UseCaptchaOnUserLogin.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForApplicationAsync(
               AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType,
               ((int)settings.CaptchaOnUserLoginType).ToString()
            );
            await SettingManager.ChangeSettingForApplicationAsync(
              AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength,
              settings.CaptchaOnUserLoginLength.ToString()
            );
        }

        private async Task UpdateSecuritySettingsAsync(SecuritySettingsEditDto settings)
        {
            if (settings.UseDefaultPasswordComplexitySettings)
            {
                await UpdatePasswordComplexitySettingsAsync(settings.DefaultPasswordComplexity);
            }
            else
            {
                await UpdatePasswordComplexitySettingsAsync(settings.PasswordComplexity);
            }

            await UpdateUserLockOutSettingsAsync(settings.UserLockOut);
            await UpdateTwoFactorLoginSettingsAsync(settings.TwoFactorLogin);
        }

        private async Task UpdatePasswordComplexitySettingsAsync(PasswordComplexitySetting settings)
        {

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit,
                settings.RequireDigit.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase,
                settings.RequireLowercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric,
                settings.RequireNonAlphanumeric.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase,
                settings.RequireUppercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength,
                settings.RequiredLength.ToString()
            );
        }

        private async Task UpdateUserLockOutSettingsAsync(UserLockOutSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled, settings.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds, settings.DefaultAccountLockoutSeconds.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout, settings.MaxFailedAccessAttemptsBeforeLockout.ToString());
        }

        private async Task UpdateTwoFactorLoginSettingsAsync(TwoFactorLoginSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled, settings.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled, settings.IsEmailProviderEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled, settings.IsSmsProviderEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled, settings.IsRememberBrowserEnabled.ToString().ToLowerInvariant());
        }

        private async Task UpdateEmailSettingsAsync(EmailSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress, settings.DefaultFromAddress);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName, settings.DefaultFromDisplayName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, settings.SmtpHost);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port, settings.SmtpPort.ToString(CultureInfo.InvariantCulture));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName, settings.SmtpUserName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password, SimpleStringCipher.Instance.Encrypt(settings.SmtpPassword));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, settings.SmtpDomain);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl, settings.SmtpEnableSsl.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials, settings.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// 修改主题设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task UpdateThemeSettings(ThemeEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettingNames.ApplicationAndTenant.ThemeLayout,
                input.Layout
                );
        }

        #endregion
    }
}
