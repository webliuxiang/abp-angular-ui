
using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.FileManager;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.CustomDtoAutoMapper
{

    /// <summary>
    /// 配置SysFile的AutoMapper映射
    /// 前往 <see cref="YoyoCmsTemplateApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// SysFileDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    internal static class SysFileDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<SysFile, SysFileListDto>();
            configuration.CreateMap<SysFileListDto, SysFile>();

            configuration.CreateMap<SysFileEditDto, SysFile>();
            configuration.CreateMap<SysFile, SysFileEditDto>();

            //// custom codes



            //// custom codes end
        }
    }
}
