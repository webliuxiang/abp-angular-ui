import {accountRouting} from '@/account';
import {appRouting} from '@/app';
import Vue from 'vue';
import VueRouter from 'vue-router';


Vue.use(VueRouter);

const rootRooting = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes: [
        ...accountRouting,
        ...appRouting,
    ]
});

export default rootRooting;
