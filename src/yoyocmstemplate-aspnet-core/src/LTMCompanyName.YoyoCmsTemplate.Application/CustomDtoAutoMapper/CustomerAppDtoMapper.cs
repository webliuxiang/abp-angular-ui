using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Organizations;
using Abp.UI.Inputs;
using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.AddressLinkage.Dto;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos.EntityChange;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Chat.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Friendships;
using LTMCompanyName.YoyoCmsTemplate.Friendships.Cache;
using LTMCompanyName.YoyoCmsTemplate.Friendships.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Languages.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Chat;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos.version;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents.Models;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Importing.Dto;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.CustomDtoAutoMapper
{
    /// <summary>
    ///     配置自定义的 的AutoMapper
    /// </summary>
    internal static partial class CustomerAppDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();
            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, UserListOutput>();
            

            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<ImportUserDto, User>();

            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<User, UserMiniDto>();
            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();



            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();


            //Organization
            configuration.CreateMap<OrganizationUnit, OrganizationUnitListDto>();



            // Language
            configuration.CreateMap<ApplicationLanguage, LanguageListDto>();
            configuration.CreateMap<ApplicationLanguage, LanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();
            configuration.CreateMap<Permission, TreePermissionDto>();


            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();




            configuration.CreateMap<AddressCityDto, AddressProvincetDto>();
            configuration.CreateMap<AddressAreaDto, AddressProvincetDto>();
            configuration.CreateMap<AddressStreetDto, AddressProvincetDto>();


            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();


            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();


            configuration.CreateMap<Project, ProjectDto>();
            configuration.CreateMap<Project, ProjectListDto>();
            configuration.CreateMap<Project, ProjectEditDto>().ReverseMap();
            configuration.CreateMap<GetNavigationDocumentInput, NavigationWithDetailsDto>();
            configuration.CreateMap<VersionInfo, VersionInfoDto>();
            configuration.CreateMap<DocumentContributor, DocumentContributorDto>();
            configuration.CreateMap<Document, DocumentWithDetailsDto>();
            configuration.CreateMap<DocumentWithDetailsDto, NavigationWithDetailsDto>();

        }
    }
}
