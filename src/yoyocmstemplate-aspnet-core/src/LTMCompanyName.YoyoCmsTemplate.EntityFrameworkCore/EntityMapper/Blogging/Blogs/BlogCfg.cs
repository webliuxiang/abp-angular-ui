

using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.Blogs
{
    public class BlogCfg : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {


            //   builder.ToTable("Blogs", YoYoAbpefCoreConsts.SchemaNames.CMS);
            builder.ToTable("Blogs");

            //可以自定义配置参数内容

            //// custom codes



            //// custom codes end
        }
    }
}


