using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.DataFileObjects
{
    public class DataFileObjectCfg : IEntityTypeConfiguration<DataFileObject>
    {
        public void Configure(EntityTypeBuilder<DataFileObject> builder)
        {

            builder.ToTable("DataFileObject");

            builder.HasIndex(e => new { e.TenantId });
        }
    }
}
