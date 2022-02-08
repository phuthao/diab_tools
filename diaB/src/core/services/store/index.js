import Vue from 'vue';
import Vuex from 'vuex';

import auth from './auth.module';
import htmlClass from './htmlclass.module';
import config from './config.module';
import breadcrumbs from './breadcrumbs.module';

import context from './context.module';
import setting from './setting.module';
Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    auth,
    htmlClass,
    config,
    breadcrumbs,
    context,
    setting,
  },
});
