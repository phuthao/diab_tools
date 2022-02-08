<template>
  <b-form-group
    :id="`${name}-group`"
    :description="description"
    :label="label"
    :label-for="`${name}-ID`"
    :invalid-feedback="invalidFeedback"
    :valid-feedback="validFeedback"
    :state="state"
    :class="`${required ? 'required' : ''}`"
  >
    <b-form-textarea
      :id="`${name}-ID`"
      :value="value"
      :state="state"
      :placeholder="placeholder"
      class=""
      @input="$emit('update:value', $event)"
      :disabled="disabled"
      :rows="rows"
      :max-rows="maxRows"
    ></b-form-textarea>
  </b-form-group>
</template>

<script>
export default {
  // computed: {
  // },
  props: {
    required: {
      type: Boolean,
      default: false,
    },
    value: {
      type: String,
      default: '',
    },
    name: {
      type: String,
      default: '',
    },
    label: {
      type: String,
      default: null,
    },
    type: {
      type: String,
      default: 'text',
    },
    placeholder: {
      type: String,
      default: null,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    state: {
      type: Boolean,
      default: null,
    },
    invalidFeedback: {
      type: String,
      default: null,
    },
    validFeedback: {
      type: String,
      default: null,
    },
    description: {
      type: String,
      default: null,
    },
    rows: {
      type: Number,
      default: 6,
    },
    maxRows: {
      type: Number,
      default: null,
    },
  },
  data() {
    return {};
  },
};
</script>
