<template>
  <div class="user-import-list-page w-100">
    <basic-subheader>
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
          <b-dropdown-form>
            <b-container class="p-0">
              <b-row>
                <b-col>
                  <basic-input
                    label="Tên nhóm"
                    :required="true"
                    placeholder="Nhập tên nhóm"
                    name="name"
                    :value.sync="filter.searchKey"
                  ></basic-input>
                </b-col>
              </b-row>
              <b-row>
                <b-col>
                  <basic-select
                    label="Chức năng"
                    placeholder="Chọn nhóm chức năng"
                    name="exerciseCategory"
                    apiPath="/exerciseCategory"
                    :value.sync="filter.searchKey"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col>
                  <basic-check-box
                    :options="[{ text: 'Lọc các nhóm Inactive', value: false }]"
                  ></basic-check-box>
                </b-col>
              </b-row>
            </b-container>
          </b-dropdown-form>
          <b-dropdown-divider> </b-dropdown-divider>
          <div class="d-flex align-items-center justify-content-lg-end m-0 p-4">
            <b-button class="btn ml-2" href="#" tabindex="0">
              <span class="svg-icon">
                <inline-svg
                  src="/media/svg/icons/Neolex/Basic/refresh-cw.svg"
                />
              </span>
              Reset bộ lọc
            </b-button>
            <b-button class="btn ml-2" href="#" tabindex="0">
              <span class="svg-icon">
                <inline-svg src="/media/svg/icons/Neolex/Basic/filter.svg" />
              </span>
              Lọc dữ liệu
            </b-button>
          </div>
        </b-dropdown>

        <b-button
          class="btn btn-success ml-2"
          type="button"
          id="show-btn"
          @click="handleImportUser()"
        >
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/upload-cloud.svg" />
          </span>
          Import danh sách
        </b-button>
      </template>
    </basic-subheader>
    <b-container fluid class="user-import-list-page__body mb-6 mt-6">
      <b-row>
        <b-col>
          <div class="card card-custom gutter-b">
            <div class="card-body mt-0">
              <template-table
                :column="column"
                :data="data"
                :paging="paging"
                :tableAction="false"
                :selectAction="false"
                :searchAction="false"
                @sortBy="sortRequest"
              >
                <template v-slot:body="{ item }">
                  <td>
                    {{ item.user_name }}
                  </td>
                  <td>
                    {{ item.user_code }}
                  </td>
                  <td>{{ item.survey_type }}</td>
                  <td>{{ item.survey_name }}</td>
                  <td>{{ item.survey_code }}</td>
                  <td>{{ item.user_age }}</td>
                  <td>{{ item.user_gender }}</td>
                  <td>{{ $moment(item.survey_day).format('DD/MM/YYYY') }}</td>
                  <td>{{ item.user_province }}</td>
                </template>
              </template-table>
            </div>
          </div>
        </b-col>
      </b-row>
    </b-container>
    <user-import-modal :popupType="'popupType'" @loadData="loadData" />
  </div>
</template>

<style lang="scss" scoped>
.dropdown-form-filter {
  .dropdown-menu {
    .container {
      width: 430px;
    }
  }
}
</style>

<script>
export default {
  components: { 'user-import-modal': () => import('./components/Modal') },
  data() {
    return {
      paging: {
        page: 1,
        pageSize: 10,
        total: 0,
      },
      filter: {
        searchKey: null,
      },
      sort: {
        by: null,
        order: null,
      },
      column: [
        {
          key: 'user_name',
          label: 'Bệnh nhân',
          sortable: false,
        },
        {
          key: 'user_code',
          label: 'Mã số',
          sortable: false,
        },
        {
          key: 'survey_type',
          label: 'Loại khảo sát',
          sortable: false,
        },
        {
          key: 'survey_name',
          label: 'Tên khảo sát',
          sortable: false,
        },
        {
          key: 'survey_code',
          label: 'Mã khảo sát',
          sortable: false,
        },
        {
          key: 'user_age',
          label: 'Năm sinh',
          sortable: false,
        },
        {
          key: 'user_gender',
          label: 'Giới Tính',
          sortable: false,
        },
        {
          key: 'survey_day',
          label: 'Ngày thực hiện khảo sát',
          sortable: false,
        },
        {
          key: 'user_province',
          label: 'Tỉnh thành',
          sortable: false,
        },
      ],
      data: [],
    };
  },
  computed: {
    searchParams() {
      return {
        searchKey: this.filter.searchKey,
        page: this.paging.page,
        size: this.paging.pageSize,
      };
    },
  },
  watch: {
    'paging.page'() {
      //this.loadData();
    },
    'paging.pageSize'() {
      //this.loadData();
    },
    sort: {},
  },
  methods: {
    pagingAction() {
      //this.loadData();
    },
    createItem() {
      this.$router.push({
        name: 'windown_import',
        params: {
          form_type: 'create',
        },
      });
    },
    editItem(item) {
      this.$router.push({
        name: 'user_group_detail',
        params: {
          form_type: 'edit',
          id: item.id,
        },
      });
    },
    searchRequest() {
      //this.loadData();
    },
    resetRequest() {
      this.filter.searchKey = null;
      this.$nextTick(() => {
        this.$validator.reset();
      });
      //this.loadData();
    },
    sortRequest() {
      return;
    },
    loadData(payload) {
      this.data = payload || [];
      return;
    },
    async handleImportUser() {
      this.$nextTick(() => {
        this.$bvModal.show('user-import-modal');
      });
    },
  },
  mounted() {
    // this.loadData();
  },
};
</script>
