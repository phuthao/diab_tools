<template>
  <b-overlay :show="loading" rounded="sm">
    <b-form @submit="onSubmit">
      <b-card no-body>
        <b-tabs
          @input="changeTab"
          class="form__card"
          v-model="tabIndex"
          active-nav-item-class="tab__active"
        >
          <b-card-text>
            <slot></slot>
          </b-card-text>
        </b-tabs>
        <template #footer v-if="formType === 'detail'">
          <slot name="footer">
            <div class="d-flex justify-content-between">
              <div class="mr-2">
                <button
                  v-if="goBackButton && !havePreviousTab"
                  @click="goBack()"
                  class="btn btn-light-primary font-weight-bold text-uppercase px-9 py-4"
                >
                  Quay lại
                </button>
                <button
                  v-if="previousButton && havePreviousTab"
                  @click="tabIndex--"
                  class="btn btn-light-primary font-weight-bold text-uppercase px-9 py-4"
                >
                  Quay lại
                </button>
              </div>
              <div>
                <button
                  v-if="submitButton && formType !== 'detail'"
                  type="submit"
                  class="btn btn-success font-weight-bold text-uppercase px-9 py-4 mr-4"
                >
                  <span v-if="formType === 'create'">
                    {{ createButtonLabel }}
                  </span>
                  <span v-else-if="formType === 'edit'">
                    {{ editButtonLabel }}
                  </span>
                </button>
                <button
                  v-if="nextButton && haveNextTab"
                  @click="tabIndex++"
                  class="btn btn-primary font-weight-bold text-uppercase px-9 py-4"
                >
                  Tiếp theo
                </button>
              </div>
            </div>
          </slot>
        </template>
      </b-card>
    </b-form>
  </b-overlay>
</template>
<script>
import { mapGetters } from 'vuex';
export default {
  components: {},
  computed: {
    ...mapGetters('context', ['loading', 'routeTabIndex']),
    disabled: function () {
      return this.formType === 'detail';
    },
  },
  props: {
    formType: {
      type: String,
      default: 'create',
      required: true,
      validator: function (value) {
        return ['create', 'edit', 'detail'].includes(value);
      },
    },
    formData: {
      type: Object,
      default: () => ({}),
    },
    goBackButton: {
      type: Boolean,
      default: false,
    },
    previousButton: {
      type: Boolean,
      default: false,
    },
    nextButton: {
      type: Boolean,
      default: false,
    },
    submitButton: {
      type: Boolean,
      default: true,
    },
    createButtonLabel: {
      type: String,
      default: 'Tạo mới',
    },
    editButtonLabel: {
      type: String,
      default: 'Cập nhật',
    },
  },
  data() {
    return {
      tabIndex: 0,
      havePreviousTab: false,
      haveNextTab: true,
    };
  },
  watch: {},
  methods: {
    changeTab(tabIndex) {
      if (this.routeTabIndex.length) {
        let firstTabIndex = Math.min(...this.routeTabIndex);
        let lastTabIndex = Math.max(...this.routeTabIndex);
        if (tabIndex === firstTabIndex) {
          this.havePreviousTab = false;
        } else {
          this.havePreviousTab = true;
        }

        if (tabIndex === lastTabIndex) {
          this.haveNextTab = false;
        } else {
          this.haveNextTab = true;
        }
      }
      this.$emit('changeTab', tabIndex);
    },
    showTab(tabIndex) {
      this.tabIndex = tabIndex;
    },
    goBack() {
      this.$router.go(-1);
    },
    onSubmit($event) {
      $event.preventDefault();
      this.$emit('submit');
    },
  },
  mounted() {},
};
</script>

<style lang="scss">
.form__card {
  background: #fff;

  .nav {
    background-color: #ebedf3 !important;
  }

  .nav-tabs {
    .nav-link {
      border: none !important;
    }
  }
}
</style>
