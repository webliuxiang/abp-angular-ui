using System;
using Abp.Dependency;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.Tests.Authentication.JwtBearer;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.DependencyInjection
{
    public static class ServiceCollectionRegistrar
    {
        public static void Register(IIocManager iocManager)
        {
            var services = new ServiceCollection();

            IdentityRegistrar.Register(services);

            services.AddEntityFrameworkInMemoryDatabase();

            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);

            var builder = new DbContextOptionsBuilder<YoyoCmsTemplateDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseInternalServiceProvider(serviceProvider);

            iocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<YoyoCmsTemplateDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton()
            );

            iocManager.IocContainer.Register(
                Component
                    .For<IAddCustomerUserClaims>()
                    .ImplementedBy<TestAddCustomerUserClaims>()
                    .LifestyleSingleton()
            );
        }
    }
}
