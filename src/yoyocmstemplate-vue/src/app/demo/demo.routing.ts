import { LayoutBlock } from '@/layout';
import { RouteConfig } from 'vue-router';

const adminRouting: RouteConfig[] = [
    {
        path: 'demo',
        meta: { title: '系统' },
        component: LayoutBlock,
        redirect: '/app/demo/demoui',
        children: [
            { path: 'demoui', component: () => import(/* webpackChunkName: "admin" */ './demoui.vue'), meta: { title: 'users' } },
        ]
    },
];

export default adminRouting;
