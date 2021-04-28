
using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Dropdown;
using LTMCompanyName.YoyoCmsTemplate.Dropdown.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.CustomDtoAutoMapper
{

    /// <summary>
    /// 配置DropdownList的AutoMapper映射
    /// 前往 <see cref="YoyoCmsTemplateApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// DropdownListDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    internal static class DropdownListDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <DropdownList,DropdownListListDto>();
            configuration.CreateMap <DropdownListListDto,DropdownList>();

            configuration.CreateMap <DropdownListEditDto,DropdownList>();
            configuration.CreateMap <DropdownList,DropdownListEditDto>();
					 
        }
	}
}
