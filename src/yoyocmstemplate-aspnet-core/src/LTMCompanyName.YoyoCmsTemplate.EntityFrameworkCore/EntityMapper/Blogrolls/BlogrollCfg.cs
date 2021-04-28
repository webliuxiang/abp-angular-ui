

using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.Blogrolls
{
    public class BlogrollCfg : IEntityTypeConfiguration<Blogroll>
    {
        public void Configure(EntityTypeBuilder<Blogroll> builder)
        {


            //   builder.ToTable("Blogrolls", YoYoAbpefCoreConsts.SchemaNames.CMS);
            builder.ToTable("Blogrolls");

            //可以自定义配置参数内容

            //// custom codes



            //// custom codes end
        }
    }
}


