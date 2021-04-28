using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Editions.Authorization;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    public class AppProAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppProAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppProAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }



        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Host permissions
            //只有当宿主登陆后才能管理的权限
            //var tenants = administration.CreateChildPermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);


            // 顶级公共权限-页面

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));


            // 组织机构
            var organizationUnits = administration.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageUsers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            //Pages_Administration_OrganizationUnits_ManageMembers

            // 角色
            var roles = administration.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Roles_Delete, L("DeletingRole"));


            // 用户
            var users = administration.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users_BatchDelete, L("BatchDeletingUser"));
            users.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users_DeleteProfilePicture, L("DeleteProfilePicture"));
            users.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users_ExportToExcel, L("ExportToExcel"));
            users.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Users_Unlock, L("UserLockOut"));


            // 语言
            var languages = administration.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));


            // 审计日志
            var auditLogs = administration.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_AuditLogs, L("AuditLogs"));

            // Host独有

            var editions = pages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(YoyoSoftPermissionNames.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(YoyoSoftPermissionNames.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(YoyoSoftPermissionNames.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(YoyoSoftPermissionNames.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(EditionAppPermissions.Query, L("Query"), multiTenancySides: MultiTenancySides.Host);



            var tenants = pages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(YoyoSoftPermissionNames.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(YoyoSoftPermissionNames.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(YoyoSoftPermissionNames.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(YoyoSoftPermissionNames.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(YoyoSoftPermissionNames.Pages_Tenants_BatchDelete, L("BatchDelete"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(YoyoSoftPermissionNames.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);
            pages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            pages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);

            pages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);

            pages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);


            // Tenant 独有
            pages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            pages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);
            pages.CreateChildPermission(YoyoSoftPermissionNames.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
