<template>
  <!-- begin:: Footer -->
  <div class="footer bg-white py-4 d-flex flex-lg-column" id="kt_footer">
    <div
      class="d-flex align-items-center justify-content-center"
      v-bind:class="{ 'container-fluid': widthFluid, container: !widthFluid }"
    >
      <!-- TODO: change phone -->
      <span class="text-dark"
        >© Copyright 2020 - Toàn bộ bản quyền thuộc về DiaB</span
      >
    </div>
  </div>
  <!-- end:: Footer -->
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  name: 'KTFooter',
  computed: {
    ...mapGetters(['layoutConfig']),

    /**
     * Check if footer container is fluid
     */
    widthFluid() {
      return this.layoutConfig('footer.width') === 'fluid';
    },
  },
};
</script>
