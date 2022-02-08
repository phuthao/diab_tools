import Vue from 'vue';

import State from '@/components/basic/state';
Vue.component('state', State);
import DiabetesState from '@/components/basic/diabetes-state';
Vue.component('DiabetesState', DiabetesState);
import UserState from '@/components/basic/user-state';
Vue.component('UserState', UserState);
import UserGender from '@/components/basic/user-gender';
Vue.component('UserGender', UserGender);
import CommunicationState from '@/components/basic/communication-state';
Vue.component('CommunicationState', CommunicationState);
import CourseState from '@/components/basic/course-state';
Vue.component('CourseState', CourseState);
import Checked from '@/components/basic/checked';
Vue.component('Checked', Checked);

import Avatar from '@/components/avatar';
Vue.component('Avatar', Avatar);

import FormTabContent from '@/components/basic/view/form/form-tab-content';
Vue.component('FormTabContent', FormTabContent);

import TemplateTable from '@/components/table/template-table';
Vue.component('TemplateTable', TemplateTable);

import TemplateSelect from '@/components/basic/field/select/select2';
Vue.component('TemplateSelect', TemplateSelect);

import Action from '@/components/table/action';
Vue.component('Action', Action);
import ActionDropdown from '@/components/table/actionDropdown';
Vue.component('ActionDropdown', ActionDropdown);

import Datepicker from '@/components/basic/datepicker';
Vue.component('datepicker', Datepicker);

import BasicForm from '@/components/basic/view/form/form';
Vue.component('BasicForm', BasicForm);

import LoadingButton from '@/components/basic/button/loading-button';
Vue.component('LoadingButton', LoadingButton);

import Wrapper from '@/components/Shared/Wrapper';
Vue.component('Wrapper', Wrapper);

import ImageInput from '@/components/image-input';
Vue.component('ImageInput', ImageInput);

// BASIC VIEW: START
import BasicFormTab from '@/components/basic/view/form-tab';
Vue.component('BasicFormTab', BasicFormTab);

import BasicTab from '@/components/basic/view/form-tab/tab';
Vue.component('BasicTab', BasicTab);

import BasicSubheader from '@/components/basic/view/subheader/subheader';
Vue.component('BasicSubheader', BasicSubheader);
// BASIC VIEW: END

// BASIC FIELD: START
import BasicInput from '@/components/basic/field/input/index';
Vue.component('BasicInput', BasicInput);

import BasicRadio from '@/components/basic/field/radio';
Vue.component('BasicRadio', BasicRadio);

import BasicTextArea from '@/components/basic/field/text-area/index';
Vue.component('BasicTextArea', BasicTextArea);

import BasicTextEditors from '@/components/basic/field/text-editors/index';
Vue.component('BasicTextEditors', BasicTextEditors);

import BasicSelect from '@/components/basic/field/select/vue-multiselect';
Vue.component('BasicSelect', BasicSelect);

import BasicDatePicker from '@/components/basic/field/date-picker/index';
Vue.component('BasicDatePicker', BasicDatePicker);

import BasicCheckBox from '@/components/basic/field/check-box/index';
Vue.component('BasicCheckBox', BasicCheckBox);
// BASIC FIELD: END
