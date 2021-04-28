using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.MultiTenancy
{
    public class TenantCfg : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasIndex(e => new { e.SubscriptionEndUtc });
            builder.HasIndex(e => new { e.CreationTime });
        }
    }
}
