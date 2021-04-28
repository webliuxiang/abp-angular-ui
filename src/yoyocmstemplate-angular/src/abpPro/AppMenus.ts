import { Menu } from '@delon/theme';
import { AbpProMenus } from './menus/AbpProMenus';

// {
//   //
//   text: '',
//   i18n: 'Tenants',
//   acl: 'Pages.Tenants',
//   icon: 'iconfont icon-dashboard ',
//   sort: 2,
//   children: [
//     {
//       //
//       text: '',
//       i18n: '',
//       acl: '',
//       icon: 'iconfont icon-team',
//       link: '',
//       sort: 1,
//     },
//   ],
//
// },

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
      // SignalR
      text: '',
      i18n: 'InstantChat',
      icon: 'iconfont icon-liaotian',
      link: '/app/other/chat',
      sort: 5,
    },
    {
      // SignalR
      text: '文档项目管理',
      i18n: 'DocumentProjectManagement',
      icon: 'iconfont icon-liaotian',
      link: '/app/other/project',
      sort: 6,
    },
    // 用户模块 4

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
    // 博客模块 5

    {
      // 	 博客的菜单按钮
      text: '',
      i18n: 'BlogModule',
      acl: 'Pages.Blog.Module',
      icon: 'iconfont icon-bokeyuan',
      // link: '/app/blogging/blogs',
      sort: 5,
      children: [
        //
        {
          // 博客模块
          text: '',
          i18n: 'Blog',
          acl: 'Pages.Blog',
          icon: 'iconfont icon-bokeyuan',
          link: '/app/blogging/blogs',
          sort: 1,
        },
        {
          // 	文章的菜单按钮
          text: 'Post',
          i18n: 'Post',
          acl: 'Pages.Post',
          icon: 'iconfont icon-16',
          link: '/app/blogging/posts',
          sort: 2,
        },

        {
          // 	评论的菜单按钮

          text: 'Comment',
          i18n: 'Comment',
          acl: 'Pages.Comment',
          icon: 'iconfont icon-pinglun_huabanfuben',
          link: '/app/blogging/comments',
          sort: 3,
        },

        {
          // 	标签的菜单按钮
          text: 'Tag',
          i18n: 'Tag',
          acl: 'Pages.Tag',
          icon: 'iconfont icon-biaoqian',
          link: '/app/blogging/tagging',
          sort: 4,
        },
      ],
    },
    //  6
    {
      // 课程模块
      text: '慕课模块',
      i18n: 'MoocModule',
      acl: 'Pages.Mooc.Module',
      icon: 'iconfont  icon-kecheng',
      sort: 6,
      children: [
        {
          text: '课程管理',
          i18n: 'CourseManagement',
          acl: 'Pages.CourseManage',
          icon: 'iconfont  icon-dashboard',
          link: '/app/mooc/course',
          sort: 1,
        },
        {
          // 	课程分类的菜单按钮
          text: '课程分类',
          i18n: 'CourseClassification',
          acl: 'Pages.CourseCategory',
          icon: 'iconfont icon-dashboard',
          link: '/app/mooc/coursecategoryinfo',
          sort: 2,
        },
        {
          text: '视频库',
          i18n: 'VideoLibrary',
          acl: 'Pages.VideoResource',
          icon: 'iconfont  icon-dashboard',
          sort: 20,
          children: [
            {
              text: '视频分类',
              i18n: 'VideoClassification',
              acl: 'Pages.VideoResource',
              icon: 'iconfont  icon-dashboard',
              link: '/app/mooc/video-category',
              sort: 2,
            },
            {
              text: '视频资源',
              i18n: 'VideoResource',
              acl: 'Pages.VideoResource',
              icon: 'iconfont  icon-dashboard',
              link: '/app/mooc/video-resource',
              sort: 1,
            },
          ],
        },
      ],
    },

    // 产品模块 7
    {
      text: '产品模块',
      i18n: 'MarketingModule',
      acl: 'Pages.Marketing.Module',
      icon: 'iconfont icon-chanpin',
      // link: '/app/blogging/blogs',
      sort: 7,
      children: [
        // 产品
        {
          text: 'Product',
          i18n: 'Product',
          acl: 'Pages.Product',
          icon: 'iconfont  icon-chanpinku',
          link: '/app/marketing/product',
          sort: 1,
        },
        // 产品密钥
        {
          text: 'ProductSecretKey',
          i18n: 'ProductSecretKey',
          acl: 'Pages.ProductSecretKey',
          icon: 'iconfont  icon-qiamizhifu',
          link: '/app/marketing/product-secret-key',
          sort: 2,
        },
        // 产品订单
        {
          text: 'ProductOrder',
          i18n: 'ProductOrder',
          acl: 'Pages.Order',
          icon: 'iconfont  icon-dingdan',
          link: '/app/marketing/order',
          sort: 3,
        },
      ],
    },

    // 订单模块 8

    {
      //
      text: '订单模块',
      i18n: 'OrderModule',
      acl: 'Pages.Order',
      icon: 'iconfont  icon-ding_huabanfuben',
      sort: 8,
      children: [
        {
          // 网易订单
          text: '网易订单信息',

          i18n: 'NeteaseOrderInfo',
          acl: 'Pages.NeteaseOrderInfo',
          icon: 'iconfont  icon-dashboard',
          link: '/app/other-course-orders/netease-orderinfo',
          sort: 9,
        },
        {
          text: '腾讯订单',
          i18n: 'TencentOrderInfo',
          acl: 'Pages.TencentOrderInfo',
          icon: 'iconfont  icon-dashboard',
          link: '/app/other-course-orders/tencent-orderinfo',
          sort: 10,
        },
      ],
    },
    // 微信模块 9
    {
      text: '',
      i18n: 'WechatManagement',
      icon: 'iconfont icon-gongzhonghao',
      sort: 9,
      children: [
        //
        {
          text: '',
          i18n: 'WechatAppConfig',
          acl: 'Pages.WechatAppConfig',
          icon: 'iconfont icon-warning-circle',
          link: '/app/wechat/wechat-app-config',
          sort: 1,

        },
      ],
    },
    // 资源库 10
    {
      // 	 资源库
      text: '资源库',
      i18n: 'ResourceBase',
      acl: undefined,
      icon: 'iconfont icon-ziyuan',
      // link: '/app/ blogging/blogs',
      sort: 10,

      children: [
        {
          // 文件管理
          text: '',
          i18n: 'FileManager',
          acl: undefined,
          icon: 'iconfont icon-book',
          link: '/app/admin/file-manager',
          sort: 1,
        },
      ],
    },

    // 系统设置 11
    {
      text: '系统设置',
      i18n: 'SystemSettings',
      acl: 'Pages.WebSite.Module',
      icon: 'iconfont icon-xitong',
      // link: '/app/blogging/blogs',
      sort: 11,
      children: [
        {
          // 	轮播图广告的菜单按钮
          text: 'BannerAd',
          i18n: 'BannerAd',
          acl: 'Pages.BannerAd',
          icon: 'iconfont icon-dashboard',
          link: '/app/website/bannerads',
          sort: 1,
        },

        {
          // 	友情链接分类的菜单按钮
          text: 'BlogrollType',
          i18n: 'BlogrollType',
          acl: 'Pages.BlogrollType',
          icon: 'iconfont icon-dashboard',
          link: '/app/website/blogrollstype',
          sort: 2,
        },
        {
          // 	友情链接分类的菜单按钮
          text: 'Blogroll',
          i18n: 'Blogroll',
          acl: 'Pages.Blogroll',
          icon: 'iconfont icon-dashboard',
          link: '/app/website/blogrolls',
          sort: 3,
        },
        {
          // 	网站公告的菜单按钮
          text: 'WebSiteNotice',
          i18n: 'WebSiteNotice',
          acl: 'Pages.WebSiteNotice',
          icon: 'iconfont icon-dashboard',
          link: '/app/website/notices',
          sort: 4,
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
