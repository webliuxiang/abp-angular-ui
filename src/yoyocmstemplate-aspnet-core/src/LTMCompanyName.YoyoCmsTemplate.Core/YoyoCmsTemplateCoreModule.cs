using System;
using System.Diagnostics;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MailKit;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using L._52ABP.Core;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.Features;
using LTMCompanyName.YoyoCmsTemplate.Localization;
using LTMCompanyName.YoyoCmsTemplate.Modules.Chat;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Notifications;
using LTMCompanyName.YoyoCmsTemplate.Timing;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Yoyo.Abp;

namespace LTMCompanyName.YoyoCmsTemplate
{
    [DependsOn(
        typeof(L52AbpCoreModule),
        typeof(AbpZeroLdapModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpMailKitModule),
        typeof(YoYoAlipayModule),
        typeof(YoyoAbpAliyunVodModule) 
        )]
    public class YoyoCmsTemplateCoreModule : AbpModule
    {
        public override void PreInitialize()
        {


            // 使用审计日志
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;


            // 声明类型
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);


            // 功能
            Configuration.Features.Providers.Add<AppFeatureProvider>();
            // 设置
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            // 本地化
            YoyoCmsTemplateLocalizationConfigurer.Configure(Configuration.Localization);
            

            // 启用LDAP身份验证(只有禁用多租户才能启用)
            //Configuration.Modules.ZeroLdap().Enable(typeof(AppLdapAuthenticationSource));

            // 配置角色
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);


            // 如果是Debug模式// 禁用邮件发送


            Configuration.ReplaceService<IMailKitSmtpBuilder, YoYoMailKitSmtpBuilder>();



            // 全局缓存配置默认过期时间
            //Configuration.Caching.Configure(PaymentCacheItem.CacheName, cache =>
            //{
            //    cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(AppConsts.PaymentCacheDurationInMinutes);
            //});
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {

            if (Debugger.IsAttached)
            {

                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


                if (environment == "production")
                {
                    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "production-nook");
                }

                if (environment == "Development")
                {
                    //  Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "production-Development");

                    AppVersionHelper.Version = "当前未授权，请购买正版授权内容.";

                }



                //    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                //    throw new UserFriendlyException("未授权，不可进入调试使用该系统。");


                //     Environment.FailFast("未授权，不可进入调试使用该系统。");

                //    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            }

            IocManager.RegisterIfNot<IChatCommunicator, NullChatCommunicator>();
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
