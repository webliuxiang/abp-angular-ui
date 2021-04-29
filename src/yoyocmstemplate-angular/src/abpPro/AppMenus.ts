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
    {
      // 数据分析
      text: '',
      i18n: '数据挖掘',
      acl: 'Pages.YSLogSearchObject',
      icon: 'iconfont icon-stock',
      sort: 2,
      hideInBreadcrumb: true,
      children: [
        {
          // Discover
          text: '',
          i18n: '数据检索',
          acl: 'Pages.YSLogSearchObject',
          icon: 'iconfont icon-compass',
          link: '/app/data-analyze/discover',
          sort: 1,
          hideInBreadcrumb: true,
        },
        {
          // Visualize
          text: '',
          i18n: '数据图表',
          acl: 'Pages.YSLogVisualizeObject',
          icon: 'iconfont icon-pointmap',
          link: '/app/data-analyze/visualize',
          sort: 2,
          hideInBreadcrumb: true,
        },
        {
          // Discover
          text: '',
          i18n: '数据分析',
          acl: 'Pages.YSLogDataAnalyzeObject',
          icon: 'iconfont icon-Report',
          link: '/app/data-analyze/report',
          sort: 3,
          hideInBreadcrumb: true,
        },
      ],
    },
    {
      // 管理
      text: '',
      i18n: 'SystemAdministration',
      acl: 'Pages.Administration',
      icon: 'iconfont icon-appstore',
      sort: 3,
      hideInBreadcrumb: true,
      children: [
        {
          // 用户
          text: '',
          i18n: 'UsersManager',
          acl: 'Pages.Administration.Users',
          icon: 'iconfont icon-user',
          link: '/app/admin/users',
          sort: 3,
          hideInBreadcrumb: true,
        },
        // {
        //   // 语言
        //   text: '',
        //   i18n: 'Languages',
        //   acl: 'Pages.Administration.Languages',
        //   icon: 'iconfont icon-earth',
        //   link: '/app/admin/languages',
        //   sort: 4,
        //   hideInBreadcrumb: true,
        // },
        {
          // 审计日志
          text: '',
          i18n: 'AuditLogs',
          acl: 'Pages.Administration.AuditLogs',
          icon: 'iconfont icon-book',
          link: '/app/admin/auditLogs',
          sort: 5,
          hideInBreadcrumb: true,
        },
        {
          // 系统维护
          text: '',
          i18n: 'SystemMaintenance',
          acl: 'Pages.Administration.Host.Maintenance',
          icon: 'iconfont icon-formatpainter',
          link: '/app/admin/maintenance',
          sort: 6,
          hideInBreadcrumb: true,
        },
        {
          // 系统设置
          text: '',
          i18n: 'SystemSettings',
          acl: 'Pages.Administration.Host.Settings',
          icon: 'iconfont icon-setting',
          link: '/app/admin/system-settings',
          sort: 8,
          hideInBreadcrumb: true,
        },
      ],
    },
    {
      // 数据采集
      text: '数据采集',
      i18n: '数据采集',
      acl: 'Pages.YSLogDataCollectorObject',
      icon: 'iconfont icon-merge-cells',
      sort: 4,
      hideInBreadcrumb: true,
      children: [
        {
          // client-management
          text: '客户端管理',
          i18n: '客户端管理',
          acl: 'Pages.YSLogDataClientObject',
          icon: 'iconfont icon-desktop',
          link: '/app/data-collect/client-management',
          sort: 1,
          hideInBreadcrumb: true,
        },
        {
          // collect-task-management
          text: '采集任务管理',
          i18n: '采集任务管理',
          acl: 'Pages.YSLogDataCollectorObject',
          icon: 'iconfont icon-detail',
          link: '/app/data-collect/collect-task-management',
          sort: 2,
          hideInBreadcrumb: true,
        },
        {
          // data-etl-management
          text: '数据ETL',
          i18n: '数据ETL',
          acl: 'Pages.YSLogDataFormatterObject',
          icon: 'iconfont icon-icon-test',
          link: '/app/data-collect/data-etl-management',
          sort: 3,
          hideInBreadcrumb: true,
        },
      ],
    },
    {
      // 数据管理
      text: '数据管理',
      i18n: '数据管理',
      acl: 'Pages.YSLogDataSetObject',
      icon: 'iconfont icon-folder-open',
      sort: 5,
      hideInBreadcrumb: true,
      children: [
        {
          // 数据集管理
          text: '数据集管理',
          i18n: '数据集管理',
          acl: 'Pages.YSLogDataSetObject',
          icon: 'iconfont icon-database',
          link: '/app/data-management/dataset',
          sort: 1,
          hideInBreadcrumb: true,
        },
        {
          // 备份还原管理
          text: '备份还原管理',
          i18n: '备份还原管理',
          acl: 'Pages.YSLogDataBackupObject',
          icon: 'iconfont icon-ungroup',
          link: '/app/data-management/backup-restore',
          sort: 2,
          hideInBreadcrumb: true,
        },
        {
          // 备份存储管理
          text: '备份存储管理',
          i18n: '备份存储管理',
          acl: 'Pages.YSLogMountPointObject',
          icon: 'iconfont icon-group',
          link: '/app/data-management/backup-storage',
          sort: 3,
          hideInBreadcrumb: true,
        },
      ],
    },
    {
      // 告警管理
      text: '告警管理',
      i18n: '告警管理',
      acl: 'Pages.YSLogDataAlertObject',
      icon: 'iconfont icon-alert',
      sort: 6,
      hideInBreadcrumb: true,
      children: [
        {
          // 告警渠道
          text: '告警渠道',
          i18n: '告警渠道',
          acl: '',
          icon: 'iconfont icon-Partition',
          link: '/app/alert-management/alert-channel',
          sort: 1,
          hideInBreadcrumb: true,
        },
        // {
        //   // 告警订阅
        //   text: '告警订阅',
        //   i18n: '告警订阅',
        //   acl: '',
        //   icon: 'iconfont icon-notification',
        //   link: '/app/alert-management/alert-subscribe',
        //   sort: 2,
        //   hideInBreadcrumb: true,
        // },
        {
          // 告警规则
          text: '告警规则',
          i18n: '告警规则',
          acl: 'Pages.YSLogDataAlertObject',
          icon: 'iconfont icon-experiment',
          link: '/app/alert-management/alert-rule',
          sort: 3,
          hideInBreadcrumb: true,
        },
      ],
    },
  ];
}
