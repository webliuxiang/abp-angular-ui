using Aliyun.Acs.vod.Model.V20170321;
using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.MoocModuleDtoMapper
{

    /// <summary>
    /// 配置Course的AutoMapper
    /// </summary>
    internal static class MoocDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Course, CourseDto>().ReverseMap();
            configuration.CreateMap<Course, CourseListDto>()
                .ForMember(obj => obj.OrderCount, option => option.Ignore())
                .ReverseMap();
            configuration.CreateMap<CourseEditDto, Course>().ReverseMap();

            configuration.CreateMap<Course, CourseDetailsDto>();


            // 章节
            configuration.CreateMap<CourseSection, CourseSectionDto>().ReverseMap();

            configuration.CreateMap<CourseSection, CourseSectionDetailsDto>().ReverseMap();

            // 课时
            configuration.CreateMap<CourseClassHour, CourseClassHourDto>().ReverseMap();





         


            configuration.CreateMap<VideoResource, VideoResourceListDto>();
            configuration.CreateMap<VideoResourceListDto, VideoResource>();

            configuration.CreateMap<VideoResourceEditDto, VideoResource>();
            configuration.CreateMap<VideoResource, VideoResourceEditDto>();




            //   configuration.CreateMap<GetVideoInfosResponse.GetVideoInfos_Video, VideoResource>();

            configuration.CreateMap<GetVideoInfosResponse.GetVideoInfos_Video, VideoResource>()
                //  .ForMember(v => v.CoverURL, opts => opts.MapFrom(d => d.CoverURL))
                .ForMember(v => v.SyncCreationTime, opts => opts.MapFrom(d => d.CreationTime))
                ;
            configuration.CreateMap<CourseCategory, CourseCategoryListDto>();
            configuration.CreateMap<CourseCategoryListDto, CourseCategory>();

            configuration.CreateMap<CourseCategoryEditDto, CourseCategory>();
            configuration.CreateMap<CourseCategory, CourseCategoryEditDto>();

            //CourseVideoResourceListDto



        }
    }
}
