import { AppMenus } from '@/abpPro/AppMenus';
import { PageHeader } from '@/components'; // 全局组件PageHeader
import { preloaderFinished } from '@/shared/utils';
import '@babel/polyfill'; // ie polyfill
import moment from 'moment';
import Vue from 'vue';
import { AppPreBootstrap } from './AppPreBootstrap'; // 预启动器
import root from './root.vue'; // 主容器组件
import { rootRouting } from './router'; // 全局路由
import { appSessionService, permissionService } from './shared/abp';
import { authRouteGuard } from './shared/auth'; // 路由守卫
import { layoutService, menuService, reuseTabService } from './shared/common';
import './shared/core/use';
import { UserNotificationHelper } from './shared/helpers';
import { localizationService } from './shared/i18n';
import { rootStore } from './shared/store'; // 全局store
import './shared/utils/use-pipes'; // 应用所有管道(filter)
import './styles/index.less'; // 全局样式注册
// import './registerServiceWorker'; // serviceWorker


Vue.config.productionTip = false;

Vue.use(PageHeader);

/**
 * 过滤器
 */
Vue.filter('moment', (value, formatString) => {
    formatString = formatString || 'YYYY-MM-DD HH:mm:ss';
    return moment(value).format(formatString);
});

AppPreBootstrap.run(() => {

    // 初始化会话信息
    appSessionService.init().then((res) => {


        // 初始化路由守卫
        authRouteGuard.init(rootRouting);
        // 初始化复用标签
        reuseTabService.init(rootRouting, localizationService, menuService);
        // 初始化菜单服务
        menuService.initService(localizationService, permissionService);
        // 初始化通知服务
        UserNotificationHelper.init();
        // 启用复用标签
        layoutService.data.reuseTab = true;
        // 初始化菜单
        menuService.add(AppMenus.Menus);

        new Vue({
            router: rootRouting,
            store: rootStore,
            render: (h) => h(root),
            mounted() {
                preloaderFinished();
            }
        }).$mount('#app');

    });

});


