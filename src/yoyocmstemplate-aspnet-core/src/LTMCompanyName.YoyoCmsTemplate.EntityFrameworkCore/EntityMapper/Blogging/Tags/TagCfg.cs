using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.Tags
{
    public class TagCfg : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {


            //   builder.ToTable("Tags", YoYoAbpefCoreConsts.SchemaNames.CMS);
            builder.ToTable("Tags");

            //可以自定义配置参数内容

            //// custom codes



            //// custom codes end
        }
    }
}


