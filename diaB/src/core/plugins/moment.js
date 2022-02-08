import Vue from 'vue';
import VueMoment from 'vue-moment';
import moment from 'moment-timezone';
moment.tz.setDefault('Asia/Ho_Chi_Minh');

Vue.use(VueMoment, {
  moment,
});
