<template>
  <ul class="menu-nav">
    <!-- USER MANAGEMENT: BEGIN -->
    <li
      aria-haspopup="true"
      data-menu-toggle="hover"
      class="menu-item menu-item-submenu"
    >
      <a class="menu-link menu-toggle">
        <span class="menu-icon svg-icon svg-icon-xl">
          <inline-svg
            class="svg-icon"
            src="/media/svg/icons/Code/Lock-overturning.svg"
          />
        </span>
        <span class="menu-text">Quản lý tài khoản</span>
        <i class="menu-arrow"></i>
      </a>

      <div class="menu-submenu">
        <span class="menu-arrow"></span>
        <ul class="menu-subnav">
          <router-link
            class="menu-link menu-toggle"
            :to="{ name: 'user_group_list' }"
            v-slot="{ href, navigate, isActive, isExactActive }"
          >
            <li
              aria-haspopup="true"
              data-menu-toggle="hover"
              class="menu-item"
              :class="[
                isActive && 'menu-item-active',
                isExactActive && 'menu-item-active',
              ]"
            >
              <a :href="href" class="menu-link" @click="navigate">
                <i class="menu-bullet menu-bullet-line">
                  <span></span>
                </i>
                <span class="menu-text">Quản lý nhóm quyền</span>
              </a>
            </li>
            
          </router-link>
          <router-link
            class="menu-link menu-toggle"
            :to="{ name: 'user_portal_list' }"
            v-slot="{ href, navigate, isActive, isExactActive }"
          >
            <li
              aria-haspopup="true"
              data-menu-toggle="hover"
              class="menu-item"
              :class="[
                isActive && 'menu-item-active',
                isExactActive && 'menu-item-active',
              ]"
            >
              <a :href="href" class="menu-link" @click="navigate">
                <i class="menu-bullet menu-bullet-line">
                  <span></span>
                </i>
                <span class="menu-text">Quản lý tài khoản Portal</span>
                
              </a>
             
              
            </li>
          </router-link>

           <router-link
            class="menu-link menu-toggle"
            :to="{ name: 'user_import_list' }"
            v-slot="{ href, navigate, isActive, isExactActive }"
          >
          <li  
              aria-haspopup="true"
              data-menu-toggle="hover"
              class="menu-item"
              :class="[
                isActive && 'menu-item-active',
                isExactActive && 'menu-item-active',
              ]">
           <a :href="href" class="menu-link" @click="navigate">
                <i class="menu-bullet menu-bullet-line">
                  <span></span>
                </i>
                <span class="menu-text">Kết quả khảo sát</span>
              </a>
          </li>
           </router-link>
        </ul>
      </div>
    </li>
    <!-- USER MANAGEMENT: END -->
    <!-- SETUPS: BEGIN -->
    <li
      aria-haspopup="true"
      data-menu-toggle="hover"
      class="menu-item menu-item-submenu"
    >
      <a class="menu-link menu-toggle">
        <span class="menu-icon svg-icon svg-icon-xl">
          <inline-svg
            class="svg-icon"
            src="/media/svg/icons/Code/Settings4.svg"
          />
        </span>
        <span class="menu-text">Cài đặt</span>
        <i class="menu-arrow"></i>
      </a>

      <div class="menu-submenu">
        <span class="menu-arrow"></span>
        <ul class="menu-subnav">
          <router-link
            class="menu-link menu-toggle"
            :to="{ name: 'system_parameter' }"
            v-slot="{ href, navigate, isActive, isExactActive }"
          >
            <li
              aria-haspopup="true"
              data-menu-toggle="hover"
              class="menu-item"
              :class="[
                isActive && 'menu-item-active',
                isExactActive && 'menu-item-active',
              ]"
            >
              <a :href="href" class="menu-link" @click="navigate">
                <i class="menu-bullet menu-bullet-line">
                  <span></span>
                </i>
                <span class="menu-text">Tham số hệ thống</span>
              </a>
            </li>
          </router-link>
        </ul>
      </div>
    </li>
    <!-- SETUPS: END -->
  </ul>
</template>

<script>
export default {
  name: 'KTMenu',
  methods: {
    hasActiveChildren(match) {
      return this.$route['path'].indexOf(match) !== -1;
    },
  },
};
</script>
<style scoped>
.no-wrap {
  white-space: nowrap;
}
</style>
