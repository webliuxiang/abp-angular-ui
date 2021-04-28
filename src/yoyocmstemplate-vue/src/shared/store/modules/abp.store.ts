import {
    GetCurrentLoginInformationsOutput,
} from '@/shared/service-proxies';
import {IRootState} from '@/shared/store/interfaces';
import {ActionTree, GetterTree, Module, MutationTree} from 'vuex';

export interface IAbpState {
    abp: any;
    loginInfo: GetCurrentLoginInformationsOutput;
}


/**
 * mutations
 */
const mutations: MutationTree<IAbpState> = {
    set: (state, val) => {
        state.abp = val;
    },
    setLoginInfo: (state, val: GetCurrentLoginInformationsOutput) => {
        state.loginInfo = val;
    }
};

/**
 * actions
 */
const actions: ActionTree<IAbpState, IRootState> = {};

/**
 * getters
 */
const getters: GetterTree<IAbpState, IRootState> = {
    get: (state: IAbpState): any => {
        return state.abp;
    },
    getLoginInfo: (state: IAbpState): any => {
        return state.loginInfo;
    },
};

/**
 * state
 */
const state: IAbpState = {
    abp: {},
    loginInfo: undefined
};

/**
 * 将 abp 对象交给 vuex 监控
 * @type
 */
const AbpStore: Module<IAbpState, IRootState> = {
    namespaced: true,
    state,
    getters,
    actions,
    mutations,
};

export default AbpStore;
