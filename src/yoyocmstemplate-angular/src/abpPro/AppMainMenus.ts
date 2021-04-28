import { Menu } from '@delon/theme';
import { AbpProMenus } from './menus/AbpProMenus';
/**
 * 全局的左侧边栏的菜单导航配置信息
 */
export class AppMainMenus {
  static Menus: AbpProMenus[] = [
    {
      // DEMO样式模块 13
      text: '',
      i18n: 'DemoManagement',
      icon: 'iconfont icon-yanshihuiyi',
      link: '/app/demo/demoui',
      sort: 13,
      // children: [
      //   //
      //   {
      //     text: undefined,
      //     i18n: 'DemoMarkDown',
      //     acl: 'Pages.WechatAppConfig',
      //     icon: 'iconfont icon-dashboard',
      //     link: '/app/demo/markdown'
      //   }
      // ]
    },
    // 数据分析 14
    {
      // 数据分析
      text: '',
      i18n: 'DataAnalysis',
      acl: '',
      icon: 'iconfont icon-fenxi',
      sort: 14,

      children: [
        {
          text: '',
          i18n: 'DProjLog',
          acl: 'Pages.DownloadLog',
          icon: 'anticon anticon-dashboard',
          link: '/app/data-analysis/download-log',
          sort: 1,
        },
        {
          text: 'UserDownloadConfig',
          i18n: 'UserDownloadConfig',
          acl: 'Pages.UserDownloadConfig',
          icon: 'anticon anticon-dashboard',
          link: '/app/data-analysis/user-download-config',
          sort: 2,
        },
      ],
    },
    // 关于我们 15

    {
      // 关于我们
      text: '',
      i18n: 'About',
      icon: 'iconfont icon-warning-circle',
      link: '/app/main/about',
      sort: 15,
    },
  ];
}
