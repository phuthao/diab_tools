<template>
  <!--begin: Breadcrumb -->
  <div class="breadcrumb-custom">
    <h5 class="breadcrumb-custom__item active">
      {{ getItemActive() || 'Trang chủ' }}
    </h5>
    <ul class="d-flex align-items-center" v-if="this.breadcrumbList.length > 0">
      <li @click="routeTo()">
        <span>Trang chủ</span>
      </li>
      <template v-for="(breadcrumb, i) in breadcrumbList">
        <li
          class="d-flex align-items-center"
          @click="routeTo(i)"
          :key="`${i}-${breadcrumb.link}`"
          v-if="!breadcrumb.active"
        >
          <span class="svg-icon svg-icon-sm ml-2 mr-2">
            <inline-svg
              src="/media/svg/icons/Neolex/Arrows/chevron-right.svg"
            />
          </span>
          <span>
            {{ breadcrumb.name }}
          </span>
        </li>
      </template>
    </ul>
  </div>
  <!--end: Breadcrumb -->
</template>

<script>
export default {
  name: 'KTMenu',
  data() {
    return {
      breadcrumbList: [],
    };
  },
  watch: {
    $route() {
      // react to route changes...
      this.updateList();
    },
  },
  methods: {
    getItemActive() {
      const itemActive = this.breadcrumbList[this.breadcrumbList.length - 1];
      if (itemActive && itemActive.active) {
        if (this.$route.params && this.$route.params.form_type) {
          return itemActive[`${this.$route.params.form_type}Name`];
        }
        return itemActive.name;
      }
    },
    routeTo(pRouteTo) {
      if (!pRouteTo && pRouteTo !== 0) {
        this.$router.push('/');
      } else if (this.breadcrumbList[pRouteTo].link) {
        this.$router.push({ name: this.breadcrumbList[pRouteTo].link });
      }
    },
    updateList() {
      this.breadcrumbList = this.$route.meta.breadcrumb || [];
    },
  },
  mounted() {
    this.updateList();
  },
};
</script>
