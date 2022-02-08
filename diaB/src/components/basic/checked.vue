<template>
  <span class="svg-icon" :class="`svg-icon-${className}`">
    <inline-svg :src="`/media/svg/icons/Neolex/Basic/${isChecked}.svg`" />
  </span>
</template>

<script>
export default {
  name: 'CourseState',
  props: {
    checked: {
      type: [Boolean, Number],
      required: true,
    },
  },
  computed: {
    className() {
      if (typeof this.checked == 'boolean') {
        return this.checked ? 'success' : 'danger';
      } else {
        return this.checked == 1
          ? 'success'
          : this.checked == 2
          ? 'danger'
          : 'warning';
      }
    },
    isChecked() {
      if (typeof this.checked == 'boolean') {
        return this.checked ? 'check' : 'close';
      } else {
        return this.checked == 1
          ? 'check'
          : this.checked == 2
          ? 'close'
          : 'alert-octagon';
      }
    },
  },
};
</script>

<style></style>
