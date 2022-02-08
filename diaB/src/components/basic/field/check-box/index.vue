<template>
  <b-form-group
    :label="label"
    class="mb-0"
    :invalid-feedback="invalidFeedback"
    :valid-feedback="validFeedback"
    :state="state"
    :class="`${required ? 'required' : ''}`"
  >
    <b-form-checkbox-group
      :class="`${required ? 'required' : ''}`"
      :stacked="stacked"
      :id="name"
      :value="value"
      :options="options"
      :name="name"
      :state="state"
      :invalid-feedback="invalidFeedback"
      :valid-feedback="validFeedback"
      @change="valueChange"
    ></b-form-checkbox-group>
  </b-form-group>
</template>

<script>
export default {
  // computed: {
  // },
  props: {
    value: {
      type: Array,
      default: () => [],
    },
    options: {
      type: Array,
      default: () => [
        { text: 'Option A', value: 'a' },
        { text: 'Option B', value: 'b' },
      ],
    },
    required: {
      type: Boolean,
      default: false,
    },
    name: {
      type: String,
      default: '',
    },
    label: {
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
    stacked: {
      type: Boolean,
      default: false,
    },
    invalidFeedback: {
      type: String,
      default: null,
    },
    validFeedback: {
      type: String,
      default: null,
    },
  },
  methods: {
    valueChange($event) {
      this.$emit('update:value', $event);
      this.$emit('input', $event);
    },
  },
};
</script>
