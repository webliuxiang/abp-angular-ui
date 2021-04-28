import { LayoutBlock } from '@/layout';
import { RouteConfig } from 'vue-router';

const websiteRouting: RouteConfig[] = [
    {
        path: 'website',
        meta: { title: '系统' },
        component: LayoutBlock,
        redirect: '/app/website/bannerads',
        children: [
            { path: 'bannerads', component: () => import('./bannerads/bannerads.vue'), meta: { title: 'BannerAd' } },
            { path: 'blogrollstype', component: () => import('./blogrollstype/blogrollstype.vue'), meta: { title: 'BlogrollType' } },
            { path: 'blogrolls', component: () => import('./blogrolls/blogrolls.vue'), meta: { title: 'Blogroll' } },
            { path: 'notices', component: () => import('./notices/notices.vue'), meta: { title: 'WebSiteNotice' } },
        ]
    },
];

export default websiteRouting;
