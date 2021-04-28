using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.Mooc
{
    public class CourseCfg : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {

            builder.ToTable("Courses");

            
			builder.Property(a => a.Title).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length128);
		
			builder.Property(a => a.ImgUrl).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length256);
			builder.Property(a => a.BuyerQqGroup).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length128);
			builder.Property(a => a.NotBuyerQqGroup).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length128);


         //   builder.HasMany(b => b.Period).WithOne();



        }
    }
}


