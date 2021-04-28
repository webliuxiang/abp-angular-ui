using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.DynamicView.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.DynamicView.Mapper
{
    /// <summary>
    /// 配置 DynamicView 的 AutoMapper
    /// </summary>
    internal static class DynamicViewMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ColumnActionItem, ColumnActionItemDto>().ReverseMap();
            configuration.CreateMap<ColumnItem, ColumnItemDto>().ReverseMap();
            configuration.CreateMap<DynamicPage, DynamicPageDto>().ReverseMap();
            configuration.CreateMap<PageFilterItem, PageFilterItemDto>().ReverseMap();
        }
    }
}
