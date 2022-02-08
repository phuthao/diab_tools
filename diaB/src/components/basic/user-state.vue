<template>
  <div class="user-state">
    <template v-if="isActivated">
      <div class="user-state__active">Active</div>
    </template>
    <template v-else>
      <div class="user-state__inactive">Inactive</div>
    </template>
  </div>
</template>

<style lang="scss" scoped>
.user-state {
  font-weight: normal;
  font-size: 12px;
  line-height: 15px;

  div {
    color: #ffffff;
    padding: 10px 23px;
    text-align: center;
    border-radius: 56px;
  }

  &__active {
    background: #21a567;
  }

  &__inactive {
    background: #ff5756;
  }
}
</style>

<script>
export default {
  props: {
    isActive: {
      type: Boolean,
      required: false,
    },
  },
  computed: {
    isActivated() {
      return this.isActive;
    },
  },
};
</script>
