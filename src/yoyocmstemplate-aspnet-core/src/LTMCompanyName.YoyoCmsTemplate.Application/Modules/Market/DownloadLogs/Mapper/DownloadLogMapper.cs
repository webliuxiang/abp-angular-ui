using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Mapper
{

	/// <summary>
    /// 配置DownloadLog的AutoMapper
    /// </summary>
	internal static class DownloadLogMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <DownloadLog,DownloadLogListDto>();
            configuration.CreateMap <DownloadLogListDto,DownloadLog>();
        }
	}
}
