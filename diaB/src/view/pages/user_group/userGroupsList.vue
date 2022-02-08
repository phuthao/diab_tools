<template>
  <div class="user-groups-list-page w-100">
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
          @click="createItem"
        >
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/plus.svg" />
          </span>
          Thêm nhóm mới
        </b-button>
      </template>
    </basic-subheader>
    <b-container fluid class="user-groups-list-page__body mb-6 mt-6">
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
                    {{ item.name }}
                  </td>
                  <td style="width: 20px">
                    <action-dropdown
                      :value="item"
                      @view="viewItem"
                      @edit="editItem"
                      @delete="deleteItem"
                      @copy="copyItem"
                    >
                    </action-dropdown>
                  </td>
                  <td>{{ item.note }}</td>
                  <td>
                    <div>
                      <avatar
                        v-for="member in item.members"
                        :key="member.id"
                        styleClass="mr-2"
                        :size="'40px'"
                        :text="member.fullName"
                        :src="member.picture"
                        :rounded="true"
                      ></avatar>
                    </div>
                  </td>
                  <td>{{ $moment(item.lastUpdated).format('DD/MM/YYYY') }}</td>
                  <td>
                    <div class="d-flex align-items-center">
                      <avatar
                        size="40px"
                        :text="item.updatedPerson.fullName"
                        :src="item.updatedPerson.picture"
                        :rounded="true"
                      ></avatar>
                      <div class="d-flex flex-column ml-5">
                        <p
                          class="mb-0"
                          style="
                            font-weight: 600;
                            font-size: 13px;
                            color: #515356;
                          "
                        >
                          {{ item.updatedPerson.fullName }}
                        </p>
                        <p
                          class="mt-2 mb-0"
                          style="font-size: 12px; color: #888c9f"
                        >
                          {{ item.updatedPerson.username }}
                        </p>
                      </div>
                    </div>
                  </td>
                </template>
              </template-table>
            </div>
          </div>
        </b-col>
      </b-row>
    </b-container>
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
          key: 'name',
          label: 'Tên Nhóm',
          sortable: true,
        },
        {
          key: 'actionDropdown',
          label: '',
          sortable: false,
        },
        {
          key: 'functionType',
          label: 'Ghi chú',
          sortable: false,
        },
        {
          key: 'members',
          label: 'Thành viên',
          sortable: false,
        },
        {
          key: 'lastUpdated',
          label: 'Cập nhật lần cuối',
          sortable: false,
        },
        {
          key: 'updatedPerson',
          label: 'Người cập nhật',
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
      this.loadData();
    },
    'paging.pageSize'() {
      this.loadData();
    },
    sort: {},
  },
  methods: {
    pagingAction() {
      this.loadData();
    },
    createItem() {
      this.$router.push({
        name: 'user_group_detail',
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
    // viewItem(item) {
    // },
    // editItem(item) {
    // },
    // copyItem(item) {
    // },
    // deleteItem(item) {
    // },
    searchRequest() {
      this.loadData();
    },
    resetRequest() {
      this.filter.searchKey = null;
      this.$nextTick(() => {
        this.$validator.reset();
      });
      this.loadData();
    },
    sortRequest() {
      return;
    },
    loadData() {
      this.$store.commit('context/setLoading', true);
      this.$api
        .get('user_groups', {
          params: this.searchParams,
        })
        .then(({ meta, data }) => {
          this.data = data || [];
          this.paging.total = meta.total;
        })
        .finally(() => {
          this.$store.commit('context/setLoading', false);
        });
      return;
    },
  },
  mounted() {
    // this.loadData();
  },
};
</script>
