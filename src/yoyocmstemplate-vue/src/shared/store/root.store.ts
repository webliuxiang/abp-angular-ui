import Vue from 'vue';
import Vuex from 'vuex';
import { IRootState } from './interfaces';

import getters from './getters';

import {
  abpStore,
  layoutStore
} from './modules';

Vue.use(Vuex);

export default new Vuex.Store<IRootState>({
  state: {
  },
  mutations: {
  },
  actions: {
  },
  modules: {
    layoutStore,
    abpStore
  },
  getters
});
