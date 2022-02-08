<template>
  <div class="user-app-detail-page w-100">
    <b-form @submit="handleValidate">
      <basic-subheader>
        <template v-slot:actions>
          <b-button
            class="btn btn-light ml-3"
            type="button"
            @click="returnPage"
          >
            <span class="svg-icon">
              <inline-svg src="/media/svg/icons/Neolex/Arrows/arrow-left.svg" />
            </span>
            Huỷ
          </b-button>
          <b-button class="btn btn-light ml-3" type="button" @click="resetData">
            <span class="svg-icon">
              <inline-svg src="/media/svg/icons/Neolex/Basic/refresh-cw.svg" />
            </span>
            Reset dữ liệu
          </b-button>
          <b-button
            class="btn btn-light ml-3"
            type="button"
            v-if="!isCreateForm"
            @click="resetPassword"
          >
            <span class="svg-icon">
              <inline-svg src="/media/svg/icons/Neolex/Security/key.svg" />
            </span>
            Reset mật khẩu
          </b-button>
          <template>
            <b-button class="btn btn-success ml-3" @click.stop="handleValidate">
              <span class="svg-icon">
                <inline-svg
                  :src="`/media/svg/icons/Neolex/Basic/${
                    isCreateForm ? 'plus' : 'save'
                  }.svg`"
                />
              </span>
              {{ isCreateForm ? 'Tạo mới' : ' Cập nhật' }}
            </b-button>
          </template>
        </template>
      </basic-subheader>
      <b-overlay :show="loading">
        <b-container fluid class="user-app-list-page__body mb-6 mt-6">
          <b-row>
            <b-col>
              <div class="card card-custom gutter-b">
                <div class="card-body mt-0">
                  <h5 class="card-title text-success">Thông tin cá nhân</h5>
                  <b-row>
                    <b-col cols="4">
                      <div
                        class="d-flex flex-column align-items-center justify-content-center"
                      >
                        <div
                          style="
                            width: 150px;
                            height: 150px;
                            background-color: #f3f6f9;
                            box-shadow: 0px 4px 4px 0px #000000 10%;
                            position: relative;
                            margin: auto;
                          "
                        >
                          <v-file-input
                            hide-input
                            v-model="file"
                            accept="image/png, image/jpeg, image/bmp"
                            class="icon-edit"
                            @change="onUpload"
                            prepend-icon="mdi-pencil-outline"
                            truncate-length="15"
                          >
                          </v-file-input>
                          <v-img :src="preview" :aspect-ratio="1 / 1"></v-img>
                          <!-- <v-icon
                            @click="clear"
                            v-if="preview"
                            style="
                              position: absolute;
                              bottom: -10px;
                              right: -10px;
                            "
                          >
                            mdi-cancel</v-icon
                          > -->
                        </div>
                        <template v-if="form_type === 'edit'">
                          <h5 class="mt-4">{{ form.fullName }}</h5>
                          <div>
                            <span
                              >{{ form.username }}, Mã số: {{ form.code }}</span
                            >
                          </div>
                          <div class="d-flex align-items-center mt-2">
                            <span
                              class="btn btn-active"
                              style="width: 95px; cursor: default"
                              v-if="form.status === 1"
                            >
                              Active
                            </span>
                            <span
                              class="btn btn-inactive"
                              style="width: 95px; cursor: default"
                              v-if="form.status === 0"
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
                        </template>
                      </div>
                    </b-col>
                    <b-col>
                      <b-row>
                        <b-col>
                          <basic-input
                            label="Họ và Tên"
                            placeholder="Nhập họ và tên"
                            name="fullName"
                            :required="true"
                            data-vv-as="Họ và tên"
                            v-validate="'required'"
                            :value.sync="form.fullName"
                            :state="validateState('fullName')"
                            :invalidFeedback="errors.first('fullName')"
                          ></basic-input>
                        </b-col>
                        <b-col>
                          <b-form-group
                            label=""
                            :state="validateState('birthday')"
                            :invalidFeedback="errors.first('birthday')"
                            class="required"
                          >
                            <label for="birthday"
                              >Ngày sinh<span class="aterisk"> *</span></label
                            >
                            <date-picker
                              label="Ngày sinh"
                              placeholder="Nhập ngày sinh"
                              name="birthday"
                              input-class="form-control"
                              type="date"
                              :required="true"
                              format="DD/MM/YYYY"
                              value-type="timestamp"
                              :disabled-date="notBeforeToday"
                              v-validate="'required'"
                              data-vv-as="Ngày sinh"
                              :state="validateState('birthday')"
                              :invalidFeedback="errors.first('birthday')"
                              v-model="form.dateOfBirth"
                            >
                            </date-picker>
                          </b-form-group>
                        </b-col>
                      </b-row>
                      <b-row>
                        <b-col>
                          <basic-select
                            label="Giới tính"
                            placeholder="--- Chọn giới tính ---"
                            name="gender"
                            :required="true"
                            data-vv-as="giới tính"
                            v-validate="'required'"
                            :options="isGenderOpts"
                            :value.sync="form.gender"
                            :state="validateState('gender')"
                            :invalidFeedback="errors.first('gender')"
                            changeValueByObject
                            valueLabel="name"
                          />
                        </b-col>
                        <b-col>
                          <basic-select
                            label="Quốc gia"
                            placeholder="--- Chọn quốc gia ---"
                            name="nation"
                            :apiPath="'/Division/Nations'"
                            :searchField="'searchTerm'"
                            :changeValueByObject="true"
                            :value.sync="form.nation"
                            :searchable="false"
                            valueLabel="name"
                          />
                        </b-col>
                      </b-row>
                      <b-row>
                        <b-col>
                          <basic-select
                            label="Tỉnh thành"
                            placeholder="--- Chọn tỉnh thành ---"
                            name="province"
                            :apiPath="'/Division/Provinces'"
                            :searchField="'searchTerm'"
                            :searchable="false"
                            valueLabel="name"
                            :changeValueByObject="true"
                            :searchParams="{ nationId: nationId }"
                            :value.sync="form.province"
                          />
                        </b-col>
                        <b-col>
                          <basic-select
                            label="Quận / huyện"
                            placeholder="--- Chọn quận / huyện ---"
                            name="district"
                            :apiPath="'/Division/Dictricts'"
                            :searchField="'searchTerm'"
                            :searchParams="{ provinceId: provinceId }"
                            :value.sync="form.district"
                            :changeValueByObject="true"
                            :searchable="false"
                            valueLabel="name"
                          />
                        </b-col>
                      </b-row>
                      <b-row>
                        <b-col>
                          <basic-select
                            label="Phường xã"
                            placeholder="--- Chọn phường xã ---"
                            name="wards"
                            :apiPath="'/Division/Wards'"
                            :searchField="'searchTerm'"
                            :searchParams="{ districtId: districtId }"
                            :value.sync="form.wards"
                            :changeValueByObject="true"
                            :searchable="false"
                            valueLabel="name"
                          />
                        </b-col>
                        <b-col>
                          <basic-input
                            label="Địa chỉ"
                            placeholder="Nhập địa chỉ"
                            name="address"
                            :value.sync="form.address"
                          ></basic-input>
                        </b-col>
                      </b-row>
                      <b-row>
                        <b-col cols="6">
                          <basic-select
                            label="Trực thuộc bệnh viện"
                            placeholder="--- Chọn bệnh viện ---"
                            name="hospital"
                            :solid="false"
                            :allowEmpty="true"
                            disabled
                            :options="[]"
                            :value.sync="form.hospitalId"
                          />
                        </b-col>
                      </b-row>
                    </b-col>
                  </b-row>
                  <hr style="margin: 2rem -2.25rem" />
                  <h5 class="card-title text-success">Tài khoản</h5>
                  <b-row>
                    <b-col>
                      <basic-input
                        label="Tên đăng nhập"
                        placeholder="Nhập username"
                        name="username"
                        :required="true"
                        :value.sync="form.username"
                        v-validate="'required'"
                        data-vv-as="Tên đăng nhập"
                        :state="validateState('username')"
                        :invalidFeedback="errors.first('username')"
                      ></basic-input>
                    </b-col>
                    <b-col>
                      <basic-input
                        label="Số điện thoại"
                        placeholder="Nhập số điện thoại"
                        name="phoneNumber"
                        :required="true"
                        v-validate="{ required: true, length: 10 }"
                        inputType="number"
                        data-vv-as="Số điện thoại"
                        :value.sync="form.phoneNumber"
                        :state="validateState('phoneNumber')"
                        :invalidFeedback="errors.first('phoneNumber')"
                      ></basic-input>
                    </b-col>
                    <b-col>
                      <basic-input
                        label="Số điện thoại thay thế"
                        placeholder="Nhập số điện thoại"
                        name="secondPhoneNumber"
                        v-validate="'length:10'"
                        inputType="number"
                        data-vv-as="Số điện thoại thay thế"
                        :value.sync="form.secondPhoneNumber"
                        :state="validateState('secondPhoneNumber')"
                        :invalidFeedback="errors.first('secondPhoneNumber')"
                      ></basic-input>
                    </b-col>
                    <b-col>
                      <basic-input
                        label="Email"
                        placeholder="Nhập email"
                        name="email"
                        :required="true"
                        v-validate="'required|email'"
                        inputType="email"
                        data-vv-as="Email"
                        :state="validateState('email')"
                        :invalidFeedback="errors.first('email')"
                        :value.sync="form.email"
                      ></basic-input>
                    </b-col>
                  </b-row>
                  <b-row v-if="isCreateForm">
                    <b-col cols="3">
                      <basic-input
                        label="Mật khẩu"
                        placeholder="Mật khẩu"
                        name="password"
                        :required="true"
                        v-validate="'required|min:6'"
                        inputType="password"
                        data-vv-as="Mật khẩu"
                        :state="validateState('password')"
                        :invalidFeedback="errors.first('password')"
                        :value.sync="form.password"
                      ></basic-input>
                    </b-col>
                    <b-col cols="3">
                      <basic-input
                        label="Xác nhận mật khẩu"
                        :required="true"
                        placeholder="Xác nhận mật khẩu"
                        name="confirm-password"
                        v-validate="'required|confirmed:password'"
                        inputType="password"
                        data-vv-as="Mật khẩu xác nhận"
                        :state="validateState('confirm-password')"
                        :invalidFeedback="errors.first('confirm-password')"
                        :value.sync="form.confirm"
                      ></basic-input>
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col>
                      <b-form-checkbox
                        class="required"
                        name="position"
                        v-model="form.changePassword"
                        value="true"
                        unchecked-value="false"
                      >
                        Bắt buộc thay đổi mật khẩu cho lần đăng nhập kế tiếp
                      </b-form-checkbox>
                    </b-col>
                  </b-row>
                  <hr style="margin: 2rem -2.25rem" />
                  <h5 class="card-title text-success">Nhóm Users</h5>
                  <b-row>
                    <b-col>
                      <basic-select
                        placeholder="--- Chọn nhóm Users ---"
                        name="userGroups"
                        :solid="false"
                        :allowEmpty="true"
                        :options="[]"
                        label=""
                        disabled
                        :value.sync="form.userGroups"
                      />
                    </b-col>
                  </b-row>
                  <hr style="margin: 2rem -2.25rem" />
                  <h5 class="card-title text-success">Nhóm Health Coach</h5>
                  <b-row>
                    <b-col>
                      <basic-select
                        placeholder="--- Chọn nhóm Health Coach ---"
                        name="healthCoachGroups"
                        :solid="false"
                        :allowEmpty="true"
                        label=""
                        :options="[]"
                        disabled
                        :value.sync="form.healthCoachGroups"
                      />
                    </b-col>
                  </b-row>
                </div>
                <div
                  class="card-footer d-flex align-items-lg-center justify-content-center"
                >
                  <b-button
                    class="btn btn-light ml-3"
                    type="button"
                    @click.stop="resetData"
                  >
                    <span class="svg-icon">
                      <inline-svg
                        src="/media/svg/icons/Neolex/Basic/refresh-cw.svg"
                      />
                    </span>
                    Reset dữ liệu
                  </b-button>
                  <b-button
                    class="btn btn-success ml-3"
                    @click.stop="handleValidate"
                  >
                    <span class="svg-icon">
                      <inline-svg
                        src="/media/svg/icons/Neolex/Basic/save.svg"
                      />
                    </span>
                    {{ isCreateForm ? 'Tạo mới' : 'Cập nhật' }}
                  </b-button>
                </div>
              </div>
            </b-col>
          </b-row>
        </b-container>
      </b-overlay>
    </b-form>
  </div>
</template>

<script>
import DatePicker from 'vue2-datepicker';
import 'vue2-datepicker/index.css';
export default {
  components: {
    'date-picker': DatePicker,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
    form_type: {
      type: String,
      default: 'edit',
    },
  },
  data() {
    return {
      isGenderOpts: [
        { id: 1, name: 'Nam' },
        { id: 2, name: 'Nữ' },
      ],
      isServicePackOpts: [
        { id: 0, name: 'Thành viên Cơ Bản' },
        { id: 1, name: 'Thành viên Đồng' },
        { id: 2, name: 'Thành viên Bạc' },
        { id: 3, name: 'Thành viên Vàng' },
      ],
      isDiabetesConditionOpts: [
        { id: 0, name: 'Tiểu đường loại 1' },
        { id: 1, name: 'Tiểu đường loại 2' },
        { id: 2, name: 'Tiểu đường loại 3' },
        { id: 3, name: 'Tiểu đường loại 4' },
      ],
      form: {},
      file: null,
      preview: null,
      loading: false,
    };
  },
  created() {
    this.loadUserData();
  },
  computed: {
    nationId() {
      return this.form.nation?.id;
    },
    provinceId() {
      return this.form.province?.id;
    },
    districtId() {
      return this.form.district?.id;
    },
    isCreateForm() {
      return this.form_type === 'create' || this.form_type === 'copy';
    },
  },
  methods: {
    resetData() {
      this.form.id = null;
      this.form.fullName = null;
      this.form.dateOfBirth = null;
      this.form.gender = null;
      this.form.phoneNumber = null;
      this.form.secondPhoneNumber = null;
      this.form.email = null;
      this.form.address = null;
      this.form.username = null;
      this.file = null;
      this.preview = null;
      this.form.changePassword = [];
      this.form.password = null;
      this.form.confirm = null;
      this.form.nation = {
        id: null,
        name: null,
      };
      this.form.province = {
        id: null,
        name: null,
      };
      this.form.district = {
        id: null,
        name: null,
      };
      this.form.wards = {
        id: null,
        name: null,
      };
      this.$nextTick(() => {
        this.$validator.reset();
      });
    },
    clear() {
      this.file = null;
      this.preview = null;
    },

    async resetPassword() {
      let id = this.$route.params.id;
      let confirm = await this.$swal({
        text: `Mật khẩu mới sẽ được gởi qua email của tài khoản! Bạn có chắc muốn reset mật khẩu cho ${this.form.fullName} không?`,
        icon: 'warning',
        buttons: {
          cancel: {
            text: 'Quay lại',
            value: false,
            visible: true,
            className: '',
            closeModal: true,
          },
          confirm: {
            text: 'Reset',
            value: true,
            visible: true,
            className: 'btn-success',
            closeModal: true,
          },
        },
      });
      if (!confirm) return;
      try {
        this.loading = true;

        await this.$api.post(`Admin/Account/${id}/reset-password`);
        this.$swal({
          text: `Reset mật khẩu thành công! Mật khẩu mới đã được gởi qua tin nhắn SMS và email của tài khoản.`,
          icon: 'success',
          confirm: {
            text: 'OK',
            value: true,
            visible: true,
            className: 'btn-success',
            closeModal: true,
          },
        });
      } catch (e) {
        this.$toastr.e({
          title: 'Lỗi!',
          msg: e,
        });
      } finally {
        this.loading = false;
      }
    },
    onUpload() {
      this.preview = URL.createObjectURL(this.file);
    },
    notBeforeToday(date) {
      return date > new Date();
    },
    validateState(ref) {
      if (
        this.veeFields[ref] &&
        (this.veeFields[ref].dirty || this.veeFields[ref].validated)
      ) {
        return !this.errors.has(ref);
      }
      return null;
    },
    async returnPage() {
      (await this.handleConfirm())
        ? this.$router.push({
            name: 'user_portal_list',
          })
        : null;
    },
    async handleConfirm() {
      let confirm = await this.$swal({
        text: 'Các thay đổi sẽ không được lưu trữ, bạn có chắc muốn huỷ?',
        icon: 'warning',
        buttons: {
          cancel: {
            text: 'Quay lại',
            value: false,
            visible: true,
            className: '',
            closeModal: true,
          },
          confirm: {
            text: 'Huỷ',
            value: true,
            visible: true,
            className: 'btn-warning',
            closeModal: true,
          },
        },
      });
      return confirm;
    },
    async handleValidate() {
      let result = await this.$validator.validateAll();
      if (!result) return;
      else {
        this.submit();
      }
    },
    async submit() {
      let payload = new FormData();
      const isCreateForm = this.isCreateForm;
      this.form.fullName && payload.append('fullName', this.form.fullName);
      this.form.phoneNumber &&
        payload.append('phoneNumber', this.form.phoneNumber);
      this.form.secondPhoneNumber &&
        payload.append('secondPhoneNumber', this.form.secondPhoneNumber);
      this.form.gender && payload.append('gender', this.form.gender.id);
      this.form.nation?.id && payload.append('nationId', this.form.nation.id);
      this.form.province?.id &&
        payload.append('provinceId', this.form.province.id);
      this.form.district?.id &&
        payload.append('districtId', this.form.district.id);
      this.form.wards?.id && payload.append('wardId', this.form.wards.id);
      // this.form.hospitalId &&
      //   payload.append('hospitalId', this.form.hospitalId);
      this.file && payload.append('image', this.file);
      this.form.username && payload.append('username', this.form.username);
      this.form.email && payload.append('email', this.form.email);
      this.form.dateOfBirth &&
        payload.append('dateOfBirth', Math.floor(this.form.dateOfBirth / 1000));
      this.form.address && payload.append('address', this.form.address);
      this.form.changePassword !== undefined &&
        payload.append('changePassword', this.form.changePassword);
      this.form.password && payload.append('password', this.form.password);
      let confirm = await this.$swal({
        text: `${
          isCreateForm
            ? 'Bạn có chắc muốn tạo người dùng Portal mới này không? '
            : 'Bạn có chắc muốn cập nhật thông tin này hay không?'
        }`,
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
            className: 'btn-success',
            closeModal: true,
          },
        },
      });
      if (confirm) {
        if (isCreateForm) {
          this.handleCreate(payload);
        } else {
          this.handleUpdate(payload);
        }
      }
    },
    async handleCreate(payload) {
      try {
        this.loading = true;
        await this.$api.post('Admin/Account/portal', payload);
        this.$toastr.s({
          title: 'Thành công!',
          msg: 'Chúc mừng, người dùng Portal mới đã được tạo!',
        });
        this.$router.push({ name: 'user_portal_list' });
      } catch (e) {
        this.$toastr.e({
          title: 'Lỗi!',
          msg: e.message,
        });
      } finally {
        this.loading = false;
      }
    },
    async handleUpdate(payload) {
      payload.append('id', this.form.id);
      try {
        this.loading = true;
        await this.$api.put('Admin/Account/portal', payload);
        this.$toastr.s({
          title: 'Thành công!',
          msg: 'Chúc mừng, người dùng Portal đã được cập nhật!',
        });
        this.$router.push({ name: 'user_portal_list' });
      } catch (e) {
        this.$toastr.e({
          title: 'Lỗi!',
          msg: e.message,
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
        data.phoneNumber =
          data.phoneNumber && data.phoneNumber.replace('+84', 0);
        data.secondPhoneNumber =
          data.secondPhoneNumber && data.secondPhoneNumber.replace('+84', 0);
        this.form = data;
        this.form.dateOfBirth = data.dateOfBirth * 1000;
        this.form.nation = { id: data.nationId, name: data.nation };
        this.form.province = { id: data.provinceId, name: data.province };
        this.form.district = { id: data.districtId, name: data.district };
        this.form.wards = { id: data.wardId, name: data.ward };
        this.form.gender = this.isGenderOpts.find(
          (e) => e.name === data.gender,
        );
        this.form.changePassword = data.changePassword;
        this.preview = data.avatar?.url;
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
};
</script>
<style lang="scss" scoped>
.pictureInput {
  position: relative;
}
.icon-edit {
  z-index: 100;
  position: absolute;
  top: -15px;
  right: -6px;
  padding: 2px 6px;
  box-shadow: -1px 2px 10px rgba(0, 0, 0, 0.2);
  border-radius: 50%;
  display: flex;
  align-items: center;
  width: fit-content;
  background-color: #fff;
}
.aterisk {
  color: #f64e60;
  font-size: 0.9rem;
}
</style>
