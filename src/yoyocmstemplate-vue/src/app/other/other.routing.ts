import { LayoutBlock } from '@/layout';
import { RouteConfig } from 'vue-router';

const adminRouting: RouteConfig[] = [
    {
        path: 'other',
        meta: { title: '系统' },
        component: LayoutBlock,
        redirect: '/app/other/chat',
        children: [
            { path: 'chat', component: () => import(/* webpackChunkName: "admin" */ './chat.vue'), meta: { title: 'users' } },
        ]
    },
];

export default adminRouting;
