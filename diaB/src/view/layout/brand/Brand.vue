<template>
  <!-- begin:: Aside -->
  <div class="brand flex-column-auto" id="kt_brand" ref="kt_brand">
    <div class="brand-logo">
      <router-link to="/">
        <img :src="siteLogo()" alt="Logo" class="img-logo" />
      </router-link>
    </div>
    <div class="brand-tools" v-if="allowMinimize">
      <button
        class="brand-toggle p-1 border rounded-circle"
        id="kt_aside_toggle"
        ref="kt_aside_toggle"
        @click="isToggleSidebar = !isToggleSidebar"
      >
        <span class="svg-icon svg-icon mr-0 ml-0">
          <inline-svg
            class="svg-icon mr-0 ml-0"
            src="/media/svg/icons/Navigation/Angle-double-left.svg"
          />
        </span>
      </button>
    </div>
  </div>
  <!-- end:: Aside -->
</template>

<style lang="scss" scoped>
.aside-toggle {
  outline: none;
}
.img-logo {
  max-width: 100px;
}
</style>

<script>
import { mapGetters } from 'vuex';
// import objectPath from 'object-path';
import KTLayoutBrand from '@/assets/js/layout/base/brand.js';
import KTLayoutAsideToggle from '@/assets/js/layout/base/aside-toggle.js';

export default {
  name: 'KTBrand',
  data() {
    return {
      isToggleSidebar: true,
    };
  },
  mounted() {
    // Init Brand Panel For Logo
    KTLayoutBrand.init(this.$refs['kt_brand']);

    // Init Aside Menu Toggle
    KTLayoutAsideToggle.init(this.$refs['kt_aside_toggle']);
  },
  methods: {
    siteLogo() {
      let logo;
      if (this.isToggleSidebar) {
        logo = this.layoutConfig('loader.logo');
      } else {
        logo = this.layoutConfig('loader.logo-minimize');
      }

      return process.env.BASE_URL + logo;
    },
  },
  computed: {
    ...mapGetters(['layoutConfig']),

    allowMinimize() {
      return !!this.layoutConfig('aside.self.minimize.toggle');
    },
  },
};
</script>
