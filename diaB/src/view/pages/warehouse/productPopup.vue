<template>
  <b-modal
    :data="data"
    id="product-modal"
    ref="product-modal"
    title="Thêm sản phẩm vào kho"
    ok-title="Lưu"
    cancel-title="Hủy"
    @show="openProductPopup"
    @hidden="resetModal"
    @ok="submit"
  >
    <div class="d-block text-center">
      <div class="row">
        <div class="col-lg-12">
          <div class="card card-custom gutter-b">
            <div class="card-body">
              <form>
                <tempalte-table
                  :btnAction="true"
                  :tableAction="false"
                  btnActionLable="Xác nhận"
                  :btnActionDisabled="scanButton"
                  :btnPrimary="false"
                  :selectAction="false"
                  @more="scanSerial"
                >
                  <div class="row mb-16">
                    <div class="col-lg-8 mb-lg-0 mb-6 ml-0">
                      <basic-input
                        label="Mã Serial: "
                        description
                        placeholder="Mã Serial"
                        :value.sync="form.code"
                        :state="validationState.externalCode"
                        :invalidFeedback="error.externalCode"
                        name="externalCode"
                      ></basic-input>
                    </div>
                    <div class="col-lg-4 mb-lg-0 mb-6 mt-8">
                      <button
                        type="submit"
                        class="btn btn-primary btn-primary--icon"
                      >
                        <span>
                          <i class="la la-search"></i>
                          <span>Tìm kiếm</span>
                        </span>
                      </button>
                    </div>
                  </div>
                </tempalte-table>
              </form>
              <template-table
                :column="column"
                :data="data"
                :paging="paging"
                :tableAction="false"
                @search="searchRequest"
                @submit="submit"
                :btnPrimary="false"
                :selectAction="false"
              >
                <template v-slot:body="{ item }">
                  <td>{{ item.product.code }}</td>
                  <td>{{ item.product.name }}</td>
                  <td>{{ item.serial.externalCode }}</td>
                  <td>{{ item.brand.name }}</td>
                </template>
              </template-table>
            </div>
          </div>
        </div>
      </div>
    </div>
    <b-button
      v-if="result && result.length"
      class="mt-3"
      block
      @click="modalShow = false"
      >Lưu sản phẩm</b-button
    >
  </b-modal>
</template>
<style>
#product-modal___BV_modal_content_ {
  width: 800px;
  left: 50%;
  transform: translateX(-50%);
}
</style>
<script>
export default {
  components: {},
  props: {
    popupType: {
      type: String,
      default: 'detail',
    },
    id: {
      type: String,
      default: null,
    },
    warehouseId: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      column: [
        {
          key: 'productUUID',
          label: 'Mã sản phẩm',
        },
        {
          key: 'productName',
          label: 'Tên sản phẩm',
        },
        {
          key: 'serial',
          label: 'Serial',
        },
        {
          key: 'brand',
          label: 'NSX',
        },
      ],
      data: [],
      serial: [],
      result: [],
      form: {
        code: null,
        name: null,
        brandId: null,
        externalCode: null,
      },
      validationState: {
        code: null,
        name: null,
        brandId: null,
        externalCode: null,
      },
      error: {
        code: null,
        name: null,
        brandId: null,
        externalCode: null,
      },
      //eslint-disable-next-line
				reg: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/,
      paging: {
        page: 1,
        pageSize: 10,
        total: 0,
      },
      selected: 1,
    };
  },
  computed: {
    // scanButton() {
    //   if (!this.filter.serial) {
    //     return true;
    //   }
    //   return false;
    // },
    staffData: {
      get() {
        let newStaff = {
          code: this.form.code,
          name: this.form.name,
          gender: this.form.gender,
          warehouseId: this.warehouseId,
        };
        return newStaff;
      },

      set(data) {
        this.form.code = data.code;
        this.form.name = data.name;
        this.form.warehouseId = data.warehouseId;
      },
    },
  },
  methods: {
    async scanSerial() {
      this.$store.commit('context/setLoading', true);
      await this.$api.get(`ProductSerial/${this.product.id}`);
      // this.$store.commit("context/setLoading", false)
      // return;
      // let data = {
      //   id: this.selectedWarehouse.id,
      //   prodcut: this.filter.serial
      // };
      // try{
      //   await this.$api.post("product/scanSerial", data);
      // };
    },
    resetModal() {
      this.form = {
        code: null,
        name: null,
        gender: 1,
        branchId: null,
        // accountId: null,
        email: null,
        department: null,
        position: null,
        phoneNumber: null,
        isDeleted: null,
      };
      this.validationState = {
        code: null,
        name: null,
        gender: 1,
        branchId: null,
        // accountId: null,
        email: null,
        department: null,
        position: null,
        phoneNumber: null,
        isDeleted: null,
      };
      this.error = {
        code: null,
        name: null,
        gender: 1,
        branchId: null,
        // accountId: null,
        email: null,
        department: null,
        position: null,
        phoneNumber: null,
        isDeleted: null,
      };
    },
    isFormValid() {
      let isValid = true;
      for (let field of Object.keys(this.form)) {
        let validationFunction = `${field}Validation`;
        if (
          this[validationFunction] &&
          typeof this[validationFunction] === 'function'
        ) {
          this[validationFunction]();
        }
      }

      for (let validate of Object.keys(this.validationState)) {
        if (this.validationState[validate] === false) {
          isValid = false;
          break;
        }
      }
      return isValid;
    },
    create() {
      this.$store.commit('context/setLoading', true);
      this.$api
        .post('product', this.staffData)
        .then(() => {
          this.$emit('reloadStaff');
          this.$toastr.s({
            title: 'Thành công!',
            msg: 'Sản phẩm thành công',
          });
          this.$nextTick(() => {
            this.$bvModal.hide('product-modal');
          });
        })
        .catch((error) => {
          if (error) {
            if (error.code === 'duplicated_code') {
              this.validationState.code = false;
              this.error.code = `Mã Nhân Viên không được trùng ${this.form.code}`;
              this.$toastr.e({
                title: 'Lỗi !',
                msg: 'Mã Nhân Viên không được trùng',
              });
            }
          }
        })
        .finally(() => {
          this.$store.commit('context/setLoading', false);
        });
    },
    edit() {
      this.$store.commit('context/setLoading', true);
      this.$api
        .put(`Staff/${this.id}`, this.staffData)
        .then(() => {
          this.$emit('reloadStaff');
          this.$toastr.s({
            title: 'Thành công!',
            msg: 'Cập nhật Nhân Viên thành công',
          });
          this.isSubmitted = true;
          this.$nextTick(() => {
            this.$bvModal.hide('product-modal');
          });
        })
        .catch((error) => {
          if (error) {
            if (error.code === 'duplicated_code') {
              this.validationState.code = false;
              this.error.code = `Mã Nhân Viên không được trùng ${this.form.code}`;
              this.$toastr.e({
                title: 'Lỗi !',
                msg: 'Mã Nhân Viên không được trùng',
              });
            }
          }
        })
        .finally(() => {
          this.$store.commit('context/setLoading', false);
        });
    },
    loadProductData() {
      this.$api.get(`/product/${this.id}`).then(({ data }) => {
        this.staffData = data;
      });
    },
    submit(bvModalEvt) {
      bvModalEvt.preventDefault();
      let isFormValid = this.isFormValid();
      if (isFormValid) {
        if (this.popupType === 'create') {
          this.create();
        }
      }
      setTimeout(() => {
        window.scrollTo(0, 0);
      }, 200);
      return;
    },
    openProductPopup() {
      if (this.popupType === 'edit') {
        this.loadProductData();
      }
    },
  },
};
</script>
