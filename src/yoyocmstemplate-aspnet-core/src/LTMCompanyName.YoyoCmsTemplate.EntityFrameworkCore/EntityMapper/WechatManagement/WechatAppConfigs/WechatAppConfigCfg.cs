using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.WechatManagement.WechatAppConfigs
{
    public class WechatAppConfigCfg : IEntityTypeConfiguration<WechatAppConfig>
    {
        public void Configure(EntityTypeBuilder<WechatAppConfig> builder)
        {

        }
    }
}
