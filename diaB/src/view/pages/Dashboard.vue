<template>
  <div></div>
</template>

<script>
import { mapGetters } from 'vuex';

import {
  ADD_BODY_CLASSNAME,
  REMOVE_BODY_CLASSNAME,
} from '@/core/services/store/htmlclass.module.js';

export default {
  name: 'dashboard',
  components: {},
  computed: {
    ...mapGetters(['currentUser', 'isAuthenticated']),
  },
  mounted() {
    // show page loading
    this.$store.dispatch(ADD_BODY_CLASSNAME, 'page-loading');

    if (this.currentUser && this.currentUser.role === 'User') {
      // hide page loading
      this.$store.dispatch(REMOVE_BODY_CLASSNAME, 'page-loading');
    } else {
      this.$store.dispatch(REMOVE_BODY_CLASSNAME, 'page-loading');
      this.$router.push({
        name: 'user_group_list',
      });
    }
  },
  methods: {},
};
</script>
