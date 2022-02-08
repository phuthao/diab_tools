<template>
  <div class="topbar-item">
    <div
      class="btn btn-icon w-auto btn-clean d-flex align-items-center btn-lg px-2"
      id="kt_quick_user_toggle"
    >
      <span
        class="text-muted font-weight-bold font-size-base d-none d-md-inline mr-1"
      >
        Hi,
      </span>
      <span
        class="text-dark-50 font-weight-bolder font-size-base d-none d-md-inline mr-3"
      >
        {{ currentUser && currentUser.fullName }}
      </span>
      <avatar
        size="35px"
        :text="currentUser && currentUser.fullName"
        :src="picture"
        :rounded="true"
      ></avatar>
    </div>

    <div
      id="kt_quick_user"
      ref="kt_quick_user"
      class="offcanvas offcanvas-right p-10"
    >
      <!--begin::Header-->
      <div
        class="offcanvas-header d-flex align-items-center justify-content-between pb-5"
      >
        <h6 class="font-weight-bold m-0">
          Thông tin người dùng
          <small class="text-muted font-size-sm ml-2"></small>
        </h6>
        <a
          href="#"
          class="btn btn-xs btn-icon btn-light btn-hover-primary"
          id="kt_quick_user_close"
        >
          <i class="ki ki-close icon-xs text-muted"></i>
        </a>
      </div>
      <!--end::Header-->

      <!--begin::Content-->

      <!-- <perfect-scrollbar
        class="offcanvas-content pr-5 mr-n5 scroll"
        style="max-height: 90vh; position: relative"
      >
        <div class="d-flex align-items-center mt-5">
          <div class="symbol symbol-100 mr-5">
            <img class="symbol-label" :src="picture" alt="" />
            <i class="symbol-badge bg-success"></i>
          </div>
          <div class="d-flex flex-column">
            <a
              @click="navigateProfile"
              class="font-weight-bold font-size-h5 text-dark-75 text-hover-primary"
            >
              {{ currentUser && currentUser.fullName }} sdadasd
            </a>
            <div class="navi mt-2">
              <a href="#" class="navi-item">
                <span class="navi-link p-0 pb-2">
                  <span class="navi-icon mr-1">
                    <span class="svg-icon svg-icon-lg svg-icon-primary">
                      <inline-svg
                        src="/media/svg/icons/Communication/Mail-notification.svg"
                      />
                    </span>
                  </span>
                  <span class="navi-text text-muted text-hover-primary">
                    {{ currentUser && currentUser.username }}
                  </span>
                </span>
              </a>
            </div>
            <button class="btn btn-light-primary btn-bold" @click="onLogout">
              Đăng xuất
            </button>
          </div>
        </div>
      </perfect-scrollbar> -->
      <!--end::Content-->
    </div>
  </div>
</template>

<style lang="scss" scoped>
#kt_quick_user {
  overflow: hidden;
}
</style>

<script>
import { mapGetters } from 'vuex';

import KTLayoutQuickUser from '@/assets/js/layout/extended/quick-user.js';
import KTOffcanvas from '@/assets/js/components/offcanvas.js';

export default {
  name: 'KTQuickUser',
  data() {
    return {};
  },
  mounted() {
    // Init Quick User Panel
    KTLayoutQuickUser.init(this.$refs['kt_quick_user']);
  },
  methods: {
    navigateProfile() {
      if (this.$route.name !== 'profile') {
        this.$router.push({ path: '/profile' });
        this.closeOffcanvas();
      }
    },

    onLogout() {
      this.$mgr.signOut();
    },
    closeOffcanvas() {
      new KTOffcanvas(KTLayoutQuickUser.getElement()).hide();
    },
  },
  computed: {
    ...mapGetters(['currentUser', 'isAuthenticated']),
    picture() {
      return process.env.BASE_URL + this.currentUser && this.currentUser?.image
        ? this.currentUser.image
        : 'media/users/300_21.jpg';
    },
  },
};
</script>
