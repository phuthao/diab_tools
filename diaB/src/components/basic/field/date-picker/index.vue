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
      <date-picker
        input-class="form-control"
        v-bind="$attrs"
        v-on="$listeners"
        v-model="time"
        @open="openHandler"
        @input="valueChange"
        :placeholder="placeholder"
        :type="type"
        :format="format"
        :appendToBody="false"
        :value-type="valueType"
        :disabled="disabled"
      >
        <template v-slot:icon-calendar>
          <span class="svg-icon">
            <inline-svg
              src="/media/svg/icons/Neolex/Time/icon.svg"
              v-if="type === 'time'"
            />
            <inline-svg
              src="/media/svg/icons/Neolex/Time/calendar-dates.svg"
              v-else
            />
          </span>
        </template>
      </date-picker>
    </b-form-group>
  </div>
</template>

<script>
import $ from 'jquery';
import DatePicker from 'vue2-datepicker';
import 'vue2-datepicker/index.css';
import moment from 'moment-timezone';
export default {
  components: { DatePicker },
  props: {
    required: {
      type: Boolean,
      default: false,
    },
    value: {
      type: [Date, String, null],
      default: null,
    },
    valueType: {
      type: [String],
      default: 'format',
    },
    type: {
      type: String,
      default: 'date',
    },
    format: {
      type: String,
      default: 'YYYY-MM-DD',
    },
    name: {
      type: String,
      default: '',
    },
    label: {
      type: String,
      default: null,
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
    formater: {
      type: Object,
    },
  },
  data() {
    return {
      time: null,
      momentFormat: {
        // Date to String
        stringify: (date) => {
          return date ? moment(date).format('YYYY-MM-DD') : '';
        },
        // String to Date
        parse: (value) => {
          return value ? moment(value, 'YYYY-MM-DD').toDate() : null;
        },
      },
    };
  },
  watch: {
    value: {
      deep: true,
      handler: function (val) {
        if (val === undefined || val === null) {
          this.selectedValue = null;
          this.time = null;
        } else {
          this.time = val;
        }
      },
    },
  },
  computed: {},
  methods: {
    openHandler() {
      if (this.$attrs.range !== undefined) {
        return false;
      }

      setTimeout(() => {
        let ele = $(this.$el);
        let popup = $('.mx-datepicker-main.mx-datepicker-popup');
        if (!ele.offset() || !popup.offset()) {
          return false;
        }

        let elLeft = ele.offset().left;
        let popupLeft = popup.offset().left;
        let transform = Math.abs(elLeft - popupLeft);

        popup.css('transform', `translateX(-${transform}px)`);
      }, 0);
    },
    valueChange($event) {
      this.$emit('update:value', $event);
      this.$emit('input', $event);
    },
  },
  mounted() {},
};
</script>
<style lang="scss">
.mx-datepicker-popup {
  z-index: 1060 !important;
}
</style>
