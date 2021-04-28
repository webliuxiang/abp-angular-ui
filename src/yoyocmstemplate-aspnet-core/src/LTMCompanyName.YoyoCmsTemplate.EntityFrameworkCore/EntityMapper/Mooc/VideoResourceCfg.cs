using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.Mooc
{
    public class VideoResourceCfg : IEntityTypeConfiguration<VideoResource>
    {
        public void Configure(EntityTypeBuilder<VideoResource> builder)
        {

            builder.ToTable("VideoResources");

           


            //builder.Property(a => a.Name).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
            //builder.Property(a => a.Type).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
            //builder.Property(a => a.Url).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
            //builder.Property(a => a.ImgUrl).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
            //builder.Property(a => a.Duration).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
            //builder.Property(a => a.Code).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
            //builder.Property(a => a.FileSize).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


