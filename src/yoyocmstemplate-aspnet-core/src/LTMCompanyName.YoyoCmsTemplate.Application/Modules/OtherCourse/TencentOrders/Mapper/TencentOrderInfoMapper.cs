using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Mapper
{

	/// <summary>
    /// 配置TencentOrderInfo的AutoMapper
    /// </summary>
	internal static class TencentOrderInfoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <TencentOrderInfo,TencentOrderInfoListDto>();
            configuration.CreateMap <TencentOrderInfoListDto,TencentOrderInfo>();

            configuration.CreateMap <ImportTencentOrderDto, TencentOrderInfo>();
            configuration.CreateMap <TencentOrderInfoEditDto,TencentOrderInfo>();
            configuration.CreateMap <TencentOrderInfo,TencentOrderInfoEditDto>();

        }
	}
}
