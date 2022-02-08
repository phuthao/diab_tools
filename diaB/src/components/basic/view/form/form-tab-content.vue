<template>
  <div class="pb-5" data-wizard-type="step-content">
    <b-container fluid>
      <slot name="tab-title">
        <b-row align-h="start" class="mb-10 font-weight-bold text-dark">
          <b-col
            ><h2>{{ title }}</h2>
          </b-col>
        </b-row>
      </slot>
      <slot></slot>
    </b-container>
  </div>
</template>

<script>
export default {
  components: {},
  props: {
    title: {
      type: String,
    },
  },
  data() {
    return {};
  },
};
</script>
