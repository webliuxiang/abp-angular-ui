using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
 
namespace YoYo.ABPCommunity.EntityMapper.OtherOrders.TencentOrderInfos
{
    public class TencentOrderInfoCfg : IEntityTypeConfiguration<TencentOrderInfo>
    {
        public void Configure(EntityTypeBuilder<TencentOrderInfo> builder)
        {

            builder.ToTable("TencentOrderInfos");

            
			//builder.Property(a => a.OrganizationId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.OrderNumber).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.CourseTitle).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.CourseType).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.CreationTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.PurchaseTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.UIn).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.NickName).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.TradingStatus).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.MobileNumber).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.RecipientPhone).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.RecipientName).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.RecipientAddress).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.TradeType).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.UsersPay).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.PlatformSubsidies).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.RefundAmount).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.AmountPaid).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.SettlementAmount).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.SettlementRatio).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.ChannelFee).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.TrafficConversionFee).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.AppleShares).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.DistributorShare).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.SettlementStatus).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.IsIosOrder).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.IsDistributorOrder).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.OrderType).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.RemarkOne).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.RemarkTwo).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.RemarkThree).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.IsQqGroupChecked).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.IsGiteeChecked).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Is52AbpChecked).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			//builder.Property(a => a.Platform).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


