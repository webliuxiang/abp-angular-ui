import AccountLayout from '@/account/account-layout.vue';
import {RouteConfig} from 'vue-router';

const accountRouting: RouteConfig[] = [
    {
        path: '/account',
        component: AccountLayout,
        redirect: '/account/login',
        children: [
            {
                path: 'login',
                name: 'login',
                component: () => import(/* webpackChunkName: "account" */ './login/login.vue')
            },
        ]
    },
];

export default accountRouting;
