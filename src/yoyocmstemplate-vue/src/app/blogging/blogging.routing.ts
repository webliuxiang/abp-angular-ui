import { LayoutBlock } from '@/layout';
import { RouteConfig } from 'vue-router';

const bloggingRouting: RouteConfig[] = [
    {
        path: 'blogging',
        meta: { title: '系统' },
        component: LayoutBlock,
        redirect: '/app/blogging/blogs',
        children: [
            { path: 'blogs', component: () => import('./blogs/blogs.vue'), meta: { title: 'blogs' } },
            { path: 'posts', component: () => import('./posts/posts.vue'), meta: { title: 'posts' } },
            // { path: 'create-or-edit-posts', component: () => import('./posts/create-or-edit-posts.vue'), meta: { title: "create-or-edit-post" } },
            { path: 'comments', component: () => import('./comments/comments.vue'), meta: { title: 'comments' } },
            { path: 'tagging', component: () => import('./tagging/tagging.vue'), meta: { title: 'tagging' } },
        ]
    },
];

export default bloggingRouting;
