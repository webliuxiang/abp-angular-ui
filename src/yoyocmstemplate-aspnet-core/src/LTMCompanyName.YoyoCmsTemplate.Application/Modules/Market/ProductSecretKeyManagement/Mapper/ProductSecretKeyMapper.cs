using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.Mapper
{

	/// <summary>
    /// 配置ProductSecretKey的AutoMapper
    /// </summary>
	internal static class ProductSecretKeyMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <ProductSecretKey,ProductSecretKeyListDto>();
            configuration.CreateMap <ProductSecretKeyListDto,ProductSecretKey>();

            configuration.CreateMap <ProductSecretKeyEditDto,ProductSecretKey>();
            configuration.CreateMap <ProductSecretKey,ProductSecretKeyEditDto>();

        }
	}
}
