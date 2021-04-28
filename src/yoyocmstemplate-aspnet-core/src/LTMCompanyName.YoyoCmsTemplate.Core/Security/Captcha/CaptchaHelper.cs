using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;

namespace LTMCompanyName.YoyoCmsTemplate.Security.Captcha
{
    public static class CaptchaHelper
    {
        #region 所有的异常定义

        /// <summary>
        /// 验证码类型错误
        /// </summary>
        private static readonly UserFriendlyException s_captchaTypeErrorException = new UserFriendlyException("验证码类型错误!");
        /// <summary>
        /// 验证码输入错误
        /// </summary>
        private static readonly UserFriendlyException s_captchaInputNullException = new UserFriendlyException("请输入有效验证码!");
        /// <summary>
        /// 验证码过期错误
        /// </summary>
        private static readonly UserFriendlyException s_captchaExpireException = new UserFriendlyException("验证码已过期,请刷新验证码图片后重试!");
        /// <summary>
        /// 验证码不匹配错误
        /// </summary>
        private static readonly UserFriendlyException s_captchaNoMatchException = new UserFriendlyException("验证码错误,请刷新验证码图片后重试!");

        #endregion



        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="settingManager">设置管理器</param>
        /// <param name="captchaType">验证码类型</param>
        /// <param name="key">验证码缓存Key</param>
        /// <param name="verificationCode">输入的验证码</param>
        /// <param name="tenantId">租户id</param>
        /// <returns></returns>
        public static async Task CheckVerificationCode(this ICacheManager cacheManager,
            ISettingManager settingManager,
            CaptchaType captchaType,
            string key,
            string verificationCode,
            int? tenantId = null)
        {
            // 获取对应验证码配置
            var captchaConfig = await settingManager.GetCaptchaConfig(captchaType, tenantId);

            // 是否启用验证码
            if (captchaConfig.Enabled)
            {
                if (verificationCode.IsNullOrWhiteSpace())
                {
                    throw s_captchaInputNullException;
                }

                // 分租户获取验证码缓存
                var cacheKey = CaptchaHelper.CreateCacheKey(captchaType, tenantId);

                // 从缓存验证码值
                var cacheValue = await cacheManager.GetValue<string>(cacheKey, key, true);

                if (cacheValue.IsNullOrWhiteSpace())
                {
                    throw s_captchaExpireException;
                }

                // 对比验证码
                if (verificationCode.ToLower() != cacheValue)
                {
                    throw s_captchaNoMatchException;
                }
            }
        }


        /// <summary>
        /// 获取对应类型的验证码配置
        /// </summary>
        /// <param name="settingManager">设置管理器</param>
        /// <param name="type">验证码使用点</param>
        /// <param name="tenantId">租户Id</param>
        /// <returns>验证码配置</returns>
        public static async Task<CaptchaConfig> GetCaptchaConfig(this ISettingManager settingManager, CaptchaType type, int? tenantId = null)
        {
            CaptchaConfig captchaConfig = null;


            switch (type)
            {
                case CaptchaType.HostTenantRegister:// 宿主租户注册
                    captchaConfig = await GetHostTenantRegistrationCaptchaConfig(settingManager);
                    break;
                case CaptchaType.TenantUserRegister:// 租户用户注册
                    if (!tenantId.HasValue && tenantId.Value > 0) throw s_captchaTypeErrorException;
                    captchaConfig = await GetUserRegistrationCaptchaConfig(settingManager, tenantId);
                    break;
                case CaptchaType.HostUserLogin:// 宿主用户登陆
                case CaptchaType.TenantUserLogin:// 租户用户登陆
                    captchaConfig = await GetUserLoginCaptchaConfig(settingManager, tenantId);
                    break;
                case CaptchaType.Defulat:
                    break;
                case CaptchaType.TenantUserRegisterActiveEmail:
                    break;
                case CaptchaType.TenantUserForotPassword:
                    break;
                case CaptchaType.TenantUserResetPassword:
                    break;
                default:
                    throw s_captchaTypeErrorException;
            }

            return captchaConfig;
        }


        /// <summary>
        /// 创建验证码缓存Key
        /// </summary>
        /// <param name="type">验证码类型</param>
        /// <param name="tenantId">租户id(可空)</param>
        /// <returns>缓存Key</returns>
        public static string CreateCacheKey(CaptchaType type, int? tenantId = null)
        {
            var res = tenantId.HasValue ? $"T:{tenantId.Value}" : null;

            switch (type)
            {
                case CaptchaType.HostTenantRegister:
                    res = HostCacheKeys.TenantRegistrationCaptchaCache;
                    break;
                case CaptchaType.HostUserLogin:
                    res = HostCacheKeys.HostUserLoginCaptchaCache;
                    break;
                case CaptchaType.Defulat:
                    res = $"T:{res}-{HostCacheKeys.Default}";

                    break;
                case CaptchaType.TenantUserRegister:
                    res = $"T:{res}-{TenantCacheKeys.UserRegistrationCaptchaCache}";
                    break;
                case CaptchaType.TenantUserLogin:
                    res = $"T:{res}-{TenantCacheKeys.UserLoginCaptchaCache}";
                    break;

                case CaptchaType.TenantUserRegisterActiveEmail:
                    res = $"T:{res}-{TenantCacheKeys.TenantUserRegisterActiveEmail}";

                    break;
                case CaptchaType.TenantUserForotPassword:
                    res = $"T:{res}-{TenantCacheKeys.TenantUserForotPassword}";

                    break;
                case CaptchaType.TenantUserResetPassword:
                    res = $"T:{res}-{TenantCacheKeys.TenantUserResetPassword}";

                    break;
                default:
                    throw new UserFriendlyException("创建验证码CacheKey错误!验证码类型正确");
            }


            return res;
        }

        /// <summary>
        /// 获取缓存的值
        /// </summary>
        /// <typeparam name="ValueT">缓存值类型</typeparam>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="cacheKey">缓存管理器的键</param>
        /// <param name="key">缓存的键</param>
        /// <param name="clear">清理此缓存</param>
        /// <returns>缓存的值</returns>
        public static async Task<ValueT> GetValue<ValueT>(this ICacheManager cacheManager, string cacheKey, string key, bool clear = false)
        {
            var cache = cacheManager.GetCache(cacheKey);
            var cacheValue = await cache?.AsTyped<string, ValueT>()
                .GetOrDefaultAsync(key);

            if (clear)
            {
                await cache.RemoveAsync(key);
            }
            return cacheValue;
        }








        #region 私有获取验证码配置信息的类


        /// <summary>
        /// 获取宿主注册租户验证码配置
        /// </summary>
        /// <param name="settingManager"></param>
        /// <returns></returns>
        private static async Task<CaptchaConfig> GetHostTenantRegistrationCaptchaConfig(ISettingManager settingManager)
        {
            return new CaptchaConfig()
            {
                Enabled = await settingManager.GetSettingValueAsync<bool>(AppSettingNames.HostSettings.UseCaptchaOnTenantRegistration),
                Type = await settingManager.GetSettingValueAsync<int>(AppSettingNames.HostSettings.CaptchaOnTenantRegistrationType),
                Length = await settingManager.GetSettingValueAsync<int>(AppSettingNames.HostSettings.CaptchaOnTenantRegistrationLength),
            };
        }


        /// <summary>
        /// 获取用户注册验证码配置
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="tenantId">租户Id</param>
        /// <returns></returns>
        private static async Task<CaptchaConfig> GetUserRegistrationCaptchaConfig(ISettingManager settingManager, int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return new CaptchaConfig()
                {
                    Enabled = await settingManager.GetSettingValueForTenantAsync<bool>(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserRegistration, tenantId.Value),
                    Type = await settingManager.GetSettingValueForTenantAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationType, tenantId.Value),
                    Length = await settingManager.GetSettingValueForTenantAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationLength, tenantId.Value),
                };
            }
            return new CaptchaConfig()
            {
                Enabled = await settingManager.GetSettingValueAsync<bool>(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserRegistration),
                Type = await settingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationType),
                Length = await settingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserRegistrationLength),
            };
        }

        /// <summary>
        /// 获取用户登陆验证码配置
        /// </summary>
        /// <param name="settingManager"></param>
        /// <param name="tenantId">租户Id</param>
        /// <returns></returns>
        private static async Task<CaptchaConfig> GetUserLoginCaptchaConfig(ISettingManager settingManager, int? tenantId)
        {
            if (tenantId.HasValue)
            {

                return new CaptchaConfig()
                {
                    Enabled = await settingManager.GetSettingValueForTenantAsync<bool>(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin, tenantId.Value),
                    Type = await settingManager.GetSettingValueForTenantAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType, tenantId.Value),
                    Length = await settingManager.GetSettingValueForTenantAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength, tenantId.Value),
                };
            }



            return new CaptchaConfig()
            {
                Enabled = await settingManager.GetSettingValueAsync<bool>(AppSettingNames.ApplicationAndTenant.UseCaptchaOnUserLogin),
                Type = await settingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginType),
                Length = await settingManager.GetSettingValueAsync<int>(AppSettingNames.ApplicationAndTenant.CaptchaOnUserLoginLength),
            };
        }

        #endregion
    }
}
