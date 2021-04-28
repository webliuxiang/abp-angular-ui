using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Mapper
{

	/// <summary>
    /// 配置UserDownloadConfig的AutoMapper
    /// </summary>
	internal static class UserDownloadConfigMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <UserDownloadConfig,UserDownloadConfigListDto>();
            configuration.CreateMap <UserDownloadConfigListDto,UserDownloadConfig>();

            configuration.CreateMap <UserDownloadConfigEditDto,UserDownloadConfig>();
            configuration.CreateMap <UserDownloadConfig,UserDownloadConfigEditDto>();

        }
	}
}
