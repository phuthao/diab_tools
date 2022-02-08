<template>
  <date-picker v-bind="$attrs" v-on="$listeners" @open="openHandler">
    <template v-slot:icon-calendar>
      <i class="ki ki-calendar"></i>
    </template>
  </date-picker>
</template>

<script>
import $ from 'jquery';
import DatePicker from 'vue2-datepicker';
import 'vue2-datepicker/index.css';

export default {
  components: { DatePicker },
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
  },
};
</script>

<style>
.mx-datepicker {
  width: 100%;
}

.mx-input-wrapper input {
  border: 1px solid #e4e6ef;
  height: 38px;
}

.mx-table-date .today {
  color: #2a90e9;
  background: #dbeff9;
  border-radius: 20%;
}

.mx-table-date td,
.mx-table-date th {
  height: 32px;
  font-size: 12px;
  border-radius: 20%;
}

.mx-input {
  font-size: 13px;
}

.mx-datepicker.is-invalid .mx-input-wrapper input {
  border-color: #f64e60;
}
.mx-datepicker.is-valid .mx-input-wrapper input {
  border-color: #1bc5bd;
}
</style>
