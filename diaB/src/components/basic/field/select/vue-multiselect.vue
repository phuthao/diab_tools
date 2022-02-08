<template>
  <b-form-group
    :id="`${name}-group`"
    :description="description"
    :label="label"
    :label-for="`${name}-ID`"
    :invalid-feedback="invalidFeedback"
    :valid-feedback="validFeedback"
    :state="state"
    :class="`${required ? 'required' : ''}`"
  >
    <multiselect
      @input="changeSelectedValue"
      v-model="selectedValue"
      :multiple="multiple"
      :options="list_options"
      :select-label="selectLabel"
      :selected-label="selectedLabel"
      :deselect-label="deselectLabel"
      :label="valueLabel"
      :placeholder="placeholder"
      :track-by="trackBy"
      :close-on-select="true"
      :class="{ 'multiselect--solid': solid }"
      :allow-empty="allowEmpty"
      :disabled="disabled"
      :open-direction="openDirection"
      :searchable="searchable"
      :loading="isLoading"
      :custom-label="computedLabel"
      :taggable="taggable"
      @open="openOptions"
      @search-change="asyncFind"
      :limit="20"
      :max="max"
      :group-values="groupValue"
      :group-label="groupLabel"
    >
      <template slot="clear">
        <div
          v-if="selectedValue && !disabled && allowEmpty"
          class="multiselect__clear"
          @mousedown.prevent.stop="clearSelect"
        ></div>
      </template>
      <template v-slot:noOptions>Không có kết quả</template>
      <template v-slot:noResult>
        <p>
          <span> Không tìm thấy kết quả.</span>
          <span> Vui lòng đổi từ khóa tìm kiếm </span>
        </p></template
      >
      <template v-slot:afterList v-if="paging.canNext">
        <b-overlay :show="isLoading" rounded="lg" opacity="0.6">
          <li class="multiselect__load-more">
            <a href="javascript:;" @click="loadMore">Hiển thị thêm... </a>
          </li>
        </b-overlay></template
      >
    </multiselect>
  </b-form-group>
</template>

<script>
import _ from 'lodash';
import Multiselect from 'vue-multiselect';

export default {
  components: {
    Multiselect,
  },
  props: {
    required: {
      type: Boolean,
      default: false,
    },
    multiple: {
      type: Boolean,
      default: false,
    },
    id: {
      type: String,
      default: null,
    },
    name: {
      type: String,
      default: null,
    },
    value: {
      type: [String, Number, Boolean, Array, Object],
      default: () => null,
    },
    valueLabel: {
      type: String,
      default: 'name',
    },
    options: {
      type: Array,
      default: () => null,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    placeholder: {
      type: String,
      default: null,
    },
    description: {
      type: String,
      default: null,
    },
    label: {
      type: String,
      default: '',
    },
    invalidFeedback: {
      type: String,
      default: null,
    },
    validFeedback: {
      type: String,
      default: null,
    },
    state: {
      type: Boolean,
      default: null,
    },
    selectLabel: {
      type: String,
      default: '',
    },
    selectedLabel: {
      type: String,
      default: 'Đã chọn',
    },
    deselectLabel: {
      type: String,
      default: 'Bỏ chọn',
    },
    changeValueByObject: {
      type: Boolean,
      default: false,
    },
    trackBy: {
      type: String,
      default: 'id',
    },
    allowEmpty: {
      type: Boolean,
      default: true,
    },
    openDirection: {
      type: String,
      default: 'bottom',
    },
    searchable: {
      type: Boolean,
      default: true,
    },
    apiDetailPath: {
      type: String,
      default: null,
    },
    apiPath: {
      type: String,
      default: null,
    },
    searchField: {
      type: String,
      default: null,
    },
    searchParams: {
      type: Object,
      default: () => ({}),
    },
    solid: {
      type: Boolean,
      default: false,
    },
    computedLabel: {
      type: Function,
      required: false,
    },
    taggable: {
      type: Boolean,
      required: false,
    },
    max: {
      type: Number,
      required: false,
    },
    groupValue: {
      type: String,
      required: false,
    },
    groupLabel: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      isLoading: false,
      selectedValue: null,
      list_options: [],
      paging: {
        page: 1,
        size: 30,
        canNext: false,
      },
    };
  },
  watch: {
    selected_value(val) {
      this.$emit('update:value', val);
    },
    value: {
      deep: true,
      handler: function (val) {
        if (val === undefined || val === null) {
          this.selectedValue = null;
        } else {
          if (this.selectedValue) {
            let isChange = false;
            if (this.multiple) {
              let listSelected = this.selectedValue.map((x) => x[this.trackBy]);
              isChange = val.some((x) => !listSelected.includes(x));
            } else {
              isChange =
                val !== this.selectedValue[this.trackBy] ? true : false;
            }

            if (isChange) {
              this.getNewValue(val);
            }
          } else {
            this.getNewValue(val);
          }
        }
      },
    },
    options: {
      deep: true,
      handler: function (val) {
        this.list_options = val;
      },
    },
    searchParams: {
      deep: true,
      handler() {
        this.asyncFind();
      },
    },
  },
  methods: {
    changeSelectedValue(value) {
      let selectedValue = null;
      if (value) {
        if (this.multiple) {
          selectedValue = value.map((x) => {
            if (!this.changeValueByObject) {
              return x[this.trackBy];
            }
            return x;
          });
        } else {
          selectedValue = !this.changeValueByObject
            ? value[this.trackBy]
            : value;
        }
      }
      this.$emit('update:value', selectedValue);
      this.$emit('input', selectedValue);
      this.$emit('selected');
    },
    clearSelect() {
      this.selectedValue = null;
      this.$emit('update:value', null);
    },
    async getNewValue(val) {
      let selectedValue = null;
      if (!this.apiPath) {
        this.list_options = this.options;
      } else {
        await this.asyncFind();
      }
      if (this.multiple) {
        selectedValue = val.map((x) => {
          if (typeof selectedValue === 'object') {
            return x;
          }
          return x;
        });
        this.selectedValue = selectedValue;
        // Fix cannot select out range of list options (e.g : API call)
        // console.log('selectedValue :>> ', selectedValue);
        // this.selectedValue = this.list_options.filter((x) => {
        //   console.log('trackBy :>> ', x[this.trackBy]);
        //   return selectedValue.includes(x[this.trackBy]);
        // });
      } else {
        selectedValue = val;
        this.selectedValue = val;
        // this.selectedValue = this.list_options.find((x) => {
        //   if (
        //     typeof selectedValue === 'object' &&
        //     selectedValue.constructor === Object
        //   ) {
        //     return x[this.trackBy] === selectedValue[this.trackBy];
        //   }

        //   return x[this.trackBy] === selectedValue;
        // });
        //eslint-disable-next-line
      }
    },
    openOptions() {
      if (!this.apiPath) {
        this.list_options = this.options;
      } else {
        this.asyncFind();
      }
    },
    asyncFind: _.debounce(async function (query) {
      if (this.apiPath) {
        let params = {
          ...this.searchParams,
          page: 1,
          size: this.paging.size,
        };
        if (this.searchField && query) {
          params[this.searchField] = query;
        }
        this.isLoading = true;
        await this.$api
          .get(this.apiPath, { params })
          .then(({ meta, data }) => {
            this.list_options = data;
            this.paging.page = meta.page;
            this.paging.canNext = meta.canNext;
          })
          .finally(() => {
            this.isLoading = false;
          });
      }
    }, 500),
    async loadMore() {
      this.isLoading = true;
      let params = {
        ...this.searchParams,
        page: this.paging.page + 1,
        size: this.paging.size,
      };
      await this.$api
        .get(this.apiPath, { params })
        .then(({ meta, data }) => {
          this.list_options = this.list_options.concat(data);
          this.paging.page = meta.page;
          this.paging.canNext = meta.canNext;
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
  },
  async mounted() {
    if (!_.isNil(this.value)) {
      await this.getNewValue(this.value);
    }
  },
};
</script>
<style lang="scss">
.text-muted {
  outline: none;
}

.multiselect__load-more {
  text-align: center;
  margin-top: 1rem;
  margin-bottom: 1rem;
}
</style>
