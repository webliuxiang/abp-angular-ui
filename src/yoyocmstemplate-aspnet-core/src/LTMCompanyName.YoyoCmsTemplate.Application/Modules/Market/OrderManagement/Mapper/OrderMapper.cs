using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.Mapper
{

    /// <summary>
    /// 配置Order的AutoMapper
    /// </summary>
    internal static class OrderMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Order, OrderEditDto>();
            configuration.CreateMap<OrderEditDto, Order>();

            configuration.CreateMap<Order, OrderListDto>()
                .ForMember(o => o.StatusStr, options => options.Ignore());
            configuration.CreateMap<OrderListDto, Order>();

            configuration.CreateMap<Order, UserCenterOrderListDto>();

        }
    }
}
