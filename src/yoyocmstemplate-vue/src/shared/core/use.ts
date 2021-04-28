import Vue from 'vue';

import Viser from 'viser-vue';
import VueClipboard from 'vue-clipboard2';
import VueCropper from 'vue-cropper';
import VueStorage from 'vue-ls';

import '../antd/import-antd';



Vue.use(Viser);
Vue.use(VueClipboard);
Vue.use(VueCropper);
Vue.use(VueStorage, {
    namespace: 'pro__', // key prefix
    name: 'ls', // name variable Vue.[ls] or this.[$ls],
    storage: 'local' // storage name session, local, memory
});

