

using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
 
namespace YoYo.ABPCommunity.EntityMapper.Orders
{
    public class OrderCfg : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.ToTable("Orders");

            
			//builder.Property(a => a.ProductName).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.ProductId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.ProductCode).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Price).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Discounts).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.ActualPayment).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Status).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


