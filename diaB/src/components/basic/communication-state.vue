<template>
  <div class="user-state">
    <span class="badge" :class="`badge__${badgeStyle}`">
      <slot></slot>
    </span>
  </div>
</template>

<style lang="scss" scoped>
.user-state {
  font-weight: normal;
  font-size: 12px;
  line-height: 15px;

  span.badge {
    color: #ffffff;
    padding: 10px 23px;
    width: 85px;
    border-radius: 56px;
    text-transform: capitalize;
    &__danger {
      background: #ff5756;
    }

    &__primary {
      background: linear-gradient(180deg, #407bff 0%, #6790eb 100%);
    }

    &__success {
      background: #21a567;
    }
    &__info {
      background: #888c9f;
    }
  }
}
</style>

<script>
export default {
  props: {
    type: {
      type: String,
      default: 'info',
    },
  },
  computed: {
    badgeStyle() {
      switch (this.type) {
        case 'draft':
          return 'info';
        case 'sent':
          return 'success';
        case 'recall':
          return 'danger';
        case 'active':
          return 'primary';
        default:
          return 'primary';
      }
    },
  },
};
</script>
