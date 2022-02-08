<template>
  <b-form-group
    v-model="checkedValue"
    :id="`${name}-group`"
    :label="label"
    :label-for="`${name}-ID`"
    :class="`${required ? 'required' : ''}`"
  >
    <b-row>
      <b-col :cols="12 - labelCols" class="col-form-label">
        <div :class="radioClass">
          <template v-for="(opt, idx) in listRadioOptions">
            <label
              :key="idx"
              class="radio radio-outline"
              :class="{
                'radio-square': radioStyle === 'square',
                'radio-rounded': radioStyle === 'rounded',
              }"
              :label-for="opt.value"
            >
              <input
                @input="radioInput($event, opt.value)"
                :checked="opt.checked"
                type="radio"
                :name="`radio-${name}`"
                :id="opt.value"
                :value="opt.value"
                :disabled="disableRadio"
              />
              <span></span>
              {{ opt.name }}
            </label>
            <slot> </slot>
          </template>
        </div>
      </b-col>
    </b-row>
    <b-row> </b-row>
  </b-form-group>
</template>

<script>
export default {
  props: {
    required: {
      type: Boolean,
      default: false,
    },
    disableRadio: {
      type: Boolean,
      default: null,
    },
    id: {
      type: String,
      default: null,
    },
    name: {
      type: String,
      default: null,
    },
    value: {
      type: [String, Boolean, Number],
      default: null,
    },
    description: {
      type: String,
      default: null,
    },
    label: {
      type: String,
      default: null,
    },
    labelCols: {
      type: Number,
      default: 2,
    },
    /** List Options must be in format
     * {
     * value: ..., Distinct value
     * name: ...., Display label
     * }
     * or Empty Array
     */
    options: {
      type: Array,
      default: () => [],
      validator: function (value) {
        if (value.length) {
          return !value.some((x) => x.value === undefined);
        } else {
          return true;
        }
      },
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
      default: true,
    },
    invalidFeedback: {
      type: String,
      default: null,
    },
    validFeedback: {
      type: String,
      default: null,
    },
    inline: {
      type: Boolean,
      default: false,
    },
    radioStyle: {
      type: String,
      default: null,
      validator: function (value) {
        if (value === null) {
          return true;
        } else {
          return ['square', 'rounded'].indexOf(value) !== -1;
        }
      },
    },
    isDeletedRadio: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      checkedValue: null,
    };
  },
  computed: {
    radioClass() {
      let classObject = {};
      if (this.inline) {
        classObject['radio-inline'] = true;
      } else {
        classObject['radio-list'] = true;
      }
      return classObject;
    },
    listRadioOptions() {
      let options = [];
      if (this.options) {
        options = this.options.map((x) => ({
          value: x.value,
          name: x.name,
          checked:
            this.checkedValue && this.checkedValue === x.value
              ? 'checked'
              : null,
        }));
      }

      if (this.isDeletedRadio) {
        options = [
          {
            value: false,
            name: 'Hoạt động',
            checked: !this.checkedValue ? 'checked' : null,
          },
          {
            value: true,
            name: 'Ngưng hoạt động',
            checked: this.checkedValue ? 'checked' : null,
          },
        ];
      }
      return options;
    },
  },
  watch: {
    value(val) {
      if (val) {
        this.checkedValue = val;
      }
    },
  },
  methods: {
    radioInput($event, value) {
      this.checkedValue = value;
      this.$emit('update:value', value);
      this.$emit('input', value);
    },
    isChecked(value) {
      return value === this.checkedValue;
    },
  },
  mounted() {
    if (this.value) {
      this.checkedValue = this.value;
    }
  },
};
</script>
