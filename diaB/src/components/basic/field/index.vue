<template>
  <b-form-group
    :id="name"
    :description="description"
    :label="label"
    :label-for="name"
    :invalid-feedback="invalidFeedback"
    :valid-feedback="validFeedback"
  >
    <component
      :is="fieldType"
      :name="name"
      :disabled="disabled"
      :state="state"
      :value="value"
      :placeholder="placeholder"
    ></component>
  </b-form-group>
</template>

<script>
import FormInput from '@/components/basic/field/input/index';
import FormCheckBox from '@/components/basic/field/check-box/index';
import FormRadio from '@/components/basic/field/radio/index';
import FormTextArea from '@/components/basic/field/text-area/index';
import FormSelect from '@/components/basic/field/select/single-select';

export default {
  components: {
    FormInput,
    FormRadio,
    FormCheckBox,
    FormTextArea,
    FormSelect,
  },
  computed: {
    fieldType() {
      let listFieldType = {
        string: 'form-input',
        checkbox: 'form-checkbox',
        radio: 'form-radio',
        textarea: 'form-textarea',
        select: 'form-select',
      };
      return listFieldType[this.type];
    },
  },
  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
    value: {
      type: String,
      default: '',
    },
    inline: {
      type: Boolean,
      default: false,
    },
    state: {
      type: Boolean,
      default: null,
    },
    name: {
      type: String,
      default: 'string',
    },
    description: {
      type: String,
      default: null,
    },
    placeholder: {
      type: String,
      default: null,
    },
    label: {
      type: String,
      default: null,
    },
    type: {
      type: String,
      default: 'string',
    },
    invalidFeedback: {
      type: String,
      default: '',
    },
    validFeedback: {
      type: String,
      default: '',
    },
  },
  data() {
    return {};
  },
};
</script>
