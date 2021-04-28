

using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.BlogrollTypes
{
    public class BlogrollTypeCfg : IEntityTypeConfiguration<BlogrollType>
    {
        public void Configure(EntityTypeBuilder<BlogrollType> builder)
        {


            //   builder.ToTable("BlogrollTypes", YoYoAbpefCoreConsts.SchemaNames.CMS);
            builder.ToTable("BlogrollTypes");

            //可以自定义配置参数内容

            //// custom codes



            //// custom codes end
        }
    }
}


