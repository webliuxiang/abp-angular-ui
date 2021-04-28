namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    public static class YoyoSoftPermissionNames
    {


        #region 用户

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_BatchDelete = "Pages.Administration.Users.BatchDelete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_ExportToExcel = "Pages.Administration.Users.ExportToExcel";
        public const string Pages_Administration_Users_DeleteProfilePicture = "Pages.Administration.Users.DeleteProfilePicture";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";

        #endregion

        #region 角色

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        #endregion

        #region 组织机构

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";

        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";

        public const string Pages_Administration_OrganizationUnits_ManageUsers = "Pages.Administration.OrganizationUnits.ManageUsers";

        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";



        #endregion

        #region 多语言管理

        /// <summary>
        /// Languages管理权限_自带查询授权
        /// </summary>
        public const string Pages_Administration_Languages = "Pages.Administration.Languages";

        /// <summary>
        /// Languages创建权限
        /// </summary>
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        /// <summary>
        /// Languages修改权限
        /// </summary>
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";

        /// <summary>
        /// Languages删除权限
        /// </summary>
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";

        /// <summary>
        /// 文本编辑
        /// </summary>
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        #endregion

        #region 审计日志

        /// <summary>
        /// 审计日志查看权限
        /// </summary>
        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        #endregion

        #region 给Host宿主的特定权限名称


        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_BatchDelete = "Pages.Tenants.BatchDelete";

        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";


        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";


        #endregion

        #region 给租户的特定权限名称

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        #endregion



        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";
    }
}
