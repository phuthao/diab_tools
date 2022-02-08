<template>
  <div>
    <vue-numeric
      :currency="currency"
      currency-symbol-position="suffix"
      separator="."
      :disabled="disabled"
      class="form-control text-right"
      :class="state === false ? 'error' : ''"
      :value="value"
      @input="$emit('update:value', $event)"
      :state="state"
    ></vue-numeric>
  </div>
</template>

<script>
import VueNumeric from 'vue-numeric';

export default {
  components: { VueNumeric },
  props: [
    'value',
    'disabled',
    'separator',
    'currency',
    'currency-symbol-position',
    'state',
  ],
  data() {
    return {};
  },
};
</script>
<style scoped>
.error {
  border: 1px solid red;
}
</style>
