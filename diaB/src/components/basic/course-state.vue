<template>
  <div class="course-state">
    <template v-if="isChecked">
      <p class="course-state course-state__active">Active</p>
    </template>
    <template v-else>
      <p class="course-state course-state__draft">Draft</p>
    </template>
  </div>
</template>

<style lang="scss" scoped>
.course-state {
  font-weight: normal;
  font-size: 12px;
  line-height: 15px;

  p {
    text-align: center;
    width: 85px;
    color: #ffffff;
    padding: 10px 23px;
    border-radius: 56px;
  }

  &__active {
    background: #21a567;
  }
  &__draft {
    background: #888c9f;
  }
  &__inactive {
    background: #ff5756;
  }
}
</style>

<script>
export default {
  props: {
    status: {
      type: Boolean,
      required: false,
    },
  },
  computed: {
    isChecked() {
      return this.status;
    },
  },
};
</script>
