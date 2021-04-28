import {IRootState} from '@/shared/store/interfaces';
import {ActionTree, GetterTree, Module, MutationTree} from 'vuex';
import {ILayout, ILayoutState} from './interfaces';


/**
 * mutations
 */
const mutations: MutationTree<ILayoutState> = {
    set: (state, val) => {
        state.value = val;
    },
};

/**
 * actions
 */
const actions: ActionTree<ILayoutState, IRootState> = {};

/**
 * getters
 */
const getters: GetterTree<ILayoutState, IRootState> = {
    get: (state: ILayoutState): ILayout => {
        return state.value;
    },
};

/**
 * state
 */
const state: ILayoutState = {
    value: {
        collapsed: false,
        theme: 'dark',
        reuseTab: true,
        isPad: false,
    }
};

/**
 * 将 layout 对象交给 vuex 监控
 * @type
 */
const LayoutStore: Module<ILayoutState, IRootState> = {
    namespaced: true,
    state,
    getters,
    actions,
    mutations,
};

export default LayoutStore;
