<template>
  <!--begin: Wizard Body-->
  <div class="card card-custom card-shadowless rounded-top-0">
    <div class="card-body p-0">
      <div class="row justify-content-center pb-10">
        <div class="container-fluid">
          <!--begin: Wizard Form-->
          <b-form class="form mt-0 mt-lg-10" id="kt_form" @submit="onSubmit">
            <slot></slot>

            <FormAction
              v-bind="{
                submit_button: submit_button,
                next_button: next_button,
                previous_button: previous_button,
              }"
            ></FormAction>
            <!--end: Wizard Actions -->
          </b-form>
          <!--end: Wizard Form-->
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import FormAction from '@/components/basic/view/form/form-action';
export default {
  components: {
    FormAction,
  },
  props: {
    submit_button: {
      type: Boolean,
      default: true,
    },
    submit: {
      type: Function,
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
    return {
      input: 'null',
    };
  },
  methods: {
    onSubmit($event) {
      $event.preventDefault();
      this.$emit('submit');
    },
  },
};
</script>
