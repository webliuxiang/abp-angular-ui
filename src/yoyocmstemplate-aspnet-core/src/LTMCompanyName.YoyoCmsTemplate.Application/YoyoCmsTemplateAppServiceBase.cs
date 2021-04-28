using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Abp.Threading;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Identity;

namespace LTMCompanyName.YoyoCmsTemplate
{
    /// <summary>
    ///    项目应用程序服务的基类。
    /// </summary>
    public abstract class YoyoCmsTemplateAppServiceBase : ApplicationService
    {
        protected YoyoCmsTemplateAppServiceBase()
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;
        }

        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        /// <summary>
        ///     返回当前用户信息
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null) throw new Exception("当前用户不存在!");

            return user;
        }
        /// <summary>
        /// 返回当前用户信息--非异步
        /// </summary>
        /// <returns></returns>
        protected virtual User GetCurrentUser()
        {
            return AsyncHelper.RunSync(GetCurrentUserAsync);
        }


        /// <summary>
        ///     返回当前租户信息
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<Tenant> GetCurrentTenantAsync()
        {
            return await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}