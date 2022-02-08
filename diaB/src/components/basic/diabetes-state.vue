<template>
  <div class="diabetes-state">
    <template v-if="diabetesState === 1">Tiền tiểu đường</template>
    <template v-if="diabetesState === 2">Loại 1</template>
    <template v-if="diabetesState === 3">Loại 2</template>
  </div>
</template>

<script>
export default {
  props: {
    diabetesState: {
      type: Number,
      required: false,
      default: 0,
    },
  },
};
</script>
