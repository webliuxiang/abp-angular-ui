using Abp.Dapper;
using Abp.Dependency;
using Abp.EntityFrameworkCore.Configuration;
using Abp.IdentityServer4;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate
{
    [DependsOn(
        typeof(YoyoCmsTemplateCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule),
        typeof(AbpZeroCoreIdentityServerEntityFrameworkCoreModule),
        typeof(AbpDapperModule)
        )]
    public class YoyoCmsTemplateEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        private readonly IConfiguration _appConfiguration;

        public YoyoCmsTemplateEntityFrameworkModule(IWebHostEnvironment env = null)
        {
            #region 获取应用的配置, 如果是迁移工具启动，则 env 为空

            if (env == null)
            {
                _appConfiguration = AppConfigurations.Get(
                    typeof(YoyoCmsTemplateEntityFrameworkModule).GetAssembly().GetDirectoryPathOrNull()
                );
            }
            else
            {
                _appConfiguration = env.GetAppConfiguration();
            }

            #endregion
        }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
                Configuration.Modules.AbpEfCore().AddDbContext<YoyoCmsTemplateDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        YoyoCmsTemplateDbContextConfigurer.Configure(
                            _appConfiguration,
                            options.DbContextOptions,
                            options.ExistingConnection
                            );
                    }

                    else
                        YoyoCmsTemplateDbContextConfigurer.Configure(
                            _appConfiguration,
                            options.DbContextOptions,
                            options.ConnectionString
                            );
                });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var configurationAccessor = IocManager.Resolve<IAppConfigurationAccessor>();

            using (var scope = IocManager.CreateScope())
            {
                if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration.GetConnectionString(AppSettingNames.System.ConnectionStrings_Default)))
                {
                    SeedHelper.SeedHostDb(IocManager);
                }
            }
        }
    }
}
