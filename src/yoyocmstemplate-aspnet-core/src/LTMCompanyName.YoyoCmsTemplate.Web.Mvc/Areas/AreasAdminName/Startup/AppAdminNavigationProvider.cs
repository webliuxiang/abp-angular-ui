using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.Startup
{
    public class AppAdminNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] =
                new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        AppAdminPageNames.Host.Dashboard,
                        L("Dashboard"),
                        url: "AreasAdminName/HostDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_Host_Dashboard)
                    )
                ).AddItem(new MenuItemDefinition(
                    AppAdminPageNames.Host.Tenants,
                    L("Tenants"),
                    url: "AreasAdminName/Tenants",
                    icon: "flaticon-list-3",
                    permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Tenants)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppAdminPageNames.Host.Editions,
                        L("Editions"),
                        url: "AreasAdminName/Editions",
                        icon: "flaticon-app",
                     permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Editions)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppAdminPageNames.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "AreasAdminName/Dashboard",
                        icon: "flaticon-line-graph",
                     permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Tenant_Dashboard)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppAdminPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8"
                    ).AddItem(new MenuItemDefinition(
                            AppAdminPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "AreasAdminName/OrganizationUnits",
                            icon: "flaticon-map",
                             permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_OrganizationUnits)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAdminPageNames.Common.Roles,
                            L("Roles"),
                            url: "AreasAdminName/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_Roles)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAdminPageNames.Common.Users,
                            L("Users"),
                            url: "AreasAdminName/Users",
                            icon: "flaticon-users",
                          permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_Users)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAdminPageNames.Common.Languages,
                            L("Languages"),
                            url: "AreasAdminName/Languages",
                            icon: "flaticon-tabs",
                           permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_Languages)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAdminPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "AreasAdminName/AuditLogs",
                            icon: "flaticon-folder-1",
                         permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_AuditLogs)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAdminPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "AreasAdminName/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_Host_Maintenance)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAdminPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "AreasAdminName/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_Tenant_SubscriptionManagement)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppAdminPageNames.Host.Settings,
                            L("Settings"),
                            url: "AreasAdminName/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_Host_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppAdminPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "AreasAdminName/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(YoyoSoftPermissionNames.Pages_Administration_Tenant_Settings)
                        )
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}