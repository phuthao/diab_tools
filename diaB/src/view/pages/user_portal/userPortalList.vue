<template>
  <div class="user-app-list-page w-100">
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
                    label="Mã nhân viên"
                    placeholder="Nhập mã nhân viên"
                    name="code"
                    :value.sync="filter.code"
                  ></basic-input>
                </b-col>
                <b-col>
                  <basic-input
                    label="Tên nhân viên"
                    placeholder="Nhập tên nhân viên"
                    name="name"
                    :value.sync="filter.name"
                  ></basic-input>
                </b-col>
                <b-col>
                  <basic-select
                    label="Giới tính"
                    placeholder="--- Chọn giới tính ---"
                    name="gender"
                    :options="isGenderOpts"
                    :value.sync="filter.gender"
                    :solid="false"
                    :allowEmpty="true"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col>
                  <b-row>
                    <b-col>
                      <label for="age">Độ tuổi</label>
                      <b-row>
                        <b-col>
                          <basic-input
                            placeholder="Từ"
                            name="ageFrom"
                            v-validate="'numeric'"
                            data-vv-as="Tuổi từ"
                            :state="validateState('ageFrom')"
                            :invalidFeedback="errors.first('ageFrom')"
                            inputType="number"
                            :value.sync="filter.ageFrom"
                          ></basic-input>
                        </b-col>
                        <b-col>
                          <basic-input
                            placeholder="Đến"
                            name="ageTo"
                            v-validate="'numeric'"
                            data-vv-as="Tuổi đến"
                            :state="validateState('ageTo')"
                            :invalidFeedback="errors.first('ageTo')"
                            inputType="number"
                            :value.sync="filter.ageTo"
                          ></basic-input>
                        </b-col>
                      </b-row>
                    </b-col>
                  </b-row>
                </b-col>
                <b-col>
                  <basic-select
                    label="Bệnh viện"
                    placeholder="--- Chọn ---"
                    name="hospital"
                    disabled
                    :options="[]"
                    :value.sync="filter.hospital"
                    :solid="false"
                    :allowEmpty="true"
                  />
                </b-col>
                <b-col>
                  <basic-select
                    label="Thuộc nhóm Health Coach"
                    placeholder="--- Chọn nhóm Health Coach ---"
                    name="healthCoachGroup"
                    :options="[]"
                    disabled
                    :value.sync="filter.healthCoachGroup"
                    :solid="false"
                    :allowEmpty="true"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col>
                  <basic-select
                    label="Thuộc nhóm user"
                    placeholder="--- Chọn nhóm user ---"
                    name="userGroup"
                    disabled
                    :options="[]"
                    :value.sync="filter.userGroup"
                    :solid="false"
                    :allowEmpty="true"
                  />
                </b-col>
                <b-col>
                  <basic-select
                    label="Quốc gia"
                    placeholder="--- Chọn quốc gia ---"
                    name="nation"
                    :apiPath="'/Division/Nations'"
                    :searchField="'searchTerm'"
                    :value.sync="filter.nationId"
                    :solid="false"
                    :allowEmpty="true"
                  />
                </b-col>
                <b-col>
                  <basic-select
                    label="Tỉnh thành"
                    placeholder="--- Chọn tỉnh thành ---"
                    name="province"
                    :apiPath="'/Division/Provinces'"
                    :searchField="'searchTerm'"
                    :searchParams="{ nationId: filter.nationId }"
                    :value.sync="filter.provinceId"
                    :solid="false"
                    :allowEmpty="true"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col>
                  <basic-select
                    label="Quận / huyện"
                    placeholder="--- Chọn quận / huyện ---"
                    name="district"
                    :apiPath="'/Division/Dictricts'"
                    :searchField="'searchTerm'"
                    :searchParams="{ provinceId: filter.provinceId }"
                    :value.sync="filter.districtId"
                    :solid="false"
                    :allowEmpty="true"
                  />
                </b-col>
                <b-col>
                  <basic-select
                    label="Phường xã"
                    placeholder="--- Chọn phường xã ---"
                    name="wards"
                    :apiPath="'/Division/Wards'"
                    :searchField="'searchTerm'"
                    :searchParams="{ districtId: filter.districtId }"
                    :value.sync="filter.wardId"
                    :solid="false"
                    :allowEmpty="true"
                  />
                </b-col>
                <b-col> </b-col>
              </b-row>
              <b-row>
                <b-col>
                  <b-form-checkbox-group
                    v-model="filter.excludeInactive"
                    :options="[
                      { text: 'Lọc người dùng Inactive', value: true },
                    ]"
                  ></b-form-checkbox-group>
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
              @click="resetRequest"
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
        <b-button
          class="btn btn-success ml-2"
          type="button"
          @click="importItems"
        >
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/upload-cloud.svg" />
          </span>
          Import danh sách
        </b-button>
        <b-button
          class="btn btn-success ml-2"
          type="button"
          @click="createItem"
          v-if="isAdmin"
        >
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/plus.svg" />
          </span>
          Thêm tài khoản
        </b-button>
      </template>
    </basic-subheader>
    <b-overlay :show="loading">
      <b-container fluid class="user-app-list-page__body mb-6 mt-6">
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
                  @search="searchRequest"
                  @reset="resetRequest"
                  @sortBy="sortRequest"
                >
                  <template v-slot:body="{ item }">
                    <td>
                      <div class="d-flex align-items-center mt-5">
                        <avatar
                          size="40px"
                          :text="item.fullName"
                          :src="item.avatar && item.avatar.url"
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
                            {{ item.fullName }}
                          </p>
                          <p
                            class="mt-2 mb-0"
                            style="font-size: 12px; color: #888c9f"
                          >
                            {{ item.username }}
                          </p>
                        </div>
                      </div>
                    </td>
                    <td style="width: 20px">
                      <action-dropdown
                        :value="item"
                        @view="viewItem"
                        @edit="editItem"
                        :show_edit="isAdmin"
                        :show_delete="false"
                        :show_copy="isAdmin"
                        @copy="copyItem"
                      >
                        <b-dropdown-text
                          v-if="isAdmin"
                          tag="div"
                          class="navi-item cursor-pointer"
                        >
                          <a
                            class="navi-link cursor-pointer"
                            @click="deleteItem(item)"
                          >
                            <span
                              class="menu-icon svg-icon svg-icon-sm"
                              :class="
                                item.isActive ? 'text-danger' : 'text-primary'
                              "
                            >
                              <inline-svg
                                class="svg-icon"
                                src="/media/svg/icons/Neolex/Basic/power.svg"
                              />
                            </span>
                            <span
                              class="success navi-text"
                              :class="
                                item.isActive ? 'text-danger' : 'text-primary'
                              "
                            >
                              {{ item.isActive ? 'Inactive' : 'Active' }}
                            </span>
                          </a>
                        </b-dropdown-text>
                      </action-dropdown>
                    </td>
                    <td>
                      {{ item.age }}
                    </td>
                    <td>
                      {{ item.gender }}
                    </td>
                    <td>
                      {{ item.hospital }}
                    </td>
                    <td>
                      {{ item.usersGroup }}
                    </td>
                    <td>
                      {{ item.healthCoachGroup }}
                    </td>
                  </template>
                </template-table>
              </div>
            </div>
          </b-col>
        </b-row>
      </b-container>
    </b-overlay>
  </div>
</template>

<style lang="scss" scoped>
.dropdown-form-filter {
  .dropdown-menu {
    .container {
      width: 850px;
    }
  }
}
</style>

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
        code: null,
        name: null,
        gender: null,
        ageFrom: null,
        ageTo: null,
        hospital: null,
        nation: null,
        provinceId: null,
        district: null,
        wards: null,
        excludeInactive: null,
      },
      sort: {
        by: null,
        order: null,
      },
      isActiveOpts: [
        { id: true, name: 'Active' },
        { id: false, name: 'Inactive' },
      ],
      isGenderOpts: [
        { id: 1, name: 'Nam' },
        { id: 2, name: 'Nữ' },
      ],
      column: [
        {
          key: 'username',
          label: 'Nhân viên',
          sortable: true,
        },
        {
          key: 'actionDropdown',
          label: '',
          sortable: false,
        },
        {
          key: 'age',
          label: 'Độ tuổi',
          sortable: false,
        },
        {
          key: 'gender',
          label: 'Giới tính',
          sortable: false,
        },
        {
          key: 'hospital',
          label: 'Bệnh Viện',
          sortable: false,
        },
        {
          key: 'usersGroup',
          label: 'Nhóm Users',
          sortable: false,
        },
        {
          key: 'healthCoachGroup',
          label: 'Nhóm Health Coach',
          sortable: false,
        },
      ],
      data: [],
      loading: false,
    };
  },
  computed: {
    ...mapGetters(['currentUser']),
    isAdmin() {
      let roles = this.currentUser?.roles;
      if (roles && roles.length) {
        return roles.includes('Admin');
      } else {
        return false;
      }
    },
    searchParams() {
      return {
        name: this.filter.name,
        excludeInactive:
          this.filter.excludeInactive?.[0] == true ? true : false,
        sortBy: this.sort.by ? `${this.sort.by} ${this.sort.order}` : null,
        code: this.filter.code,
        gender: this.filter.gender,
        ageFrom: this.filter.ageFrom,
        ageTo: this.filter.ageTo,
        nationId: this.filter.nationId,
        provinceId: this.filter.provinceId,
        districtId: this.filter.districtId,
        wardId: this.filter.wardId,
        page: this.paging.page,
        size: this.paging.pageSize,
      };
    },
  },
  watch: {
    paging: {
      handler() {
        this.loadData();
      },
      deep: true,
    },
    sort: {
      handler() {
        this.loadData();
      },
      deep: true,
    },
  },
  methods: {
    pagingAction() {
      this.loadData();
    },
    searchRequest() {
      this.loadData();
    },
    resetRequest() {
      this.filter.code = null;
      this.filter.name = null;
      this.filter.gender = null;
      this.filter.ageFrom = null;
      this.filter.ageTo = null;
      this.filter.hospital = null;
      this.filter.nationId = null;
      this.filter.provinceId = null;
      this.filter.districtId = null;
      this.filter.wardId = null;
      this.filter.excludeInactive = [];
      this.$nextTick(() => {
        this.$validator.reset();
      });
      this.loadData();
    },
    sortRequest(sortData) {
      this.sort = {
        by: sortData.column,
        order: sortData.order,
      };
      return;
    },
    viewItem(item) {
      this.$router.push({
        name: 'user_portal_view',
        params: {
          id: item.id,
        },
      });
    },
    editItem(item) {
      this.$router.push({
        name: 'user_portal_detail',
        params: {
          form_type: 'edit',
          id: item.id,
        },
      });
    },
    createItem() {
      this.$router.push({
        name: 'user_portal_detail',
        params: {
          form_type: 'create',
        },
      });
    },
    validateState(ref) {
      if (
        this.veeFields[ref] &&
        this.veeFields[ref].dirty &&
        this.veeFields[ref].validated
      ) {
        return this.veeFields[ref].valid;
      }
      return null;
    },
    importItems() {},
    deleteItem(item) {
      let text = item.isActive ? 'Inactive' : 'Active';
      let btn = item.isActive ? 'btn-inactive' : 'btn-active';
      this.$swal({
        title: '',
        text: `Bạn có chắc muốn ${text} app user ${item.fullName} không?`,
        icon: '/media/svg/icons/SweetAlert/alert-triangle-red.svg',
        buttons: {
          cancel: {
            text: 'Quay lại',
            value: false,
            visible: true,
            className: 'btn btn-secondary',
            closeModal: true,
          },
          confirm: {
            text: `${text}`,
            value: true,
            visible: true,
            className: `btn ${btn}`,
            closeModal: true,
          },
        },
      }).then(
        function (result) {
          if (result) {
            // inactive all
            this.inactiveItem(item)
              .then(() => {
                this.loadData();
                this.$swal(
                  'Thành công!',
                  'Chúc mừng, bạn đã cập nhật thông tin người dùng thành công!',
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
    copyItem(item) {
      this.$router.push({
        name: 'user_portal_detail',
        params: {
          form_type: 'copy',
          id: item.id,
        },
      });
    },

    async inactiveItem(item) {
      this.loading = true;
      try {
        let payload = new FormData();
        payload.append('id', item.id);
        payload.append('active', !item.isActive);
        await this.$api.put(`Admin/Account/portal`, payload);
      } catch (error) {
        this.$toastr.e({
          title: 'Lỗi!',
          msg: error,
        });
      } finally {
        this.loading = false;
      }
    },
    loadData() {
      this.$store.commit('context/setLoading', true);
      this.$api
        .get('Admin/Account/portal', {
          params: { ...this.searchParams },
        })
        .then(({ data }) => {
          this.data = data.items || [];
          this.paging.total = data.total;
        })
        .catch((error) => {
          this.$toastr.e({
            title: 'Lỗi',
            msg: error,
          });
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
