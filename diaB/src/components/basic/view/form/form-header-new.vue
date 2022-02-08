<template>
  <div class="card card-custom card-transparent">
    <div class="card-body p-0">
      <!--begin: Wizard-->
      <div
        class="wizard wizard-4"
        id="kt_wizard_v4"
        data-wizard-state="step-first"
        data-wizard-clickable="true"
      >
        <!--begin: Wizard Nav-->
        <div class="wizard-nav">
          <div class="wizard-steps">
            <div
              class="wizard-step"
              data-wizard-type="step"
              data-wizard-state="current"
            >
              <div class="wizard-wrapper">
                <div class="wizard-number">1</div>
                <div class="wizard-label">
                  <div class="wizard-title">Add Account</div>
                  <div class="wizard-desc">Create Custom Account</div>
                </div>
              </div>
            </div>
            <div class="wizard-step" data-wizard-type="step">
              <div class="wizard-wrapper">
                <div class="wizard-number">2</div>
                <div class="wizard-label">
                  <div class="wizard-title">Your Address</div>
                  <div class="wizard-desc">Setup Your Address</div>
                </div>
              </div>
            </div>
            <div class="wizard-step" data-wizard-type="step">
              <div class="wizard-wrapper">
                <div class="wizard-number">3</div>
                <div class="wizard-label">
                  <div class="wizard-title">Make Payment</div>
                  <div class="wizard-desc">Add Payment Options</div>
                </div>
              </div>
            </div>
            <div class="wizard-step" data-wizard-type="step">
              <div class="wizard-wrapper">
                <div class="wizard-number">4</div>
                <div class="wizard-label">
                  <div class="wizard-title">Completed</div>
                  <div class="wizard-desc">Review and Submit</div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <!--end: Wizard Nav -->

        <!--begin: Wizard Body-->
        <!--end: Wizard Bpdy-->
      </div>
      <!--end: Wizard-->
    </div>
  </div>
</template>

<style lang="scss">
@import '@/assets/sass/pages/wizard/wizard-4.scss';
</style>

<script>
import KTUtil from '@/assets/js/components/util';
import KTWizard from '@/assets/js/components/wizard';

export default {
  name: 'Wizard-4',
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
  methods: {},
};
</script>
