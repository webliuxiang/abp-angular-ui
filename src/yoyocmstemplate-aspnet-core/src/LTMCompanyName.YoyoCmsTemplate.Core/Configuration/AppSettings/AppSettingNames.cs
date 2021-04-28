namespace LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings
{
    /// <summary>
    /// 定义用于在应用程序中设置名称的字符串常量。
    /// See <see cref="AppSettingProvider"/> for setting definitions.
    /// </summary>
    public static class AppSettingNames
    {

        public static class System
        {
            /// <summary>
            /// 数据库连接字符串
            /// </summary>
            public static string ConnectionStrings_Default { get; }


            #region MultiTenancy

            /// <summary>
            /// 多租户是否启用
            /// </summary>
            public static string MultiTenancy_IsEnabled { get; }

            #endregion


            #region JWT

            /// <summary>
            /// 是否启用jwt
            /// </summary>
            public static string Authentication_JwtBearer_IsEnabled { get; }
            /// <summary>
            /// jwt SecurityKey
            /// </summary>
            public static string Authentication_JwtBearer_SecurityKey { get; }
            /// <summary>
            /// jwt Issuer
            /// </summary>
            public static string Authentication_JwtBearer_Issuer { get; }
            /// <summary>
            /// jwt Audience
            /// </summary>
            public static string Authentication_JwtBearer_Audience { get; }

            #endregion


            static System()
            {
                ConnectionStrings_Default = "Default";
                MultiTenancy_IsEnabled = "MultiTenancy:IsEnabled";
                Authentication_JwtBearer_IsEnabled = "Authentication:JwtBearer:IsEnabled";
                Authentication_JwtBearer_SecurityKey = "Authentication:JwtBearer:SecurityKey";
                Authentication_JwtBearer_Issuer = "Authentication:JwtBearer:Issuer";
                Authentication_JwtBearer_Audience = "Authentication:JwtBearer:Audience";
            }
        }

        /// <summary>
        /// 管理应用程序配置信息
        /// </summary>
        public static class HostSettings
        {
            /// <summary>
            /// 发票抬头
            /// </summary>
            public const string BillingLegalName = "App.Host.BillingLegalName";
            /// <summary>
            /// 发票地址
            /// </summary>
            public const string BillingAddress = "App.Host.BillingAddress";

            /// <summary>
            /// 启用注册租户
            /// </summary>
            public const string AllowSelfRegistration = "App.Host.AllowSelfRegistration";

            /// <summary>
            /// 启用新租户默认激活
            /// </summary>
            public const string IsNewRegisteredTenantActiveByDefault = "App.Host.IsNewRegisteredTenantActiveByDefault";

            /// <summary>
            /// 新租户默认版本
            /// </summary>
            public const string DefaultEdition = "App.Host.DefaultEdition";

            /// <summary>
            /// 启用租户注册验证码
            /// </summary>
            public const string UseCaptchaOnTenantRegistration = "App.Host.UseCaptchaOnTenantRegistration";

            /// <summary>
            /// 租户注册验证码类型 0:纯数字 1:纯字母 2:数字+字母  3:纯汉字
            /// </summary>
            public const string CaptchaOnTenantRegistrationType = "App.Host.CaptchaOnTenantRegistrationType";
            /// <summary>
            /// 租户注册验证码验证码长度
            /// </summary>
            public const string CaptchaOnTenantRegistrationLength = "App.Host.CaptchaOnTenantRegistrationLength";


            /// <summary>
            /// 订阅过期通知日计数
            /// </summary>
            public const string SubscriptionExpireNotifyDayCount = "App.Host.SubscriptionExpireNotifyDayCount";

        }


        /// <summary>
        /// 租户管理
        /// </summary>
        public static class TenantSettings
        {
            /// <summary>
            /// 发票抬头
            /// </summary>
            public const string BillingLegalName = "App.Tenant.BillingLegalName";
            /// <summary>
            /// 发票地址
            /// </summary>
            public const string BillingAddress = "App.Tenant.BillingAddress";
            /// <summary>
            /// 
            /// </summary>
            public const string BillingTaxVatNo = "App.Tenant.BillingTaxVatNo";
            public const string IsNewRegisteredTenantActiveByDefault = "App.TenantManagement.IsNewRegisteredTenantActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.TenantManagement.UseCaptchaOnRegistration";
            public const string DefaultEdition = "App.TenantManagement.DefaultEdition";
            public const string SubscriptionExpireNotifyDayCount = "App.TenantManagement.SubscriptionExpireNotifyDayCount";




        }
        /// <summary>
        /// 用户设置管理
        /// </summary>
        public static class UserManagement
        {
            public static class TwoFactorLogin
            {
                public const string IsGoogleAuthenticatorEnabled = "App.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled";
            }

            public const string AllowSelfRegistration = "App.UserManagement.AllowSelfRegistration";
            public const string IsNewRegisteredUserActiveByDefault = "App.UserManagement.IsNewRegisteredUserActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.UserManagement.UseCaptchaOnRegistration";
            public const string SmsVerificationEnabled = "App.UserManagement.SmsVerificationEnabled";
            public const string IsCookieConsentEnabled = "App.UserManagement.IsCookieConsentEnabled";
            public const string IsQuickThemeSelectEnabled = "App.UserManagement.IsQuickThemeSelectEnabled";
            public const string ExternalLoginProviders = "App.UserManagement.ExternalLoginProviders";

        }



        /// <summary>
        /// setting scopes Application/Tenant
        /// </summary>
        public static class ApplicationAndTenant
        {
            /// <summary>
            /// 启用用户注册
            /// </summary>
            public const string AllowSelfRegistrationUser = "App.AllowSelfRegistrationUser";
            /// <summary>
            /// 启用新用户默认激活
            /// </summary>
            public const string IsNewRegisteredUserActiveByDefault = "App.IsNewRegisteredUserActiveByDefault";

            /// <summary>
            /// 启用用户注册验证码
            /// </summary>
            public const string UseCaptchaOnUserRegistration = "App.UseCaptchaOnUserRegistration";
            /// <summary>
            /// 用户注册验证码类型 0:纯数字 1:纯字母 2:数字+字母  3:纯汉字
            /// </summary>
            public const string CaptchaOnUserRegistrationType = "App.CaptchaOnUserRegistrationType";
            /// <summary>
            /// 用户注册验证码长度
            /// </summary>
            public const string CaptchaOnUserRegistrationLength = "App.CaptchaOnUserRegistrationLength";

            /// <summary>
            /// 启用用户登陆验证码
            /// </summary>
            public const string UseCaptchaOnUserLogin = "App.UseCaptchaOnUserLogin";
            /// <summary>
            /// 用户登陆验证码类型 0:纯数字 1:纯字母 2:数字+字母  3:纯汉字
            /// </summary>
            public const string CaptchaOnUserLoginType = "App.CaptchaOnUserLoginType";
            /// <summary>
            /// 用户登陆验证码长度
            /// </summary>
            public const string CaptchaOnUserLoginLength = "App.CaptchaOnUserLoginLength";

            /// <summary>
            /// 布局
            /// </summary>
            public const string ThemeLayout = "App.Theme.Layout";

        }

        /// <summary>
        /// setting scopes all
        /// </summary>
        public static class Shared
        {


            /// <summary>
            /// 启用短信校验
            /// </summary>
            public const string SmsVerificationEnabled = "App.UserManagement.SmsVerificationEnabled";
        }
    }

}
