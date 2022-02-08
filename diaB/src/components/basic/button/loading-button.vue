<template>
  <button
    @click.prevent="load()"
    class="spinner-right spinner-white"
    type="button"
    :class="{
      spinner: loading,
    }"
    :disabled="loading"
  >
    <slot></slot>
  </button>
</template>

<script>
export default {
  props: ['handler'],
  data() {
    return {
      loading: false,
    };
  },
  methods: {
    load() {
      if (this.loading === true) {
        return;
      }
      this.loading = true;

      this.handler().finally(() => {
        this.loading = false;
      });
    },
  },
};
</script>
