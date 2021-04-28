using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Mapper
{

	/// <summary>
    /// 配置Product的AutoMapper
    /// </summary>
	internal static class ProductMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Product,ProductListDto>();
            configuration.CreateMap <ProductListDto,Product>();

            configuration.CreateMap <ProductEditDto,Product>();
            configuration.CreateMap <Product,ProductEditDto>();

        }
	}
}
