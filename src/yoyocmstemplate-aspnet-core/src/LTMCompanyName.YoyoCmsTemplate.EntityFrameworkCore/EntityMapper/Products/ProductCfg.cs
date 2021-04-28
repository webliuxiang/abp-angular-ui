using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.Products
{
    public class ProductCfg : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.ToTable("Products");

            
			//builder.Property(a => a.Name).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Price).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Type).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.CreateProjectCount).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Recommended).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Indate).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Code).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


