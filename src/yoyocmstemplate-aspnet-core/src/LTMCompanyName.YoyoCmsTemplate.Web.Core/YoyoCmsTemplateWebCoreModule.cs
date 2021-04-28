using System;
using System.IO;
using System.Text;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Configuration.Startup;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Zero.Configuration;
using L._52ABP.Application;
using L._52ABP.Web.Core;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer;
using LTMCompanyName.YoyoCmsTemplate.Authentication.TwoFactor;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Helpers.Debugging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Yoyo.Abp;

namespace LTMCompanyName.YoyoCmsTemplate
{
    [DependsOn(
        typeof(YoyoCmsTemplateApplicationModule),
        typeof(YoyoCmsTemplateEntityFrameworkModule),
        typeof(AbpAspNetCoreModule),
        typeof(AbpAspNetCoreSignalRModule),
        typeof(L52AbpWebCoreModule),
         typeof(YoYoAlipayModule),
        typeof(YoyoAbpRedisCacheModule), // 如果不使用Redis做缓存,可以移除 Yoyo.Abp.RedisCache
        typeof(AbpHangfireAspNetCoreModule) // 如果不使用Hangfire做后台任务,可以移除 Abp.Hangfire 和 Hangfire.Sqlxxxx
    )]
    public class YoyoCmsTemplateWebCoreModule : AbpModule
    {
        private readonly IConfiguration _appConfiguration;
        private readonly IWebHostEnvironment _env;

        public YoyoCmsTemplateWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            // 只有在开发环境下才会把错误发送到客户端
            if (DebugHelper.IsDebug)
            {
                Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;

            }

            // 默认数据库连接字符串
            Configuration.DefaultNameOrConnectionString = _appConfiguration.ConnectionStringsDefault();

            // 使用数据库管理语言
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            // 多租户的启用或关闭
            Configuration.MultiTenancy.IsEnabled = _appConfiguration.MultiTenancyIsEnabled();
            // 多租户请求头
            Configuration.MultiTenancy.TenantIdResolveKey = AppConsts.TenantIdResolveKey;

            // 注册动态webapi
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(YoyoCmsTemplateApplicationModule).GetAssembly()
                );

            // 注册动态webapi
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(L52AbpApplicationModule).GetAssembly()
                );

            // 设置双向验证缓存的绝对过期时间
            Configuration.Caching.Configure(
                TwoFactorCodeCacheItem.CacheName,
                (cache) =>
                {
                    cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(2);
                });

            // 配置 jwt token
            if (_appConfiguration.AuthenticationJwtBearerIsEnabled())
            {
                ConfigureTokenAuth();
            }

            // 配置访问器
            Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();

            // 将默认后台任务替换成 Hangfire,还需要取消Startup中对Hangfire配置的注释
            Configuration.BackgroundJobs.UseHangfire();

            // TODO: 将默认缓存切换为Redis
            // 配置信息位于 appsettings.json中
            //Configuration.Caching.UseYoyoRedis(options =>
            //{
            //    options.ConnectionString = _appConfiguration["Cache:Redis:ConnectionString"];
            //    options.DatabaseId = _appConfiguration.GetValue<int>("Cache:Redis:DatabaseId");
            //});
        }

        /// <summary>
        /// 注册token配置
        /// </summary>
        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_appConfiguration.AuthenticationJwtBearerSecurityKey())
                    );
            tokenAuthConfig.Issuer = _appConfiguration.AuthenticationJwtBearerIssuer();
            tokenAuthConfig.Audience = _appConfiguration.AuthenticationJwtBearerAudience();
            tokenAuthConfig.SigningCredentials = new SigningCredentials(
                tokenAuthConfig.SecurityKey,
                SecurityAlgorithms.HmacSha256
                );
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            SetAppFolders();
        }

        /// <summary>
        /// 启动项目的时候设置存档的文件夹信息
        /// </summary>
        private void SetAppFolders()
        {
            var appFolders = IocManager.Resolve<AppFolder>();

            appFolders.SampleProfileImagesFolder = Path.Combine(_env.WebRootPath,
                $"Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}SampleProfilePics");

            appFolders.WebSiteLogsFolder =
                Path.Combine(_env.ContentRootPath, $"App_Data{Path.DirectorySeparatorChar}Logs");

            appFolders.WebContentRootPath = _env.WebRootPath;

            appFolders.TempFileDownloadFolder =
                Path.Combine(_env.WebRootPath, $"Temp{Path.DirectorySeparatorChar}Downloads");

            appFolders.SysFileRootFolder = Path.Combine(_env.WebRootPath, $"SysFiles");

            appFolders.ProjectRootFolder = Path.Combine(_env.WebRootPath, $"Projects");

            #region ABP项目模板
            // 模板路径
            appFolders.DownloadAbpTemplate = Path.Combine(_env.ContentRootPath, $"Download{Path.DirectorySeparatorChar}ABPTemplate");

            // 下载路径
            // .net core
            appFolders.DownloadProjectChargeDirectory = Path.Combine(_env.ContentRootPath, $"Download{Path.DirectorySeparatorChar}OutPut{Path.DirectorySeparatorChar}Charge");
            appFolders.DownloadProjectFreeDirectory = Path.Combine(_env.ContentRootPath, $"Download{Path.DirectorySeparatorChar}OutPut{Path.DirectorySeparatorChar}Free");



            #endregion

            try
            {
                DirectoryHelper.CreateIfNotExists(appFolders.SampleProfileImagesFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.SysFileRootFolder);

                DirectoryHelper.CreateIfNotExists(appFolders.DownloadAbpTemplate);
                DirectoryHelper.CreateIfNotExists(appFolders.DownloadProjectChargeDirectory);
                DirectoryHelper.CreateIfNotExists(appFolders.DownloadProjectFreeDirectory);
            }
            catch
            {
                // ignored
            }
        }
    }
}
