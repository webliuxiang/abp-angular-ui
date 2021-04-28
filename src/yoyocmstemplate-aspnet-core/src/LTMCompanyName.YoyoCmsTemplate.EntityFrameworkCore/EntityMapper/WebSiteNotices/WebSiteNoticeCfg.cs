

using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.WebSiteNotices
{
    public class WebSiteNoticeCfg : IEntityTypeConfiguration<WebSiteNotice>
    {
        public void Configure(EntityTypeBuilder<WebSiteNotice> builder)
        {


            //   builder.ToTable("WebSiteNotices", YoYoAbpefCoreConsts.SchemaNames.CMS);
            builder.ToTable("WebSiteNotices");

            //可以自定义配置参数内容

            //// custom codes



            //// custom codes end
        }
    }
}


