<template>
  <b-container fluid>
    <b-row>
      <b-col>
        <div class="card card-custom gutter-b">
          <div class="card-header">
            <div class="card-title">
              <h3 class="card-label">Danh sách kho</h3>
            </div>
          </div>
          <div class="card-body mb-0">
            <template-table
              :column="column"
              :data="data"
              :paging="paging"
              @search="searchRequest"
              @reset="resetRequest"
              @delete="deleteAllRequest"
              @sortBy="sortRequest"
              :selectAction="false"
            >
              <template v-slot:form>
                <div class="row mb-0">
                  <label class="mt-3 ml-5">Tìm kiếm:</label>
                  <div class="col-lg-3 mb-lg-0 mb-0">
                    <b-input
                      type="text"
                      class="form-control datatable-input"
                      placeholder="Nhập mã, tên kho"
                      data-col-index="0"
                      v-model="filter.searchKey"
                    />
                  </div>
                  <label class="mt-3 ml-10">Trạng thái:</label>
                  <div class="col-lg-3 mb-lg-0 mb-0 ml-2">
                    <basic-select
                      :value.sync="filter.isDeleted"
                      :options="isDeletedOpts"
                      placeholder="Chọn trạng thái"
                      :solid="false"
                      :allowEmpty="true"
                    />
                  </div>
                </div>
              </template>
              <template v-slot:body="{ item }">
                <td>
                  <router-link
                    :to="{
                      name: 'warehouse_detail',
                      params: {
                        form_type: 'detail',
                        id: item.id,
                      },
                    }"
                    >{{ item.code }}</router-link
                  >
                </td>
                <td>
                  <router-link
                    :to="{
                      name: 'warehouse_detail',
                      params: {
                        form_type: 'detail',
                        id: item.id,
                      },
                    }"
                    >{{ item.name }}</router-link
                  >
                </td>
                <td>{{ item.otherName }}</td>
                <td>{{ item.inventoryNumber }}</td>
                <td></td>
                <td>
                  <state :isDeleted="item.isDeleted" />
                </td>
              </template>
              <template v-slot:action="{ item }">
                <action
                  :value="item"
                  @view="view"
                  @edit="edit"
                  :show_delete="false"
                ></action>
              </template>
            </template-table>
          </div>
        </div>
      </b-col>
    </b-row>
  </b-container>
</template>

<script>
import { mapGetters } from 'vuex';
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
        isDeleted: false,
      },
      sort: {
        by: null,
        order: null,
      },
      isDeletedOpts: [
        { id: false, name: 'Hoạt động' },
        { id: true, name: 'Ngưng hoạt động' },
      ],
      column: [
        {
          key: 'code',
          label: 'Mã kho',
          sortable: true,
        },
        {
          key: 'name',
          label: 'Tên kho',
          sortable: true,
        },
        {
          key: 'otherName',
          label: 'Tên khác',
          sortable: false,
        },
        {
          key: 'otherName',
          label: 'Nhóm kho',
          sortable: false,
        },
        {
          key: 'inventoryNumber',
          label: 'Số lượng tồn kho',
          sortable: true,
        },
        {
          key: 'IsDeleted',
          label: 'Trạng thái',
          sortable: true,
        },
      ],
      data: [],
    };
  },
  computed: {
    ...mapGetters('context', ['branch']),
    searchParams() {
      return {
        branchId: this.branch.id,
        searchKey: this.filter.searchKey,
        isDeleted: this.filter.isDeleted,
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
    create() {
      this.$router.push({
        name: 'warehouse_detail',
        params: {
          form_type: 'create',
        },
      });
    },
    searchRequest() {
      this.loadData();
    },
    resetRequest() {
      this.filter.searchKey = null;
      this.filter.isDeleted = null;
      this.$nextTick(() => {
        this.$validator.reset();
      });
      this.loadData();
    },
    deleteAllRequest(item) {
      this.$swal({
        title: 'Xác nhận?',
        text: `Bạn có muốn xóa ${item.length} đối tượng đã chọn`,
        icon: 'warning',
        buttons: {
          cancel: {
            text: 'Huỷ',
            value: false,
            visible: true,
            className: '',
            closeModal: true,
          },
          confirm: {
            text: 'Xác nhận',
            value: true,
            visible: true,
            className: '',
            closeModal: true,
          },
        },
      }).then(
        function (result) {
          if (result) {
            // inactive all
            Promise.all(item.map((x) => this.inactiveItem(x)))
              .then(() => {
                this.$swal(
                  'Thành công!',
                  'Các đối tượng đã ngưng hoạt động.',
                  'success',
                );
              })
              .catch(() => {
                this.$swal('Lỗi!', 'Đã có lỗi xảy ra.', 'error');
              });
          }
        }.bind(this),
      );
    },
    sortRequest() {
      return;
    },
    view(item) {
      this.$router.push({
        name: 'warehouse_detail',
        params: {
          form_type: 'detail',
          id: item.id,
        },
      });
    },
    edit(item) {
      this.$router.push({
        name: 'warehouse_detail',
        params: {
          form_type: 'edit',
          id: item.id,
        },
      });
    },
    inactive(item) {
      this.$swal({
        title: 'Xác nhận?',
        text: `Bạn có muốn xóa đối tượng ${item.name}`,
        icon: 'warning',
        buttons: {
          cancel: {
            text: 'Huỷ',
            value: false,
            visible: true,
            className: '',
            closeModal: true,
          },
          confirm: {
            text: 'Xác nhận',
            value: true,
            visible: true,
            className: '',
            closeModal: true,
          },
        },
      }).then(
        function (result) {
          if (result) {
            // inactive all
            this.inactiveItem(item)
              .then(() => {
                this.$swal(
                  'Thành công!',
                  'Đối tượng đã ngưng hoạt động.',
                  'success',
                );
              })
              .catch(() => {
                this.$swal('Lỗi!', 'Đã có lỗi xảy ra.', 'error');
              });
          }
        }.bind(this),
      );
    },
    async inactiveItem(item) {
      if (item.state !== 'inactive') {
        await this.$api.delete(`warehouse/${item.id}`).then(() => {
          item.isDeleted = true;
        });
      }
      return true;
    },
    loadData() {
      this.$store.commit('context/setLoading', true);
      this.$api
        .get('warehouse', this.searchParams)
        .then(({ meta, data }) => {
          this.data = data;
          this.paging.total = meta.total;
        })
        .finally(() => {
          this.$store.commit('context/setLoading', false);
        });
      return;
    },
  },
  mounted() {
    this.loadData();
  },
};
</script>
