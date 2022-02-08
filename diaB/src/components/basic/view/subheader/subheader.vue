<template>
  <div class="basic-subheader">
    <h5 class="subheader-content__title mb-0">{{ title }}</h5>
    <div class="subheader-content__actions">
      <slot name="actions"></slot>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.basic-subheader {
  height: 60px;
  background-color: #ffffff;
  border-top: 1px solid #ebedf3;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-align: center;
  -ms-flex-align: center;
  align-items: center;
  justify-content: space-between;
  padding: 4px 24px;
}
</style>

<script>
export default {
  props: {
    title: {
      type: String,
      default: '',
    },
  },
};
</script>
