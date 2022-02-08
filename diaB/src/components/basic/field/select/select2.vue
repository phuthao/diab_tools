<template>
  <div class="template-select-2">
    <select2
      :id="id"
      :options="optionsList"
      :value="value"
      :settings="settings"
      :disabled="disabled"
      :placeholder="placeholder"
      @change="$emit('input', $event)"
    ></select2>
  </div>
</template>

<script>
import Select2 from 'v-select2-component';

export default {
  components: {
    Select2,
  },
  props: ['id', 'value', 'options', 'settings', 'disabled', 'placeholder'],
  computed: {
    optionsList() {
      return this.settings && this.settings.multiple
        ? this.options
        : [
            {
              id: '',
            },
            ...this.options,
          ];
    },
  },
};
</script>

<style lang="scss">
.template-select-2 {
  .select2-container {
    width: 100% !important;
  }
  .select2-container--focus .select2-selection {
    border: 1px solid #69b3ff !important;
  }
  .select2-selection {
    border: 1px solid #e4e6ef;
    min-height: 38px !important;
  }
  .select2-selection__arrow {
    min-height: 38px !important;
  }
  .select2-selection__rendered {
    padding: 4px 1rem !important;
  }
  .select2-selection--multiple .select2-selection__rendered {
    line-height: initial !important;
  }

  &.has-error .select2-selection {
    border: 1px solid #f64e60 !important;
  }
}
</style>
