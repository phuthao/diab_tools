<template>
  <div class="card card-custom card-transparent">
    <div class="card-body p-0">
      <!--begin: Wizard-->
      <div
        class="wizard"
        id="kt_wizard_v4"
        data-wizard-state="step-first"
        data-wizard-clickable="true"
      >
        <FormHeader :tabs="tabs"></FormHeader>
        <FormBody
          @submit="$emit('submit')"
          v-bind="{
            tabs: tabs,
            submit_button: submit_button,
            next_button: next_button,
            previous_button: previous_button,
          }"
        >
          <slot></slot>
        </FormBody>
      </div>
    </div>
    <b-overlay :show="loading" no-wrap></b-overlay>
  </div>
</template>
<style lang="scss">
@import '@/assets/sass/pages/wizard/wizard-4.scss';
</style>
<script>
import { mapGetters } from 'vuex';
import FormHeader from '@/components/basic/view/form/form-header';
import FormBody from '@/components/basic/view/form/form-body';
import KTUtil from '@/assets/js/components/util';
import KTWizard from '@/assets/js/components/wizard';
export default {
  components: {
    FormHeader,
    FormBody,
  },
  computed: {
    ...mapGetters('context', ['loading']),
    disabled: function () {
      return this.formType === 'detail';
    },
  },
  props: {
    tabs: {
      type: Array,
      default: () => [],
    },
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
    return {
      currentTaab: null,
    };
  },
  mounted() {
    // Initialize form wizard
    const wizard = new KTWizard('kt_wizard_v4', {
      startStep: 1, // initial active step number
      clickableSteps: true, // allow step clicking
    });
    // Validation before going to next page
    wizard.on('beforeNext', function (/*wizardObj*/) {
      // validate the form and use below function to stop the wizard's step
      // wizardObj.stop();
    });
    // Change event
    wizard.on('change', function (/*wizardObj*/) {
      setTimeout(() => {
        KTUtil.scrollTop();
      }, 500);
    });
  },
};
</script>
