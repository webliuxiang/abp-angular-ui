using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Mapper
{

	/// <summary>
    /// 配置网易云云课堂订单的AutoMapper
    /// </summary>
	internal static class NeteaseOrderInfoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <NeteaseOrders.NeteaseOrderInfo,NeteaseOrderInfoListDto>();
            configuration.CreateMap <NeteaseOrderInfoListDto,NeteaseOrders.NeteaseOrderInfo>();

            configuration.CreateMap <NeteaseOrderInfoEditDto,NeteaseOrders.NeteaseOrderInfo>();
            configuration.CreateMap <NeteaseOrders.NeteaseOrderInfo,NeteaseOrderInfoEditDto>();

        }
	}
}
