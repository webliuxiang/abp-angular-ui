

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LTMCompanyName.YoyoCmsTemplate.Message;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.MessageHistorys
{
    public class MessageHistoryCfg : IEntityTypeConfiguration<MessageHistory>
    {
        public void Configure(EntityTypeBuilder<MessageHistory> builder)
        {
            builder.ToTable("MessageHistorys");
        }
    }
}


