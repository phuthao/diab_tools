<template>
  <b-button
    class="ml-2"
    @click="$emit('click')"
    :type="type"
    :variant="btnVariant"
    >{{ btnLabel }}</b-button
  >
</template>

<script>
export default {
  props: {
    btnVariant: {
      type: String,
      default: null,
    },
    btnLabel: {
      type: String,
      default: null,
    },
    type: {
      type: String,
      default: 'submit',
    },
  },
};
</script>
