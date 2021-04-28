using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members.Mapper
{

	/// <summary>
    /// 配置Member的AutoMapper
    /// </summary>
	internal static class MemberMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Member,MemberListDto>();
            configuration.CreateMap <MemberListDto,Member>();

            configuration.CreateMap <MemberEditDto,Member>();
            configuration.CreateMap <Member,MemberEditDto>();

        }
	}
}
