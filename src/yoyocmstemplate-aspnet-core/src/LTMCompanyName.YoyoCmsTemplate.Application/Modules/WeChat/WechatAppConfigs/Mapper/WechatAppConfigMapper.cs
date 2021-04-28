using AutoMapper;
using L._52ABP.Common.Extensions.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMenus;
using LTMCompanyName.YoyoCmsTemplate.WechatManagement;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Mapper
{

    /// <summary>
    /// 配置WechatAppConfig的AutoMapper
    /// </summary>
    internal static class WechatAppConfigMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<WechatAppConfig, WechatAppConfigListDto>()
                .ForMember(o => o.AppTypeStr, options => options.Ignore())
                .ForMember(o => o.Registered, options => options.Ignore());
            configuration.CreateMap<WechatAppConfigListDto, WechatAppConfig>();

            configuration.CreateMap<WechatAppConfigEditDto, WechatAppConfig>();
            configuration.CreateMap<WechatAppConfig, WechatAppConfigEditDto>();


            WechatMenuAppSevice.WechatMenuTypeList = EnumExtensions.GetEnumTypeDescriptionNameList<WechatMenuTypeEnum>();

            WechatAppConfigAppService.WechatAppTypeKVList = EnumExtensions.GetEntityStringIntKeyValueList<WechatAppTypeEnum>();
        }
    }
}
