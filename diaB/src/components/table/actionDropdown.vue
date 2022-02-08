<template>
  <b-dropdown
    size="sm"
    variant="link"
    class="custom-v-dropdown"
    :boundary="boundaryView"
    no-caret
    no-flip
    right
  >
    <template #button-content>
      <span class="menu-icon svg-icon svg-icon-xl">
        <inline-svg src="/media/svg/icons/Neolex/Basic/more-horizontal.svg" />
      </span>
    </template>
    <!--begin::Navigation-->
    <div class="navi navi-hover min-w-md-132px">
      <template v-if="showView">
        <b-dropdown-text tag="div" class="navi-item">
          <a class="navi-link text-primary" @click.stop="$emit('view', value)">
            <span class="menu-icon svg-icon svg-icon-sm">
              <inline-svg
                class="svg-icon"
                src="/media/svg/icons/Neolex/Basic/eye.svg"
              />
            </span>
            <span class="navi-text text-primary">Xem chi tiết</span>
          </a>
        </b-dropdown-text>
        <b-dropdown-divider></b-dropdown-divider>
      </template>
      <template v-if="showEdit">
        <b-dropdown-text tag="div" class="navi-item">
          <a class="navi-link text-primary" @click.stop="$emit('edit', value)">
            <span class="menu-icon svg-icon svg-icon-sm">
              <inline-svg
                class="svg-icon"
                src="/media/svg/icons/Neolex/Basic/edit-2.svg"
              />
            </span>
            <span class="navi-text text-primary">Chỉnh sửa</span>
          </a>
        </b-dropdown-text>
        <b-dropdown-divider></b-dropdown-divider>
      </template>
      <template v-if="showCopy">
        <b-dropdown-text tag="div" class="navi-item">
          <a class="navi-link text-primary" @click.stop="$emit('copy', value)">
            <span class="menu-icon svg-icon svg-icon-sm">
              <inline-svg
                class="svg-icon"
                src="/media/svg/icons/Neolex/Basic/copy.svg"
              />
            </span>
            <span class="navi-text text-primary">Copy</span>
          </a>
        </b-dropdown-text>
        <b-dropdown-divider></b-dropdown-divider>
      </template>
      <template v-if="showSetDefault">
        <b-dropdown-text tag="div" class="navi-item">
          <a
            class="navi-link text-primary"
            @click.stop="$emit('setDefault', value)"
          >
            <span class="menu-icon svg-icon svg-icon-sm text-primary">
              <inline-svg
                class="svg-icon"
                src="/media/svg/icons/Neolex/Basic/bookmark.svg"
              />
            </span>
            <span class="navi-text text-primary">Đặt làm mặc định</span>
          </a>
        </b-dropdown-text>
        <b-dropdown-divider></b-dropdown-divider>
      </template>
      <slot />
      <template v-if="showDelete">
        <b-dropdown-text tag="div" class="navi-item">
          <a class="navi-link text-danger" @click.stop="$emit('delete', value)">
            <span class="menu-icon svg-icon svg-icon-sm">
              <inline-svg
                class="svg-icon"
                src="/media/svg/icons/Neolex/Basic/power.svg"
              />
            </span>
            <span class="navi-text text-danger">Inactive</span>
          </a>
        </b-dropdown-text>
      </template>
    </div>
    <!--end::Navigation-->
  </b-dropdown>
</template>

<style lang="scss" scope>
.custom-v-dropdown {
  .dropdown-toggle,
  .dropdown-toggle:hover {
    color: inherit;
  }
  .dropdown-menu {
    .b-dropdown-text {
      padding: 0;
      .navi-link {
        padding: 0.5rem 1rem;
        white-space: nowrap;
        font-weight: normal;
        font-size: 13px;

        .menu-icon {
          .svg-icon {
            margin-right: 0.5rem;
          }
        }
      }
    }
  }
}
</style>

<script>
export default {
  props: [
    'value',
    'show_view',
    'show_edit',
    'show_delete',
    'show_copy',
    'show_setDefault',
    'boundary',
  ],
  computed: {
    boundaryView() {
      return this.boundary;
    },
    showView() {
      if (this.show_view !== undefined) {
        return this.show_view;
      }

      return true;
    },
    showEdit() {
      if (this.show_edit !== undefined) {
        return this.show_edit;
      }

      return true;
    },
    showDelete() {
      if (this.show_delete !== undefined) {
        return this.show_delete;
      }

      return true;
    },
    showCopy() {
      if (this.show_copy !== undefined) {
        return this.show_copy;
      }

      return true;
    },
    showSetDefault() {
      if (this.show_setDefault !== undefined) {
        return this.show_setDefault;
      }

      return false;
    },
  },
};
</script>
