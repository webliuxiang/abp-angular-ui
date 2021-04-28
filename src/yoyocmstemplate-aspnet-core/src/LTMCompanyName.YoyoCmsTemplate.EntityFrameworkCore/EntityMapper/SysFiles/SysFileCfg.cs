using LTMCompanyName.YoyoCmsTemplate.Modules.FileManager;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.SysFiles
{
    public class SysFileCfg : IEntityTypeConfiguration<SysFile>
    {
        public void Configure(EntityTypeBuilder<SysFile> builder)
        {
            // builder.ToTable("SysFiles", YoYoAbpefCoreConsts.SchemaNames.CMS);
            builder.ToTable("SysResource");

            //可以自定义配置参数内容

            //// custom codes

            //// custom codes end
        }
    }
}
