using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.Comments
{
    public class CommentCfg : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {


            //   builder.ToTable("Comments", YoYoAbpefCoreConsts.SchemaNames.CMS);
            builder.ToTable("Comments");

            //可以自定义配置参数内容

            //// custom codes



            //// custom codes end
        }
    }
}


