import { LayoutBlock } from '@/layout';
import { RouteConfig } from 'vue-router';

const adminRouting: RouteConfig[] = [
    {
        path: 'wechat',
        meta: { title: '系统' },
        component: LayoutBlock,
        redirect: '/app/wechat/wechat-app-config',
        children: [
            { path: 'wechat-app-config', component: () => import(/* webpackChunkName: "admin" */ './wechat-app-config.vue'), meta: { title: 'users' } },
        ]
    },
];

export default adminRouting;
