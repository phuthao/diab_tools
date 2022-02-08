<template>
  <div class="template-table">
    <b-overlay :show="loading">
      <!--begin: Search Form-->
      <form
        class="kt-form kt-form--fit mb-8 fixcss"
        v-if="searchAction"
        @submit.prevent="$emit('search')"
        @reset="$emit('reset')"
      >
        <slot name="form"></slot>
        <div class="row mt-0">
          <div class="col-lg-12">
            <span v-if="btnPrimary">
              <button type="submit" class="btn btn-info">
                <span class="svg-icon">
                  <inline-svg src="/media/svg/icons/Neolex/Basic/filter.svg" />
                </span>
                Lọc dữ liệu
              </button>
              <button type="reset" class="btn btn-warning ml-4">
                <span class="svg-icon">
                  <inline-svg
                    src="/media/svg/icons/Neolex/Basic/refresh-cw.svg"
                  />
                </span>
                Reset bộ lọc
              </button>
            </span>
            <button
              v-if="btnAction"
              style="float: right"
              @click="$emit('more')"
              class="btn btn-primary btn-primary--icon ml-4"
              :disabled="btnActionDisabled"
            >
              <span>
                <span>{{ btnActionLable }}</span>
              </span>
            </button>
          </div>
        </div>
      </form>
      <!--begin: Datatable-->

      <!--begin: Datatable-->
      <div class="dataTables_wrapper dt-bootstrap4">
        <div class="row">
          <div class="col-sm-12">
            <b-collapse
              v-if="btnInactiveSelected"
              :visible="selected.length > 0"
              class="mb-5"
            >
              <div class="d-flex align-items-center">
                <div class="font-weight-bold text-danger mr-3">
                  Chọn
                  <span>{{ selected.length }}</span> dữ liệu:
                </div>
                <button
                  class="btn btn-sm btn-danger mr-2"
                  type="button"
                  @click="$emit('delete', selected), (selected = [])"
                >
                  Vô hiệu toàn bộ
                </button>
              </div>
            </b-collapse>
            <div class="table-responsive">
              <table
                class="table table-hover table-checkable dataTable dtr-inline"
              >
                <thead>
                  <tr role="row">
                    <th v-if="selectAction" style="width: 49px">
                      <span style="width: 20px">
                        <label class="checkbox checkbox-single checkbox-all">
                          <input type="checkbox" v-model="selectAll" />&nbsp;
                          <span></span>
                        </label>
                      </span>
                    </th>
                    <th
                      v-for="col in column"
                      :key="`head_${col.key}`"
                      :style="col.style"
                    >
                      <div
                        @click="col.sortable ? sortBy(col.key) : null"
                        :class="{
                          sorting: col.sortable,
                          sorted: col.sortable && col.key === sort.column,
                          asc: col.sortable && 'asc' === sort.order,
                          desc: col.sortable && 'desc' === sort.order,
                        }"
                      >
                        {{ col.label }}
                        <span class="svg-icon">
                          <inline-svg
                            src="/media/svg/icons/Neolex/Arrows/arrow-top.svg"
                            v-if="
                              col.sortable &&
                              ('' === sort.order || 'asc' === sort.order)
                            "
                          />
                          <inline-svg
                            src="/media/svg/icons/Neolex/Arrows/arrow-bottom.svg"
                            v-else-if="col.sortable && 'desc' === sort.order"
                          />
                        </span>
                      </div>
                    </th>
                    <th v-if="tableAction" style="min-width: 130px">
                      <div>Thao tác</div>
                    </th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="data.length == 0">
                    <td
                      valign="top"
                      :colspan="column.length + 2"
                      class="dataTables_empty text-center"
                    >
                      Không tìm thấy dữ liệu
                    </td>
                  </tr>
                  <tr
                    role="row"
                    v-for="(item, idx) of data"
                    :key="idx"
                    :class="
                      item.active === false || item.isActive === false
                        ? 'inActive-row'
                        : null
                    "
                  >
                    <td
                      v-if="selectAction"
                      class="datatable-cell-center datatable-cell datatable-cell-check"
                    >
                      <span style="width: 20px">
                        <label class="checkbox checkbox-single">
                          <input
                            type="checkbox"
                            :value="item"
                            v-model="selected"
                          />&nbsp;
                          <span></span>
                        </label>
                      </span>
                    </td>
                    <slot name="body" :item="item" :index="idx"></slot>
                    <td v-if="tableAction">
                      <slot name="action" :item="item"></slot>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
            <div class="dataTables_processing card" v-show="loading">
              Đang xử lý...
            </div>
          </div>
        </div>
        <pagination v-if="pagingAction" :paging="paging" />
      </div>
      <!--end: Datatable-->
    </b-overlay>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import Pagination from './pagination';

export default {
  components: {
    Pagination,
  },
  props: {
    // loading: {
    //   type: Boolean,
    //   default: false
    // },
    data: {
      type: Array,
      required: true,
    },
    column: {
      type: Array,
      required: true,
    },
    paging: {
      type: Object,
      default() {
        return {
          page: 1,
          pageSize: 10,
          total: 0,
        };
      },
    },
    selectAction: {
      type: Boolean,
      default: true,
    },
    searchAction: {
      type: Boolean,
      default: true,
    },
    tableAction: {
      type: Boolean,
      default: true,
    },
    pagingAction: {
      type: Boolean,
      default: true,
    },
    btnAction: {
      type: Boolean,
      default: false,
    },
    btnActionLable: {
      type: String,
    },
    btnActionDisabled: {
      type: Boolean,
      default: false,
    },
    btnPrimary: {
      type: Boolean,
      default: true,
    },
    btnInactiveSelected: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      selected: [],
      sort: {
        column: '',
        order: '',
      },
    };
  },
  methods: {
    sortBy(col) {
      if (this.sort.column === col) {
        this.sort.order = this.sort.order === 'asc' ? 'desc' : 'asc';
      } else {
        this.sort.order = 'asc';
      }
      this.sort.column = col;
      this.$emit('sortBy', { ...this.sort });
    },
  },
  computed: {
    ...mapGetters('context', ['loading']),
    selectAll: {
      get() {
        return (
          this.data.length > 0 && this.selected.length === this.data.length
        );
      },
      set(value) {
        if (value) {
          this.selected = [...this.data];
        } else {
          this.selected = [];
        }
      },
    },
  },
};
</script>

<style lang="scss">
.template-table {
  .dataTable {
    margin: 0px !important;
  }

  thead th {
    position: sticky;
    top: 0px;
    z-index: 10;
    background-color: #ebedf3;
    padding: 1rem !important;
    color: #515356;
    font-size: 13px;

    div {
      border: 1px solid transparent;
    }
  }

  .sorting {
    position: relative;
    padding-right: 2rem;
    cursor: pointer;
    display: flex;
    align-items: center;

    .svg-icon {
      margin-left: 0.5rem;
    }

    &.sorted {
      color: #515356;
    }
  }

  tbody tr {
    &:hover {
      background-color: #fff8eb;
    }

    td {
      border-top: none;
      border-bottom: 1px solid #ebedf3;
    }
  }
  tr.inActive-row:hover {
    background-color: #fff3f6;
  }
}
</style>
