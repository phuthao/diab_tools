<template>
  <b-modal
    id="user-profile"
    ref="user-profile"
    :title="`Thông tin của bạn`"
    size="xl"
    hide-footer
    @show="show"
    dialog-class="full-width"
    style="padding-left: 0"
  >
    <b-container fluid>
      <b-row>
        <b-col>
          <b-overlay :show="loading">
            <div class="d-flex justify-content-between w-100">
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
                  id="picture-input"
                  @change="onUpload"
                  prepend-icon="mdi-pencil-outline"
                  truncate-length="15"
                >
                </v-file-input>
                <v-img :src="preview" :aspect-ratio="1 / 1"></v-img>
              </div>
            </div>
          </b-overlay>
        </b-col>
      </b-row>
      <b-row>
        <b-col cols="12">
          <label for="" class="text-bold">Thông tin cá nhân</label>
        </b-col>
        <b-col cols="12">
          <div class="d-flex justify-content-between">
            <span class="label-user">Họ & Tên</span>
            <span>{{ profile.fullName }}</span>
          </div>
        </b-col>
        <b-col cols="12">
          <div class="d-flex justify-content-between">
            <span class="label-user">Mã nhân viên</span>
            <span>{{ profile.code }}</span>
          </div>
        </b-col>
        <b-col cols="12">
          <div class="d-flex justify-content-between">
            <span class="label-user">Ngày sinh (tuổi)</span>
            <span>{{ profile.age }}</span>
          </div>
        </b-col>
        <b-col cols="12">
          <div class="d-flex justify-content-between">
            <span class="label-user">Trực thuộc bệnh viện</span>
            <span></span>
          </div>
        </b-col>
      </b-row>
      <b-row>
        <b-col cols="12">
          <label for="" class="text-bold">Thông tin liên lạc</label>
        </b-col>
      </b-row>
      <b-row>
        <b-col cols="12">
          <div class="d-flex justify-content-between">
            <span class="label-user">Số điện thoại</span>
            <span>{{ profile.phoneNumber }}</span>
          </div>
        </b-col>
      </b-row>
      <b-row>
        <b-col cols="12">
          <div class="d-flex justify-content-between">
            <span class="label-user">Email</span>
            <span>{{ profile.email }}</span>
          </div>
        </b-col>
      </b-row>
      <b-row>
        <b-col cols="12">
          <div class="d-flex justify-content-between">
            <span class="label-user">Tỉnh thành</span>
            <span>{{ profile.province }}</span>
          </div>
        </b-col>
      </b-row>
      <b-row>
        <b-col cols="12">
          <div class="d-flex justify-content-between">
            <span class="label-user">Quận / Huyện</span>
            <span>{{ profile.district }}</span>
          </div>
        </b-col>
      </b-row>
      <b-row>
        <b-col>
          <div class="d-flex justify-content-between">
            <span class="label-user">Phường / Xã</span>
            <span>{{ profile.ward }}</span>
          </div>
        </b-col>
      </b-row>
      <b-row>
        <b-col>
          <div class="d-flex justify-content-between">
            <span class="label-user">Địa chỉ</span>
            <span>{{ profile.address }}</span>
          </div>
        </b-col>
      </b-row>
    </b-container>
  </b-modal>
</template>

<script>
import { GET_USER } from '@/core/services/store/auth.module';
import { mapGetters } from 'vuex';
export default {
  name: 'ModalInputDetail',
  props: {
    data: {
      type: Object,
    },
  },
  data() {
    return {
      preview: null,
      file: null,
      loading: false,
    };
  },

  methods: {
    show() {
      this.profile.avatar && this.profile.avatar.url
        ? (this.preview = this.profile.avatar.url)
        : '/media/placeholder/placeholder-image.png';
    },
    onUpload() {
      this.preview = URL.createObjectURL(this.file);
      let payload = new FormData();
      this.file && payload.append('image', this.file);
      this.handleUpload(payload);
    },
    async handleUpload(payload) {
      try {
        this.loading = true;
        await this.$api.post('admin/Account/Current/avatar', payload);
        this.$toastr.s({
          title: 'Thành công!',
          msg: 'Cập nhật thành công',
        });
        this.$store.dispatch(GET_USER);
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

  computed: {
    ...mapGetters({ profile: 'currentUser' }),
  },
};
</script>

<style lang="scss" scoped>
.avatar {
  width: 150px;
  height: 150px;
  margin: auto;
  border-radius: 3px;
  border: 1px solid #ccc;
}
.pictureInput {
  position: relative;
}
.icon-edit {
  z-index: 100;
  position: absolute;
  top: -15px;
  right: -6px;
  padding: 0px 6px;
  box-shadow: -1px 2px 10px rgba(0, 0, 0, 0.2);
  border-radius: 50%;
  display: flex;
  align-items: center;
  width: fit-content;
  background-color: #fff;
}
</style>
<style lang="scss">
.selectRange-container {
  width: 200px;
}
.selectRange {
  background-color: #e4e6ef;
  border-color: #e4e6ef;
  padding: 8px 13px;
  border-radius: 6px;
  .vs__dropdown-toggle {
    border: 0;
  }
}
.text-bold {
  font-weight: bold;
  font-size: 1.2rem;
}
</style>
