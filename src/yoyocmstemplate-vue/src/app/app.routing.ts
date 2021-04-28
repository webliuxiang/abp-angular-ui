import { LayoutDefault } from '@/layout';
import { RouteConfig } from 'vue-router';
import { adminRouting } from './admin';
import { bloggingRouting } from './blogging';
import { demoRouting } from './demo';
import { mainRouting } from './main';
import { otherRouting } from './other';
import { websiteRouting } from './website';
import { wechatRouting } from './wechat';

const appRouting: RouteConfig[] = [
    {
        path: '/app',
        component: LayoutDefault,
        redirect: '/app/main',
        children: [
            ...otherRouting,
            ...wechatRouting,
            ...mainRouting,
            ...demoRouting,
            ...adminRouting,
            ...websiteRouting,
            ...bloggingRouting,
        ]
    },
];

export default appRouting;
