<template>
  <!-- begin:: Header Topbar -->
  <div class="topbar">
    <!--begin: Hotel bar -->
    <!-- <div class="topbar-item mr-4">
      <b-dropdown variant="link" block menu-class="w-100">
        <template v-slot:button-content>
          <span class="svg-icon svg-icon-primary svg-icon-2x">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              xmlns:xlink="http://www.w3.org/1999/xlink"
              width="24px"
              height="24px"
              viewBox="0 0 24 24"
              version="1.1"
            >
              <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                <rect x="0" y="0" width="24" height="24" />
                <path
                  d="M3.95709826,8.41510662 L11.47855,3.81866389 C11.7986624,3.62303967
                  12.2013376,3.62303967 12.52145,3.81866389 L20.0429,8.41510557
                  C20.6374094,8.77841684 21,9.42493654 21,10.1216692 L21,19.0000642
                  C21,20.1046337 20.1045695,21.0000642 19,21.0000642 L4.99998155,21.0000673
                  C3.89541205,21.0000673 2.99998155,20.1046368 2.99998155,19.0000673
                  L2.99999828,10.1216672 C2.99999935,9.42493561 3.36258984,8.77841732
                  3.95709826,8.41510662 Z M10,13 C9.44771525,13 9,13.4477153 9,14 L9,17
                  C9,17.5522847 9.44771525,18 10,18 L14,18 C14.5522847,18 15,17.5522847
                  15,17 L15,14 C15,13.4477153 14.5522847,13 14,13 L10,13 Z"
                  fill="#000000"
                />
              </g></svg
          ></span>
          {{ branch.name }}
        </template>
        <b-dropdown-text tag="div" class="min-w-md-200px">
          <DropdownBranch></DropdownBranch>
        </b-dropdown-text>
      </b-dropdown>
    </div> -->
    <!--end: Hotel bar -->

    <!--begin: User Bar -->
    <b-dropdown no-caret no-flip right variant="link">
      <template #button-content>
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
      </template>
      <div class="navi navi-hover min-w-md-132px">
        <b-dropdown-text tag="div" class="navi-item cursor-pointer">
          <a class="navi-link cursor-pointer" @click.stop="viewUser">
            <span class="menu-icon svg-icon svg-icon-sm">
              <inline-svg
                class="svg-icon"
                src="/media/svg/icons/Neolex/Basic/user.svg"
              />
              Xem thông tin cá nhân
            </span>
          </a>
        </b-dropdown-text>
        <b-dropdown-divider></b-dropdown-divider>
        <b-dropdown-text tag="div" class="navi-item cursor-pointer">
          <a class="navi-link cursor-pointer" @click.stop="changePassword">
            <span class="menu-icon svg-icon svg-icon-sm">
              <inline-svg
                class="svg-icon"
                src="/media/svg/icons/Neolex/Basic/edit-2.svg"
              />
              Thay đổi mật khẩu
            </span>
          </a>
        </b-dropdown-text>
        <b-dropdown-divider></b-dropdown-divider>
        <b-dropdown-text tag="div" class="navi-item cursor-pointer">
          <a class="navi-link cursor-pointer" @click.stop="onLogout">
            <span class="menu-icon svg-icon svg-icon-sm">
              <inline-svg
                class="svg-icon"
                src="/media/svg/icons/Navigation/Sign-out.svg"
              />
              Đăng xuất
            </span>
          </a>
        </b-dropdown-text>
      </div>
    </b-dropdown>
    <UserProfile />
    <!-- <KTQuickUser></KTQuickUser> -->
    <!--end: User Bar -->
  </div>
  <!-- end:: Header Topbar -->
</template>

<style lang="scss">
.topbar {
  .dropdown-toggle {
    padding: 0;
    &:hover {
      text-decoration: none;
    }

    &.dropdown-toggle-no-caret {
      &:after {
        content: none;
      }
    }
  }

  .dropdown-menu {
    margin: 0;
    padding: 0;
    outline: none;
    .b-dropdown-text {
      padding: 0;
    }
  }
}
.min-w-md-132px {
  min-width: 132px;
}
.navi-link {
  font-weight: 400;
  white-space: nowrap;
}
</style>

<script>
import { mapGetters } from 'vuex';
import UserProfile from './UserProfile';
export default {
  name: 'KTTopbar',
  data() {
    return {
      branchList: [],
    };
  },
  components: {
    UserProfile,
    // KTQuickUser,
    // DropdownBranch,
  },
  methods: {
    async onLogout() {
      let confirm = await this.$swal({
        title: 'Xác nhận?',
        text: `Bạn có muốn đăng xuất`,
        icon: 'warning',
        buttons: {
          cancel: {
            text: 'Huỷ',
            value: false,
            visible: true,
            className: '',
            closeModal: true,
          },
          confirm: {
            text: 'Xác nhận',
            value: true,
            visible: true,
            className: '',
            closeModal: true,
          },
        },
      });
      if (confirm) {
        this.$mgr.signOut();
      }
    },
    changePassword() {
      const auth = `${process.env.VUE_APP_AUTHORITY}/account/changepassword`;
      const redirectUri = encodeURIComponent(`${window.location.origin}`);
      const params = {
        returnurl: redirectUri,
      };
      // convert objec to a query string
      let qs = Object.keys(params)
        .map((key) => `${key}=${params[key]}`)
        .join('&');

      window.location.replace(`${auth}?${qs}`);
    },
    viewUser() {
      this.$bvModal.show('user-profile');
    },
  },
  computed: {
    ...mapGetters(['currentUser', 'isAuthenticated']),
    ...mapGetters('context', ['branch']),
    picture() {
      return this.currentUser.avatar && this.currentUser.avatar.url
        ? this.currentUser.avatar.url
        : 'media/users/300_21.jpg';
    },
  },
  async mounted() {},
};
</script>
