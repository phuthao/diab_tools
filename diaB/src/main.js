import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from '@/core/services/store';
import ApiService from '@/core/services/api.service';
import MgrService from '@/core/services/security.service';
import MockService from '@/core/mock/mock.service';
// import { VERIFY_AUTH } from "@/core/services/store/auth.module";
import { RESET_LAYOUT_CONFIG } from '@/core/services/store/config.module';

Vue.config.productionTip = false;

// Global 3rd party plugins
import 'popper.js';
import 'tooltip.js';
import PerfectScrollbar from 'perfect-scrollbar';
window.PerfectScrollbar = PerfectScrollbar;
import ClipboardJS from 'clipboard';
window.ClipboardJS = ClipboardJS;

// Vue 3rd party plugins
import i18n from '@/core/plugins/vue-i18n';
import vuetify from '@/core/plugins/vuetify';
import '@/core/plugins/portal-vue';
import '@/core/plugins/bootstrap-vue';
import '@/core/plugins/perfect-scrollbar';
import '@/core/plugins/highlight-js';
import '@/core/plugins/inline-svg';
import '@/core/plugins/apexcharts';
import '@/core/plugins/metronic';
import '@mdi/font/css/materialdesignicons.css';
import '@/core/plugins/vue-swal';
import '@/core/plugins/vue-toastr';
import '@/core/plugins/vee-validate';
import 'vue-select/dist/vue-select.css';
import '@/core/plugins/moment';
import '@/core/plugins/vue-mask';
// Global Components
import '@/components/global';

// Custom Directive
import '@/directive';

Vue.prototype.$api = new ApiService();
Vue.prototype.$mgr = new MgrService();

// Remove this to disable mock API
MockService.init();

router.beforeEach(async (to, from, next) => {
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth);

  if (requiresAuth) {
    store.dispatch(RESET_LAYOUT_CONFIG);
    next();
    // Scroll page to top on every route change
    setTimeout(() => {
      window.scrollTo(0, 0);
    }, 100);
  } else {
    let mgs = new MgrService();
    await mgs.signOut();
  }
  // reset config to initial state
});

new Vue({
  router,
  store,
  i18n,
  vuetify,
  render: (h) => h(App),
}).$mount('#app');
