import { LayoutBlock } from '@/layout';
import { RouteConfig } from 'vue-router';

const adminRouting: RouteConfig[] = [
    {
        path: 'admin',
        meta: {title:  '系统' },
        component: LayoutBlock,
        redirect: '/app/admin/user',
        children: [
            { path: 'users', component: () => import(/* webpackChunkName: "admin" */ './users/users.vue'), meta: {title:  'users' } },
            { path: 'roles', component: () => import(/* webpackChunkName: "admin" */ './roles/roles.vue') },
            { path: 'auditLogs', component: () => import(/* webpackChunkName: "admin" */ './audit-logs/audit-logs.vue') },
            { path: 'file-manager', component: () => import(/* webpackChunkName: "admin" */ './file-manager/file-manager.vue') },
            {
                path: 'maintenance',
                component: () => import(/* webpackChunkName: "admin" */ './maintenance/maintenance.vue')
            },
            {
                path: 'host-settings',
                component: () => import(/* webpackChunkName: "admin" */ './host-settings/host-settings.vue')
            },
            { path: 'editions', component: () => import(/* webpackChunkName: "admin" */ './editions/editions.vue') },
            { path: 'languages', component: () => import(/* webpackChunkName: "admin" */ './languages/languages.vue') },
            {
                path: 'languagetexts',
                component: () => import(/* webpackChunkName: "admin" */ './language-texts/language-texts.vue')
            },
            { path: 'tenants', component: () => import(/* webpackChunkName: "admin" */ './tenants/tenants.vue'), meta: {title:  'tenants' }  },

            {
                path: 'organization-units',
                component: () => import(/* webpackChunkName: "admin" */ './organization-units/organization-units.vue')
            },
            {
                path: 'subscription-management',
                component: () => import(/* webpackChunkName: "admin" */ './subscription-management/subscription-management.vue')
            },
            {
                path: 'tenant-settings',
                component: () => import(/* webpackChunkName: "admin" */ './tenant-settings/tenant-settings.vue')
            },
            // {
            //     path: 'hangfire',
            //     component: () => import(/* webpackChunkName: "admin" */ './roles/roles.vue'),
            // }
        ]
    },
];

export default adminRouting;
