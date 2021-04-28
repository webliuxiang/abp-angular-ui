import { Menu } from '@/shared/common';

/**
 * 全局的左侧边栏的菜单导航配置信息
 */
export class AppMenus {
    public static Menus: Menu[] = [
        {
            // 工作台
            text: '',
            i18n: 'Dashboard',
            acl: undefined,
            icon: 'anticon anticon-dashboard',
            link: '/app/main/dashboard'
        },
        {
            // 租户
            text: '',
            i18n: 'Tenants',
            acl: 'Pages.Tenants',
            icon: 'anticon anticon-dashboard',
            link: '/app/admin/tenants'
        },
        {
            // 版本
            text: '',
            i18n: 'Editions',
            acl: 'Pages.Editions.Query',
            icon: 'anticon anticon-dashboard',
            link: '/app/admin/editions'
        },
        {
            // 管理
            text: '',
            i18n: 'Administration',
            acl: 'Pages.Administration',
            icon: 'anticon anticon-appstore',
            children: [
                {
                    // 组织机构
                    text: '',
                    i18n: 'OrganizationUnits',
                    acl: 'Pages.Administration.OrganizationUnits',
                    icon: 'anticon anticon-team',
                    link: '/app/admin/organization-units'
                },
                {
                    // 角色
                    text: '',
                    i18n: 'Roles',
                    acl: 'Pages.Administration.Roles',
                    icon: 'anticon anticon-safety',
                    link: '/app/admin/roles'
                },
                {
                    // 用户
                    text: '',
                    i18n: 'Users',
                    acl: 'Pages.Administration.Users',
                    icon: 'anticon anticon-user',
                    link: '/app/admin/users'
                },
                {
                    // 语言
                    text: '',
                    i18n: 'Languages',
                    acl: 'Pages.Administration.Languages',
                    icon: 'anticon anticon-global',
                    link: '/app/admin/languages'
                },
                {
                    // 审计日志
                    text: '',
                    i18n: 'AuditLogs',
                    acl: 'Pages.Administration.AuditLogs',
                    icon: 'anticon anticon-book',
                    link: '/app/admin/auditLogs'
                },
                {
                    // 文件管理
                    text: '',
                    i18n: 'FileManager',
                    acl: undefined,
                    icon: 'anticon anticon-book',
                    link: '/app/admin/file-manager',
                },
                {
                    // 宿主机器设置/维护
                    text: '',
                    i18n: 'Maintenance',
                    acl: 'Pages.Administration.Host.Maintenance',
                    icon: 'anticon anticon-setting',
                    link: '/app/admin/maintenance'
                },
                // {
                //     // 租户设置
                //     text: '',
                //     i18n: 'Settings',
                //     acl: 'Pages.Administration.Tenant.Settings',
                //     icon: 'anticon anticon-setting',
                //     link: '/app/admin/tenant-settings'
                // },
                {
                    // 宿主设置
                    text: '',
                    i18n: 'Settings',
                    acl: 'Pages.Administration.Host.Settings',
                    icon: 'anticon anticon-setting',
                    link: '/app/admin/host-settings'
                },
                // // Hangfire管理
                // {
                //     text: '',
                //     i18n: 'HangfireSchedule',
                //     acl: 'Pages.Administration.HangfireDashboard',
                //     link: '/app/admin/hangfire',
                //     icon: { type: 'icon', value: 'setting' }
                // }
            ]
        },
        {
            // Wechat 模块
            text: '',
            i18n: 'WechatManagement',
            icon: 'anticon anticon-info-circle',
            children: [
                //
                {
                    text: undefined,
                    i18n: 'WechatAppConfig',
                    acl: 'Pages.WechatAppConfig',
                    icon: 'anticon anticon-dashboard',
                    link: '/app/wechat/wechat-app-config'
                },
            ]
        },
        // {
        //     text: 'Books',
        //     i18n: 'Books',
        //     acl: 'Pages.Books.Book',
        //     icon: 'anticon anticon-dashboard',
        //     link: '/app/main/books/book'
        // },
        {
            // 演示ui
            text: '',
            i18n: 'DemoManagement',
            icon: 'anticon anticon-info-circle',
            link: '/app/demo/demoui',
        },
        {
            // 网站设置
            text: '',
            i18n: 'WebSiteSettingModule',
            icon: 'iconfont icon-dashboard',
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
            ]
        },
        {
            // 	 博客的菜单按钮
            text: '',
            i18n: 'Blog',
            acl: 'Pages.Blog',
            icon: 'iconfont icon-dashboard',
            // link: '/app/blogging/blogs',
            sort: 6,
            children: [
                //
                {
                    // 组织机构
                    text: '',
                    i18n: 'Blog',
                    acl: 'Pages.Blog',
                    icon: 'iconfont icon-team',
                    link: '/app/blogging/blogs',
                    sort: 1,
                },
                {
                    // 	文章的菜单按钮
                    text: 'Post',
                    i18n: 'Post',
                    acl: 'Pages.Post',
                    icon: 'iconfont icon-dashboard',
                    link: '/app/blogging/posts',
                    sort: 2,
                },
                // ToDo 先不做
                // {
                //     // 	评论的菜单按钮

                //     text: 'Comment',
                //     i18n: 'Comment',
                //     acl: 'Pages.Comment',
                //     icon: 'iconfont icon-dashboard',
                //     link: '/app/blogging/comments',
                //     sort: 99,
                // },
                {
                    // 	标签的菜单按钮
                    text: 'Tag',
                    i18n: 'Tag',
                    acl: 'Pages.Tag',
                    icon: 'iconfont icon-dashboard',
                    link: '/app/blogging/tagging',
                    sort: 99,
                },
            ],
        },
        {
            // 关于我们
            text: '',
            i18n: 'About',
            icon: 'anticon anticon-info-circle',
            link: '/app/main/about'
        },
        {
            // 实时聊天
            text: '实时聊天',
            icon: 'anticon anticon-info-circle',
            link: '/app/other/chat',
            sort: 7,
        },
    ];
}
