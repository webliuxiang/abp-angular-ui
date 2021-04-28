using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.ProductSecretKeys
{
    public class ProductSecretKeyCfg : IEntityTypeConfiguration<ProductSecretKey>
    {
        public void Configure(EntityTypeBuilder<ProductSecretKey> builder)
        {

            builder.ToTable("ProductSecretKeys");

            
			//builder.Property(a => a.SecretKey).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.ProductId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.ProductCode).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Used).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.UserId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.UserName).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.OrderId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


