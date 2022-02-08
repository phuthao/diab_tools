<template>
  <div class="user-app-view-page w-100">
    <basic-subheader>
      <template v-slot:actions>
        <b-button class="btn ml-3" type="button" @click="returnPage">
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Arrows/arrow-left.svg" />
          </span>
          Trở về trước
        </b-button>
        <b-button
          class="btn btn-inactive ml-3"
          type="button"
          v-if="userData.isActive && isAdmin"
          @click="handleInactive(userData)"
        >
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/power.svg" />
          </span>
          Inactive
        </b-button>
        <b-button
          class="btn btn-active ml-3"
          type="button"
          v-if="!userData.isActive && isAdmin"
          @click="handleInactive(userData)"
        >
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/power.svg" />
          </span>
          Active
        </b-button>
        <b-button
          class="btn btn-success ml-3"
          type="button"
          v-if="isAdmin"
          @click.stop="handleUpdate(userData)"
        >
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/edit-2.svg" />
          </span>
          Chỉnh sửa
        </b-button>
      </template>
    </basic-subheader>
    <b-overlay :show="loading">
      <b-container fluid class="user-app-view-page__body mb-6 mt-6">
        <b-row>
          <b-col>
            <b-row>
              <b-col>
                <div class="card card-custom gutter-b user-info-card">
                  <div class="card-body mt-0">
                    <b-row>
                      <b-col>
                        <div class="d-flex align-items-center">
                          <avatar
                            size="100px"
                            :text="userData.fullName"
                            :src="userData.avatar && userData.avatar.url"
                            :rounded="true"
                          ></avatar>
                          <div class="d-flex flex-column ml-5">
                            <h5 style="color: #515356">
                              {{ userData.fullName }}
                            </h5>
                            <p
                              class="mt-2 mb-0"
                              style="font-size: 12px; color: #888c9f"
                            >
                              {{ userData.username }}, Mã số: 17
                            </p>
                            <div class="d-flex align-items-center mt-2">
                              <span
                                class="btn btn-active"
                                style="width: 95px; cursor: default"
                                v-if="userData.isActive"
                              >
                                Active
                              </span>
                              <span
                                class="btn btn-inactive"
                                style="width: 95px; cursor: default"
                                v-if="userData.isActive === false"
                              >
                                Inactive
                              </span>
                              <span
                                class="btn btn-user-app ml-3"
                                style="width: 95px; cursor: default"
                              >
                                Portal User
                              </span>
                            </div>
                          </div>
                        </div>
                      </b-col>
                    </b-row>
                    <b-row class="mt-8">
                      <b-col>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Tuổi:</div>
                            <div>{{ userData.age }}</div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Điện thoại:</div>
                            <div>
                              {{
                                userData.phoneNumber &&
                                userData.phoneNumber.replace('+84', 0)
                              }}
                            </div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Điện thoại thay thế:</div>
                            <div>
                              {{
                                userData.secondPhoneNumber &&
                                userData.secondPhoneNumber.replace('+84', 0)
                              }}
                            </div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Email:</div>
                            <div>{{ userData.email }}</div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Tỉnh thành:</div>
                            <div>{{ userData.province }}</div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Phường / Xã:</div>
                            <div>{{ userData.ward }}</div>
                          </b-col>
                        </b-row>
                      </b-col>
                      <b-col>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Ngày sinh:</div>
                            <div>
                              {{
                                $moment(userData.dateOfBirth * 1000).format(
                                  'DD/MM/YYYY',
                                )
                              }}
                            </div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Giới tính:</div>
                            <div>
                              {{ userData.gender }}
                            </div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Quốc gia:</div>
                            <div>{{ userData.nation }}</div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Quận / Huyện:</div>
                            <div>{{ userData.district }}</div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Địa chỉ:</div>
                            <div>{{ userData.address }}</div>
                          </b-col>
                        </b-row>
                      </b-col>
                    </b-row>
                  </div>
                </div>
              </b-col>
            </b-row>
            <b-row>
              <b-col>
                <div class="card card-custom gutter-b user-time-update-card">
                  <div class="card-header h-25">
                    <div class="card-title">
                      <h5 class="card-label text-success">
                        Thời gian cập nhật
                      </h5>
                    </div>
                  </div>
                  <div class="card-body mt-0">
                    <b-row>
                      <b-col>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Thời gian tạo:</div>
                            <div>
                              {{
                                $moment(userData.createDatetime * 1000).format(
                                  'DD/MM/YYYY',
                                )
                              }}
                            </div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">Người tạo:</div>
                            <div class="d-flex align-items-center mt-2">
                              <avatar
                                v-if="userData.creator"
                                size="40px"
                                :text="userData.creator.fullName"
                                :src="
                                  userData.creator && userData.creator.avatar
                                    ? userData.creator.avatar.url
                                    : null
                                "
                                :rounded="true"
                              ></avatar>
                              <div
                                class="d-flex flex-column ml-5"
                                v-if="userData.creator"
                              >
                                <p
                                  class="mb-0"
                                  style="
                                    font-weight: 600;
                                    font-size: 13px;
                                    color: #515356;
                                  "
                                >
                                  {{ userData.creator.fullName }}
                                </p>
                                <p
                                  class="mt-2 mb-0"
                                  style="font-size: 12px; color: #888c9f"
                                >
                                  {{ userData.creator.username }}
                                </p>
                              </div>
                            </div>
                          </b-col>
                        </b-row>
                      </b-col>
                      <b-col>
                        <b-row>
                          <b-col>
                            <div class="text-muted">
                              Lần cập nhật cuối cùng:
                            </div>
                            <div>
                              {{
                                $moment(userData.updateDatetime * 1000).format(
                                  'DD/MM/YYYY',
                                )
                              }}
                            </div>
                          </b-col>
                        </b-row>
                        <b-row>
                          <b-col>
                            <div class="text-muted">
                              Người cập nhật lần cuối:
                            </div>
                            <div class="d-flex align-items-center mt-2">
                              <avatar
                                v-if="userData.updater"
                                size="40px"
                                :text="userData.updater.fullName"
                                :src="
                                  userData.updater && userData.updater.avatar
                                    ? userData.updater.avatar.url
                                    : null
                                "
                                :rounded="true"
                              ></avatar>
                              <div
                                class="d-flex flex-column ml-5"
                                v-if="userData.updater"
                              >
                                <p
                                  class="mb-0"
                                  style="
                                    font-weight: 600;
                                    font-size: 13px;
                                    color: #515356;
                                  "
                                >
                                  {{ userData.updater.fullName }}
                                </p>
                                <p
                                  class="mt-2 mb-0"
                                  style="font-size: 12px; color: #888c9f"
                                >
                                  {{ userData.updater.username }}
                                </p>
                              </div>
                            </div>
                          </b-col>
                        </b-row>
                      </b-col>
                    </b-row>
                  </div>
                </div>
              </b-col>
            </b-row>
          </b-col>
          <b-col>
            <b-row>
              <b-col>
                <div
                  class="card card-custom gutter-b user-patient-condition-card"
                >
                  <div class="card-header h-25">
                    <div class="card-title">
                      <h5 class="card-label text-success">
                        Tham gia 0 nhóm Users
                      </h5>
                    </div>
                  </div>
                  <div class="card-body mt-0">
                    <b-row>
                      <b-col>
                        <template-table
                          v-if="userData.userGroups"
                          :column="columnUserGroups"
                          :data="userData.userGroups"
                          :tableAction="false"
                          :selectAction="false"
                          :searchAction="false"
                          :pagingAction="false"
                        >
                          <template v-slot:body="{ item }">
                            <td>
                              {{ item.name }}
                            </td>
                          </template>
                        </template-table>
                      </b-col>
                    </b-row>
                  </div>
                </div>
              </b-col>
            </b-row>
            <b-row>
              <b-col>
                <div class="card card-custom gutter-b user-time-update-card">
                  <div class="card-header h-25">
                    <div class="card-title">
                      <h5 class="card-label text-success">
                        Tham gia 0 nhóm Health Coach
                      </h5>
                    </div>
                  </div>
                  <div class="card-body mt-0">
                    <b-row>
                      <b-col>
                        <template-table
                          v-if="userData.healthCoachGroups"
                          :column="columnHealthCoachGroups"
                          :data="userData.healthCoachGroups"
                          :tableAction="false"
                          :selectAction="false"
                          :searchAction="false"
                          :pagingAction="false"
                        >
                          <template v-slot:body="{ item }">
                            <td>
                              {{ item.name }}
                            </td>
                            <td>
                              {{ item.members }}
                            </td>
                          </template>
                        </template-table>
                      </b-col>
                    </b-row>
                  </div>
                </div>
              </b-col>
            </b-row>
          </b-col>
        </b-row>
      </b-container>
    </b-overlay>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      loading: false,
      isFirstLoad: false,
      userData: {},
      columnUserGroups: [
        {
          key: 'name',
          label: 'Tên Nhóm User',
          sortable: false,
        },
      ],
      columnHealthCoachGroups: [
        {
          key: 'name',
          label: 'Nhóm Health Coach',
          sortable: false,
        },
        {
          key: 'members',
          label: 'Số lượng thành viên',
          sortable: false,
        },
      ],
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
  },
  methods: {
    returnPage() {
      this.$router.push({
        name: 'user_portal_list',
      });
    },
    handleUpdate(item) {
      this.$router.push({
        name: 'user_portal_detail',
        params: {
          form_type: 'edit',
          id: item.id,
        },
      });
    },
    handleInactive(item) {
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
                this.loadUserData();
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
    async loadUserData() {
      if (!this.$route.params.id) return;
      try {
        this.loading = true;
        const id = this.$route.params.id;
        let { data } = await this.$api.get(`Admin/Account/portal/${id}`);
        this.userData = data;
      } catch (error) {
        this.$toastr.e({
          title: 'Lỗi!',
          msg: error,
        });
      } finally {
        this.loading = false;
      }
    },
  },
  mounted() {
    this.loadUserData();
  },
};
</script>
