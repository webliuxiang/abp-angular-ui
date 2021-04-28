import {LayoutBlockKeepAlive} from '@/layout';
import {RouteConfig} from 'vue-router';

const mainRouting: RouteConfig[] = [
    {
        path: 'main',
        component: LayoutBlockKeepAlive,
        meta: {title:  '系统' },
        redirect: '/app/main/dashboard',
        children: [
            {
                path: 'dashboard',
                name: 'dashboard',
                component: () => import(/* webpackChunkName: "main" */ './dashboard/dashboard.vue'),
                meta: {permission: 'Pages.Administration', title:  'Dashboard' }
            },
            {
                path: 'about',
                name: 'about',
                component: () => import(/* webpackChunkName: "main" */ './about/about.vue'),
                meta: {permission: 'Pages.Administration', title: 'About'}
            },
        ]
    },
];

export default mainRouting;
