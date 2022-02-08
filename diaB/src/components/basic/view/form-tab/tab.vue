<template>
  <div ref="tab">
    <b-tab :disabled="disabled">
      <template v-slot:title>
        <div class="d-flex align-items-center tab__header">
          <div class="header__icon">
            <div class="symbol symbol-40">
              <span class="symbol-icon symbol-label font-size-h4">{{
                index
              }}</span>
            </div>
          </div>
          <div class="header__content">
            <slot name="title">
              <div class="content__title">{{ title }}</div>
            </slot>
            <slot name="description">
              <div class="content__description">
                {{ description }}
              </div>
            </slot>
          </div>
        </div>
      </template>
      <b-container fluid>
        <slot name="tab-title">
          <b-row align-h="start" class="mt-10 mb-10 font-weight-bold text-dark">
            <b-col
              ><h2>{{ contentTitle ? contentTitle : title }}</h2>
            </b-col>
          </b-row>
        </slot>
        <slot></slot>
      </b-container>
    </b-tab>
  </div>
</template>
<script>
import {
  // mapGetters,
  mapActions,
} from 'vuex';
export default {
  components: {},
  // computed: {
  //   ...mapGetters("context", ["loading"]),
  //   disabled: function() {
  //     return this.formType === "detail";
  //   }
  // },
  props: {
    currentTab: {
      type: String,
      default: null,
    },
    name: {
      type: String,
      default: null,
    },
    index: {
      type: Number,
      default: 0,
    },
    title: {
      type: String,
      default: null,
    },
    contentTitle: {
      type: String,
      default: null,
    },
    description: {
      type: String,
      default: null,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {};
  },
  methods: {
    ...mapActions('context', ['pushRouteTabIndex']),
  },
  mounted() {
    this.pushRouteTabIndex();
  },
};
</script>

<style lang="scss">
.form__card {
  .nav-link {
    background: #f3f6f9;
  }

  .tab__header {
    margin: 10px;
    margin-left: 0;
    min-width: 150px;
    max-width: 300px;
    overflow: hidden;
    .header__icon {
      height: 40px;
      margin-right: 1rem;

      .symbol-icon {
        color: #3699ff;
        background-color: rgba(54, 153, 255, 0.08);
      }
    }

    .header__content {
      .content__title {
        font-size: 14px;
        font-weight: bold;
      }
    }
  }

  .tab__active {
    .symbol-icon {
      background: #3699ff !important;
      color: white !important;
    }
  }
}
</style>
