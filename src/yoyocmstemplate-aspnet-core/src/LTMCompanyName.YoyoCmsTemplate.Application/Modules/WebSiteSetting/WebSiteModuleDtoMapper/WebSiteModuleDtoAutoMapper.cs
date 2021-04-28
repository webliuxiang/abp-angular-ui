using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.CustomDtoAutoMapper
{
    internal class WebSiteModuleDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {

            configuration.CreateMap<BannerAd, BannerAdListDto>();
            configuration.CreateMap<BannerAdListDto, BannerAd>();

            configuration.CreateMap<BannerAdEditDto, BannerAd>();
            configuration.CreateMap<BannerAd, BannerAdEditDto>();

            configuration.CreateMap<Blogroll, BlogrollListDto>();
            configuration.CreateMap<BlogrollListDto, Blogroll>();

            configuration.CreateMap<BlogrollEditDto, Blogroll>();
            configuration.CreateMap<Blogroll, BlogrollEditDto>();

            configuration.CreateMap<BlogrollType, BlogrollTypeListDto>();
            configuration.CreateMap<BlogrollTypeListDto, BlogrollType>();

            configuration.CreateMap<BlogrollTypeEditDto, BlogrollType>();
            configuration.CreateMap<BlogrollType, BlogrollTypeEditDto>();

            configuration.CreateMap<WebSiteNotice, WebSiteNoticeListDto>();
            configuration.CreateMap<WebSiteNoticeListDto, WebSiteNotice>();

            configuration.CreateMap<WebSiteNoticeEditDto, WebSiteNotice>();
            configuration.CreateMap<WebSiteNotice, WebSiteNoticeEditDto>();

        }
    }
}
