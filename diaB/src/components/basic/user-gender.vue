<template>
  <div class="user-gender">
    <span>{{ gender === 0 ? 'Nữ' : 'Nam' }}</span>
  </div>
</template>

<script>
export default {
  props: {
    gender: {
      type: [Number, String],
      required: false,
      default: 0,
    },
  },
};
</script>
