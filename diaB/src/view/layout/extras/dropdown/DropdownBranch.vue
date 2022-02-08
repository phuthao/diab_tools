<template>
  <b-list-group>
    <template v-for="(item, i) in branchList">
      <b-list-group-item
        href="#"
        v-on:click="setBranch(item)"
        :active="isActiveBranch(item.id)"
        :key="i"
        ><span class="symbol symbol-20 mr-3">
          <i class="fas fa-home"></i>
        </span>
        <span class="navi-text">{{ item.name }}</span></b-list-group-item
      >
    </template>
  </b-list-group>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  name: 'DropdownBranch',
  data() {
    return {
      branchList: [],
    };
  },
  methods: {
    async setBranch(item) {
      await this.$store.commit('context/setBranch', item);
      window.location.reload();
    },
    isActiveBranch(id) {
      return this.branch && this.branch.id === id ? true : false;
    },
    getBranchList() {
      this.$api
        .get('branch', {
          isDeleted: false,
        })
        .then(({ data }) => {
          this.branchList = data;
        });
    },
  },
  computed: {
    ...mapGetters('context', ['branch']),
  },
  mounted() {
    this.getBranchList();
  },
};
</script>
