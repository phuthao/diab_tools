<template>
  <div>
    <template v-if="isDeleted">
      <span
        class="label label-lg font-weight-bolder label-light-danger label-inline"
        >Ngưng hoạt động</span
      >
    </template>
    <template v-else>
      <span
        class="label label-lg font-weight-bolder label-light-success label-inline"
        >Hoạt động</span
      >
    </template>
  </div>
</template>

<script>
export default {
  props: {
    isDeleted: {
      type: Boolean,
      required: true,
    },
  },
};
</script>
