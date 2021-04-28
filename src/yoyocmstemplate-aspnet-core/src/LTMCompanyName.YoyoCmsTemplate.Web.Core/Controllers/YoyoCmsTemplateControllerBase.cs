using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Abp.Runtime.Caching;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Security.Captcha;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    public abstract class YoyoCmsTemplateControllerBase : AbpController
    {

        public ICacheManager CacheManager { get; set; }

        protected YoyoCmsTemplateControllerBase()
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        /// <summary>
        /// Session 移除并抛出异常
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="sessionCacheKey"></param>
        protected void ThrowExceptionAndRemoveSession(string msg, string sessionCacheKey)
        {
            // 移除session缓存并抛出异常
            HttpContext.Session.Remove(sessionCacheKey);
            throw new UserFriendlyException(msg);
        }

        /// <summary>
        ///     检查图形验证码功能
        /// </summary>
        /// <param name="type">图形验证码的类型</param>
        /// <param name="code">待验证的code</param>
        
        protected async Task<bool> VerifyImgCodeAsync(CaptchaType type, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new UserFriendlyException("验证码不能为空!");
            }

            //租户Id
            var tenantId = AbpSession.TenantId;
            // 分租户获取缓存
            var cacheKey = CaptchaHelper.CreateCacheKey(type, tenantId);
            //图形验证码缓存管理器
            var captchaCacheManager = CacheManager.GetCache(cacheKey);

            var sessionId = HttpContext.Request.Cookies[cacheKey];

            //获取缓存中的值
            var cacheCode = await captchaCacheManager.GetOrDefaultAsync(sessionId);



            if (cacheCode != null && code == cacheCode.ToString())
            {
                await captchaCacheManager.RemoveAsync(sessionId);
                return true;
            }

            await captchaCacheManager.RemoveAsync(sessionId);
            throw new UserFriendlyException("验证码错误!");

        }

    }
}
