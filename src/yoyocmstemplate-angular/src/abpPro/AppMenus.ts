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
