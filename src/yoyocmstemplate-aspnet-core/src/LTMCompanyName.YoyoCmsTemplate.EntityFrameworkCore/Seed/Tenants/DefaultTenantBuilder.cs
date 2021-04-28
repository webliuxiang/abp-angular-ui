using System.Linq;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Tenants
{
    public class DefaultTenantBuilder
    {
        private readonly YoyoCmsTemplateDbContext _context;

        public DefaultTenantBuilder(YoyoCmsTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDefaultTenant();
        }

        private void CreateDefaultTenant()
        {
            //创建Default租户

            var defaultTenant = _context.Tenants.IgnoreQueryFilters()
                .FirstOrDefault(t => t.TenancyName == AbpTenantBase.DefaultTenantName);
            if (defaultTenant == null)
            {
                defaultTenant = new Tenant(AbpTenantBase.DefaultTenantName, AbpTenantBase.DefaultTenantName);

                var defaultEdition = _context.Editions.IgnoreQueryFilters()
                    .FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                if (defaultEdition != null) defaultTenant.EditionId = defaultEdition.Id;

                _context.Tenants.Add(defaultTenant);
                _context.SaveChanges();
            }
        }
    }
}
