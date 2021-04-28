using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
 
namespace YoYo.ABPCommunity.EntityMapper.OtherOrders.NeteaseOrders
{
    public class NeteaseOrderInfoCfg : IEntityTypeConfiguration<NeteaseOrderInfo>
    {
        public void Configure(EntityTypeBuilder<NeteaseOrderInfo> builder)
        {

            builder.ToTable("OrderInfos");

        }
    }
}


