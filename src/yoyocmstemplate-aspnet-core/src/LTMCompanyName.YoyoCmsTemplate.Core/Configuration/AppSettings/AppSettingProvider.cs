using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using Abp.Zero.Configuration;
using Microsoft.Extensions.Configuration;
using Senparc.CO2NET.Extensions;

namespace LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings
{
    /// <summary>
    /// 应用程序的中定义的一些设置。
    /// 可以到 <see cref="AppSettingNames"/>查看这些名词.
    /// 给项目默认配置的启动信息
    /// </summary>
    public class AppSettingProvider : SettingProvider
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AppSettingProvider(IAppConfigurationAccessor configurationAccessor)
        {
            //  YoyoCmsTemplateConsts.MultiTenancyEnabled
            _appConfiguration = configurationAccessor.Configuration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            ////默认禁用TwoFactorLogin(可以通过UI启用)
            context.Manager.
                GetSettingDefinition(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled)
                .DefaultValue = false.ToString().ToLowerInvariant();

            return GetHostSettings().Union(GetTenantSettings()).Union(GetApplicationAndTenantSettings());
        }

        /// <summary>
        ///  Application的设定范围
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettingDefinition> GetHostSettings()
        {
            return new[]
            {
                // 宿主租户注册 配置
                new SettingDefinition(AppSettingNames.HostSettings.AllowSelfRegistration, GetFromAppSettings(AppSettingNames.HostSettings.AllowSelfRegistration, "true"), isVisibleToClients: true),

                new SettingDefinition(AppSettingNames.HostSettings.IsNewRegisteredTenantActiveByDefault, GetFromAppSettings(AppSettingNames.HostSettings.IsNewRegisteredTenantActiveByDefault, "false")),
                // 宿主租户注册默认版本配置
                new SettingDefinition(AppSettingNames.HostSettings.DefaultEdition, GetFromAppSettings(AppSettingNames.HostSettings.DefaultEdition, "")),
                // 是否启用短信验证码
                new SettingDefinition(AppSettingNames.UserManagement.SmsVerificationEnabled, GetFromAppSettings(AppSettingNames.UserManagement.SmsVerificationEnabled, "false"), isVisibleToClients: true),
                // 宿主租户注册验证码 配置
                new SettingDefinition(AppSettingNames.HostSettings.UseCaptchaOnTenantRegistration, GetFromAppSettings(AppSettingNames.HostSettings.UseCaptchaOnTenantRegistration, "false"), isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.HostSettings.CaptchaOnTenantRegistrationType, GetFromAppSettings(AppSettingNames.HostSettings.CaptchaOnTenantRegistrationType, "1"), isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.HostSettings.CaptchaOnTenantRegistrationLength, GetFromAppSettings(AppSettingNames.HostSettings.CaptchaOnTenantRegistrationLength, "4"), isVisibleToClients: true),
                // 宿主的租户订阅过期提醒配置
                new SettingDefinition(AppSettingNames.HostSettings.SubscriptionExpireNotifyDayCount, GetFromAppSettings(AppSettingNames.HostSettings.SubscriptionExpireNotifyDayCount, "7"), isVisibleToClients: true),
                // 宿主发票配置
                new SettingDefinition(AppSettingNames.HostSettings.BillingLegalName, GetFromAppSettings(AppSettingNames.HostSettings.BillingLegalName, "")),
                new SettingDefinition(AppSettingNames.HostSettings.BillingAddress, GetFromAppSettings(AppSettingNames.HostSettings.BillingAddress, "")),
                // 三方登陆配置
                new SettingDefinition(AppSettingNames.UserManagement.ExternalLoginProviders, GetFromAppSettings(AppSettingNames.UserManagement.ExternalLoginProviders, _appConfiguration.GetSection("Authentication:External").GetChildren().Where(x=>x.GetValue<bool>("IsEnabled")).Select(x=>x.Key).ToJson()), isVisibleToClients:true)
            };
        }

        /// <summary>
        /// setting scopes Tenant
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettingDefinition> GetTenantSettings()
        {
            var scopes = SettingScopes.Tenant;
            return new[]
            {
                //是否允许用户注册
                new SettingDefinition(AppSettingNames.UserManagement.AllowSelfRegistration, GetFromAppSettings(AppSettingNames.UserManagement.AllowSelfRegistration, "true"), scopes: scopes),
                // 租户发票 配置
                new SettingDefinition(AppSettingNames.TenantSettings.BillingLegalName, GetFromAppSettings(AppSettingNames.TenantSettings.BillingLegalName, ""), scopes: scopes),
                new SettingDefinition(AppSettingNames.TenantSettings.BillingAddress, GetFromAppSettings(AppSettingNames.TenantSettings.BillingAddress, ""), scopes: scopes),
                new SettingDefinition(AppSettingNames.TenantSettings.BillingTaxVatNo, GetFromAppSettings(AppSettingNames.TenantSettings.BillingTaxVatNo, ""), scopes: scopes),
            };
        }

        /// <summary>
        /// setting scopes Application/Tenant
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettingDefinition> GetApplicationAndTenantSettings()
        {
            var scopes = SettingScopes.Application | SettingScopes.Tenant;
            return new[]
            {
                // 用户注册 配置
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.AllowSelfRegistrationUser,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.AllowSelfRegistrationUser, "true"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.IsNewRegisteredUserActiveByDefault,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.IsNewRegisteredUserActiveByDefault, "false"),
                    scopes: scopes
                ),
                // 用户注册验证码 配置
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserRegistration,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserRegistration, "false"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationType,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationType, "1"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationLength,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationLength, "4"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                // 用户登陆验证码 配置
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin, "false"),
                    scopes: scopes, isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType, "1"),
                    scopes: scopes,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength,
                    GetFromAppSettings(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength, "4"),
                    scopes:scopes,
                    isVisibleToClients: true
                ),
                // 样式布局
                new SettingDefinition(
                    AppSettingNames.ApplicationAndTenant.ThemeLayout,
                    AppSettingValues.CommonSettings.Theme_Layout_Default,
                    scopes:scopes,
                    isVisibleToClients:true),
            };
        }

        /// <summary>
        /// setting scopes All
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettingDefinition> GetSharedSettings()
        {
            var scopes = SettingScopes.All;
            return new[]
            {
                // 短信配置
                new SettingDefinition(
                    AppSettingNames.Shared.SmsVerificationEnabled,
                    GetFromAppSettings(AppSettingNames.Shared.SmsVerificationEnabled, "false"),
                    scopes: scopes,
                    isVisibleToClients: false
                )
            };
        }

        private string GetFromAppSettings(string name, string defaultValue = null)
        {
            return GetFromSettings("App:" + name, defaultValue);
        }

        private string GetFromSettings(string name, string defaultValue = null)
        {
            return _appConfiguration[name] ?? defaultValue;
        }
    }
}
