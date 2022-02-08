import Vue from 'vue';
import VeeValidate, { Validator } from 'vee-validate';
import vi from 'vee-validate/dist/locale/vi';
Validator.localize('vi', vi);
Vue.use(VeeValidate, {
  inject: true,
  fieldsBagName: 'veeFields',
  errorBagName: 'errors',
  events: 'change|blur|select',
});
