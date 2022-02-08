<template>
  <div
    class="wizard wizard-4"
    id="kt_wizard_v4"
    data-wizard-state="step-first"
    data-wizard-clickable="true"
  >
    <!--begin: Wizard Nav-->
    <template v-if="tabs && tabs.length">
      <div class="wizard-nav">
        <div class="wizard-steps">
          <div
            class="wizard-step mr-2"
            data-wizard-type="step"
            data-wizard-state="current"
          >
            <div class="wizard-wrapper">
              <div class="wizard-number">1</div>
              <div class="wizard-label">
                <div class="wizard-title">{{ tabs[0].title }}</div>
                <div class="wizard-desc">{{ tabs[0].description }}</div>
              </div>
            </div>
          </div>
          <template v-for="(item, index) in tabs.slice(1)">
            <div class="wizard-step mr-2" data-wizard-type="step" :key="index">
              <div class="wizard-wrapper">
                <div class="wizard-number">
                  {{ item.sequence ? item.sequence : index + 2 }}
                </div>
                <div class="wizard-label">
                  <div class="wizard-title">{{ item.title }}</div>
                  <div class="wizard-desc">{{ item.description }}</div>
                </div>
              </div>
            </div>
          </template>
        </div>
      </div>
    </template>
  </div>
  <!--end: Wizard Nav -->
</template>
<script>
export default {
  name: 'form-header',
  props: {
    tabs: {
      type: Array,
      default: () => [],
    },
  },
  mounted() {},
  methods: {},
};
</script>
