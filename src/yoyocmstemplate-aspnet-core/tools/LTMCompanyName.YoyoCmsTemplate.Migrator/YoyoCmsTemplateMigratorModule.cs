using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Configuration.AppSettings;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Migrator.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Migrator
{
    [DependsOn(typeof(YoyoCmsTemplateEntityFrameworkModule))]
    public class YoyoCmsTemplateMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public YoyoCmsTemplateMigratorModule(YoyoCmsTemplateEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(YoyoCmsTemplateMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
               AppSettingNames.System.ConnectionStrings_Default
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;


            

            Configuration.ReplaceService(
                typeof(IEventBus),
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(YoyoCmsTemplateMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
