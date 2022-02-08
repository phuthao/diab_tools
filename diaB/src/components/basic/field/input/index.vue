<template>
  <div>
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
      <b-form-input
        v-bind="$attrs"
        :id="`${name}-ID`"
        :name="`${name}`"
        :value="value"
        :state="state"
        :type="inputType"
        trim
        :placeholder="placeholder"
        class=""
        @change="valueChange"
        v-mask="mask"
        :disabled="disabled"
      ></b-form-input>
    </b-form-group>
  </div>
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
      type: [String, Number, null],
      default: null,
    },
    name: {
      type: String,
      default: '',
    },
    label: {
      type: String,
      default: null,
    },
    inputType: {
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
  },
  data() {
    return {};
  },
  computed: {
    mask() {
      let mask = null;
      if (this.type === 'phone_number') {
        mask = '+84 ### ### ###';
      }
      return mask;
    },
  },
  methods: {
    valueChange($event) {
      this.$emit('update:value', $event);
      this.$emit('input', $event);
    },
  },
  mounted() {},
};
</script>
