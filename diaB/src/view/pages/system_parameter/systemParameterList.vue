<template>
  <div class="exercise-list-page w-100">
    <basic-subheader title="">
      <template v-slot:actions>
        <b-dropdown
          id="dropdown-right dropdown-form"
          class="dropdown-form-filter"
          no-caret
          right
        >
          <template #button-content>
            <span class="svg-icon">
              <inline-svg src="/media/svg/icons/Neolex/Basic/filter.svg" />
            </span>
            Bộ lọc
          </template>
          <h6 class="d-flex align-items-center mb-0 p-4">
            <span class="svg-icon mr-3">
              <inline-svg src="/media/svg/icons/Neolex/Basic/filter.svg" />
            </span>
            Bộ lọc
          </h6>
          <b-dropdown-divider> </b-dropdown-divider>
          <b-dropdown-form class="mw-400">
            <b-container class="p-0">
              <b-row>
                <b-col class="pb-0">
                  <basic-input
                    label="Mã tham số"
                    placeholder="Nhập mã tham số "
                    name="codeParams"
                    :value.sync="search.key"
                  ></basic-input>
                </b-col>
              </b-row>
              <b-row>
                <b-col class="pb-0">
                  <basic-input
                    label="Tên tham số"
                    placeholder="Nhập mã tham số "
                    name="codeParams"
                    :value.sync="search.value"
                  ></basic-input>
                </b-col>
              </b-row>
            </b-container>
          </b-dropdown-form>
          <b-dropdown-divider> </b-dropdown-divider>
          <div class="d-flex align-items-center justify-content-lg-end m-0 p-4">
            <b-button
              class="btn ml-2"
              href="#"
              tabindex="0"
              @click.stop="resetRequest"
            >
              <span class="svg-icon">
                <inline-svg
                  src="/media/svg/icons/Neolex/Basic/refresh-cw.svg"
                />
              </span>
              Reset bộ lọc
            </b-button>
            <b-button
              class="btn ml-2"
              href="#"
              tabindex="0"
              @click="searchRequest"
            >
              <span class="svg-icon">
                <inline-svg src="/media/svg/icons/Neolex/Basic/filter.svg" />
              </span>
              Lọc dữ liệu
            </b-button>
          </div>
        </b-dropdown>
      </template>
    </basic-subheader>
    <Wrapper>
      <b-overlay :show="loading">
        <v-data-table
          :headers="headers"
          :items="data"
          item-key="id"
          group-by="group"
          :items-per-page="-1"
          class="systems-parameter"
          :options.sync="options"
          hide-default-footer
        >
          <template v-slot:header.id="{ header }">
            <v-icon @click.stop="expandAll"
              >mdi-{{
                isExpandedNew
                  ? 'unfold-more-horizontal'
                  : 'unfold-less-horizontal'
              }}
            </v-icon>
            {{ header.text }}
          </template>

          <template v-slot:group.header="{ toggle, group, isOpen }">
            <td colspan="5" class="pl-0 no-background">
              <v-btn @click="toggle" :ref="group" :data-open="isOpen" icon>
                <v-icon
                  >mdi-{{ isOpen ? 'chevron-down' : 'chevron-up' }}</v-icon
                >
              </v-btn>
              <span class="font-weight-boldest">{{ group }}</span>
            </td>
          </template>
          <template v-slot:item="{ item, expand, isExpanded }">
            <tr>
              <td>{{ item.key }}</td>
              <td>{{ item.name }}</td>
              <td>{{ item.value }}</td>
              <td>{{ item.description }}</td>
              <td>
                <a
                  class="btn btn-icon btn-sm"
                  @click.stop="handleUpdate(item)"
                  style="
                    background-color: #e6f2f8;
                    border: none;
                    color: #407bff;
                  "
                  title="Chỉnh sửa"
                >
                  <span class="menu-icon svg-icon svg-icon-sm icon-edit">
                    <inline-svg
                      class="svg-icon"
                      src="/media/svg/icons/Neolex/Basic/edit-2.svg"
                    />
                  </span>
                </a>
              </td>
            </tr>
          </template>
        </v-data-table>
      </b-overlay>
      <update-system :detail.sync="detail" @loadData="loadData" />
    </Wrapper>
  </div>
</template>

<script>
export default {
  name: 'SystemParameter',
  components: { 'update-system': () => import('./components/Modal') },
  data() {
    return {
      loading: false,
      isExpandedNew: true,
      isExpandedOld: true,
      search: {
        key: null,
        value: null,
      },
      filter: {
        sortBy: null,
        sortDesc: null,
      },
      expanded: [],
      pagination: {
        page: 1,
        pageCount: 0,
        itemsPerPage: 10,
      },
      // searchParams: {
      //   configureSetupType: 2,
      // },
      headers: [
        {
          text: 'Mã tham số',
          align: 'left',
          value: 'key',
          width: '15%',
        },
        // { text: 'Category', value: 'group', sortable: false },
        {
          text: 'Tên tham số',
          value: 'name',
          sortable: true,
          align: 'left',
          width: '25%',
        },
        { text: 'Giá trị', value: 'value', sortable: false, width: '25%' },
        {
          text: 'Ghi chú',
          value: 'shortDescription',
          sortable: false,
          width: '25%',
        },
        { text: '', value: null, sortable: false },
        { text: '', value: null, sortable: false },
      ],
      data: [],
      detail: {},
      options: {},
    };
  },
  watch: {
    filter: {
      deep: true,
      handler() {
        this.loadData();
      },
    },
    options: {
      handler(newVal) {
        let { sortBy, sortDesc } = newVal;
        let [order] = sortDesc;
        let [sort] = sortBy;
        this.filter.sortBy = sort;

        this.filter.sortDesc = order ? 'desc' : 'asc';
      },
      deep: true,
    },
  },
  computed: {
    // FIX: fix update list after sort
    searchParams() {
      return {
        configureSetupType: 2,
        key: this.search.key,
        value: this.search.value,
        orderBy: this.filter.sortBy
          ? `${this.filter.sortBy} ${this.filter.sortDesc}`
          : null,

        takeAll: true,
      };
    },
  },

  methods: {
    searchRequest() {
      this.loadData();
    },
    resetRequest() {
      this.search.key = null;
      this.search.value = null;
      this.loadData();
    },
    async loadData() {
      this.loading = true;
      try {
        const { data } = await this.$api.get('CommonConfigureTask/Parameter', {
          params: this.searchParams,
        });
        this.data = data || [];
      } catch (error) {
        this.$toastr.e(error, 'ERROR');
      } finally {
        this.loading = false;
      }
    },
    handleUpdate(payload) {
      this.detail = payload;
      this.$bvModal.show('system-modal');
    },

    expandAll() {
      this.isExpandedNew = !this.isExpandedOld;
      // Filter vue-components
      Object.keys(this.$refs)
        .filter((e) => this.$refs[e]?.$el)
        .forEach((k) => {
          // Check if element has already open
          const status = this.$refs[k].$attrs['data-open'];
          if (status != this.isExpandedNew) {
            this.$refs[k].$el.click();
          }
          return;
        });
      this.isExpandedOld = this.isExpandedNew;
    },
  },
};
</script>

<style lang="scss" scoped>
.icon-edit {
  background-color: #e6f2f8;
  border: none;
  color: #407bff;
}
.mw-400 {
  min-width: 400px;
}
</style>
