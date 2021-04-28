using System;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Seed.Host;
using LTMCompanyName.YoyoCmsTemplate.Seed.Tenants;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<YoyoCmsTemplateDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(YoyoCmsTemplateDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            // 初始化宿主的种子数据
            new InitialHostDbBuilder(context).Create();

            // 初始化default租户的种子数据
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
            new DefaultSettingsBuilder(context, 1).Create();
            new DefaultBlogAndPostBuilder(context, 1).Create();




        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
