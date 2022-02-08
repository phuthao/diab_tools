<template>
  <div class="d-flex justify-content-between border-top pt-10">
    <div class="mr-2">
      <button
        v-if="previous_button"
        class="btn btn-light-primary font-weight-bold text-uppercase px-9 py-4 mr-4"
        data-wizard-type="action-prev"
      >
        Previous
      </button>
    </div>
    <div>
      <button
        v-if="submit_button"
        type="submit"
        class="btn btn-success font-weight-bold text-uppercase px-9 py-4 mr-4"
      >
        Submit
      </button>
      <button
        v-if="next_button"
        class="btn btn-primary font-weight-bold text-uppercase px-9 py-4"
        data-wizard-type="action-next"
      >
        Next Step
      </button>
    </div>
  </div>
</template>

<script>
export default {
  components: {},
  props: {
    submit_button: {
      type: Boolean,
      default: true,
    },
    next_button: {
      type: Boolean,
      default: false,
    },
    previous_button: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {};
  },
  methods: {},
};
</script>
