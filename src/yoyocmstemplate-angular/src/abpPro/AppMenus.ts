import { Menu } from '@delon/theme';
import { AbpProMenus } from './menus/AbpProMenus';
/**
 * 全局的左侧边栏的菜单导航配置信息
 */
export class AppMenus {
  static Menus: AbpProMenus[] = [
    {
      // 工作台 1
      text: '',
      i18n: 'Dashboard',
      acl: undefined,
      icon: 'iconfont icon-dashboard ',
      link: '/app/main/dashboard',
      sort: 1,
    },
    // 	 Saas模块 2
    {
      // 	 Saas模块
      text: '',
      i18n: 'Tenants',
      acl: 'Pages.Tenants',
      icon: 'iconfont icon-dashboard ',
      sort: 2,
      children: [
        {
          // 租户管理
          text: '',
          i18n: 'TenantManagement',
          acl: 'Pages.Tenants',
          icon: 'iconfont icon-team',
          link: '/app/admin/tenants',
          sort: 1,
        },
        {
          // 版本管理
          text: '',
          i18n: 'VersionManagement',
          acl: 'Pages.Editions.Query',
          icon: 'iconfont icon-dashboard ',
          link: '/app/admin/editions',
          sort: 2,
        },
      ],
      // link: '/app/blogging/blogs',
    },
    {
      // 多语言
      text: '',
      i18n: 'LanguageManagement',
      acl: 'Pages.Administration.Languages',
      icon: 'iconfont icon-duoyuyan',
      link: '/app/admin/languages',
      sort: 5,
    },
    {
      //
      text: '',
      i18n: 'UserModule',
      acl: 'Pages.Administration.Users',
      icon: 'iconfont icon-user',
      sort: 4,
      children: [
        {
          // 用户
          text: '',
          i18n: 'Users',
          acl: 'Pages.Administration.Users',
          icon: 'iconfont icon-user',
          link: '/app/admin/users',
          sort: 1,
        },
        {
          // 角色
          text: '',
          i18n: 'Roles',
          acl: 'Pages.Administration.Roles',
          icon: 'iconfont icon-safetycertificate',
          link: '/app/admin/roles',
          sort: 2,
        },

        {
          // 组织机构
          text: '',
          i18n: 'OrganizationUnits',
          acl: 'Pages.Administration.OrganizationUnits',
          icon: 'iconfont icon-team',
          link: '/app/admin/organization-units',
          sort: 3,
        },
      ],
    },
    // 维护与日志 12
    {
      // 维护与日志
      text: '维护与日志',
      i18n: 'MaintenanceAndLog',
      acl: 'Pages.Administration',
      icon: 'iconfont icon-appstore',
      sort: 12,
      children: [
        {
          // 审计日志
          text: '',
          i18n: 'AuditLogs',
          acl: 'Pages.Administration.AuditLogs',
          icon: 'iconfont icon-book',
          link: '/app/admin/auditLogs',
          sort: 5,
        },

        {
          // 宿主机器设置/维护
          text: '',
          i18n: 'Maintenance',
          acl: 'Pages.Administration.Host.Maintenance',
          icon: 'iconfont icon-setting',
          link: '/app/admin/maintenance',
          sort: 6,
        },
        {
          // 租户设置
          text: '',
          i18n: 'Settings',
          acl: 'Pages.Administration.Tenant.Settings',
          icon: 'iconfont icon-setting',
          link: '/app/admin/tenant-settings',
          sort: 7,
        },
        {
          // 宿主设置
          text: '',
          i18n: 'Settings',
          acl: 'Pages.Administration.Host.Settings',
          icon: 'iconfont icon-setting',
          link: '/app/admin/host-settings',
          sort: 8,
        },
      ],
    },
  ];
}
