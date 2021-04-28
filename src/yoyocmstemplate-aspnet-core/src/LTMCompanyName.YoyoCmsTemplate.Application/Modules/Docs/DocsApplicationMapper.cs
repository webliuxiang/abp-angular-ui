using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos.version;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents.Models;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs
{

    /// <summary>
    /// 配置Project的AutoMapper
    /// </summary>
    internal static class DocsApplicationMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Project, ProjectDto>();

            configuration.CreateMap<Project, ProjectListDto>();
            configuration.CreateMap<ProjectListDto, Project>();

            configuration.CreateMap<ProjectEditDto, Project>();
            configuration.CreateMap<Project, ProjectEditDto>();
        

            configuration.CreateMap<VersionInfo, VersionInfoDto>();

            configuration.CreateMap<Document, DocumentWithDetailsDto>();
               
            configuration.CreateMap<DocumentContributor, DocumentContributorDto>();

            configuration.CreateMap<DocumentResource, DocumentResourceDto>();

            


            configuration.CreateMap<DocumentWithDetailsDto, NavigationWithDetailsDto>()
                .ForMember(x => x.RootNode, option => option.Ignore());

        }
    }
}
