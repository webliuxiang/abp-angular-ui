using System;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.TestBase;
using Abp.Web;
using Abp.Zero.Configuration;
using Abp.Zero.EntityFrameworkCore;
using AngleSharp;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Tests.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;




namespace LTMCompanyName.YoyoCmsTemplate.Tests
{
    [DependsOn(
        typeof(YoyoCmsTemplateApplicationModule),
        typeof(AbpWebCommonModule),
        typeof(YoyoCmsTemplateEntityFrameworkModule),
        typeof(AbpWebCommonModule),
        typeof(AbpTestBaseModule)
        )]
    public class YoyoCmsTemplateTestModule : AbpModule
    {
 
        public YoyoCmsTemplateTestModule(YoyoCmsTemplateEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;
        }

        public override void PreInitialize()
        {
            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            // Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            RegisterFakeService<AbpZeroDbMigrator<YoyoCmsTemplateDbContext>>();

            Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
 Configuration
                .ReplaceService<Microsoft.Extensions.Configuration.IConfiguration,
                    Microsoft.Extensions.Configuration.ConfigurationRoot>();



        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager);
                var services = new ServiceCollection();
            services.AddHttpClient();
            IocManager.IocContainer.AddServices(services);
        }

        private void RegisterFakeService<TService>() where TService : class
        {
            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton()
            );
        }
    }
}
