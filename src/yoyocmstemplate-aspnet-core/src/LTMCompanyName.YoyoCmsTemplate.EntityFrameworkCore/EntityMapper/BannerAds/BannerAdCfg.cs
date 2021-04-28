

using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.BannerAds
{
    public class BannerAdCfg : IEntityTypeConfiguration<BannerAd>
    {
        public void Configure(EntityTypeBuilder<BannerAd> builder)
        {


            //   builder.ToTable("BannerAds", YoYoAbpefCoreConsts.SchemaNames.CMS);
            builder.ToTable("BannerAds");

            //可以自定义配置参数内容

            //// custom codes



            //// custom codes end
        }
    }
}


