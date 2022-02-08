<template>
  <basic-form-tab
    :form_type="form_type"
    :previousButton="true"
    :goBackButton="true"
    :submitButton="false"
    :formData="form"
    :formType="form_type"
    @submit="submit"
    ref="formTab"
  >
    <basic-tab
      :index="1"
      title="Thông tin kho"
      description="Thông tin chung về kho hàng"
    >
      <template v-slot:tab-title>
        <b-row align-h="center" class="mt-10 mb-10 font-weight-bold text-dark">
          <b-col cols="8">
            <h2>Thông tin chi tiết</h2>
          </b-col>
        </b-row>
      </template>
      <template v-slot>
        <b-row align-h="center">
          <b-col cols="8">
            <b-row>
              <b-col>
                <basic-input
                  label="Mã kho* :"
                  description="Mã của kho"
                  placeholder="Nhập mã của kho"
                  :value.sync="form.code"
                  :state="validationState.code"
                  :invalidFeedback="error.code"
                  name="code"
                  disabled
                ></basic-input>
              </b-col>
            </b-row>

            <b-row>
              <b-col>
                <basic-input
                  label="Tên kho* :"
                  description="Tên của kho"
                  placeholder="Nhập tên của kho"
                  :value.sync="form.name"
                  name="name"
                  :state="validationState.name"
                  :invalidFeedback="error.name"
                  disabled
                ></basic-input>
              </b-col>
            </b-row>

            <b-row>
              <b-col>
                <basic-input
                  label="Tên khác:"
                  description="Tên khác của kho"
                  placeholder="Nhập tên khác của kho"
                  :value.sync="form.otherName"
                  name="otherName"
                  :state="validationState.otherName"
                  :invalidFeedback="error.otherName"
                  disabled
                ></basic-input>
              </b-col>
            </b-row>

            <b-row>
              <b-col>
                <basic-text-area
                  label="Ghi chú:"
                  description="Ghi chú"
                  placeholder="Nhập ghi chú"
                  :value.sync="form.note"
                  name="note"
                  :state="validationState.note"
                  :invalidFeedback="error.note"
                  disabled
                ></basic-text-area>
              </b-col>
            </b-row>
            <b-row>
              <b-col>
                <basic-radio
                  label="Trạng thái:"
                  :labelCols="3"
                  inline
                  name="isDelete"
                  :value.sync="form.isDelete"
                  v-model="form.isDeleted"
                  isDeletedRadio
                  :disableRadio="true"
                ></basic-radio>
              </b-col>
            </b-row>
          </b-col>
        </b-row>
      </template>
    </basic-tab>
    <basic-tab
      :disabled="disabledTab"
      :index="2"
      title="Danh sách sản phẩm trong kho"
      description="Danh sách sản phẩm"
    >
      <template v-slot:tab-title>
        <b-row align-h="start" class="mt-10 mb-10 font-weight-bold text-dark">
          <b-col>
            <h2>Danh sách sản phẩm</h2>
          </b-col>
          <b-col>
            <div
              v-if="form_type !== 'detail'"
              class="d-flex justify-content-end"
            >
              <a
                @click="warehouseAction('create')"
                class="btn btn-primary font-weight-bolder"
                >Thêm sản phẩm</a
              >
            </div>
          </b-col>
        </b-row>
      </template>
      <b-row>
        <b-col>
          <template-table
            :column="columnWarehouse"
            :paging="productpaging"
            :data="productList"
            :searchAction="false"
            :tableAction="tableAction"
          >
            <template v-slot:body="{}">
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
            </template>
            <!-- <template v-slot:action="{ item }">
              <action
                :value="item"
                @view="viewWarehouse"
                @edit="warehouseAction('edit', item.id)"
                @delete="warehouseAction('inactive', item)"
              ></action>
						</template>-->
          </template-table>
        </b-col>
      </b-row>
    </basic-tab>
    <warehouse-popup
      :popupType="popupType"
      :id="productId"
      :warehouseId="id ? id : form.id"
    ></warehouse-popup>
  </basic-form-tab>
</template>
<script>
import { mapGetters } from 'vuex';
import KTUtil from '@/assets/js/components/util';
import KTWizard from '@/assets/js/components/wizard';
import warehousePopup from './productPopup';

export default {
  components: {
    warehousePopup,
  },
  props: {
    form_type: {
      type: String,
      default: 'detail',
    },
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      productpaging: {
        page: 1,
        pageSize: 10,
        total: 0,
      },
      productList: [],
      productId: null,
      popupType: null,
      form: {
        code: null,
        name: null,
        otherName: null,
        note: null,
        isDeleted: null,
      },
      tabs: [
        {
          key: 'general',
          title: 'Thông tin kho',
          description: 'Thông tin chung về kho',
        },
        {
          key: 'general',
          title: 'Danh sách sản phẩm',
          decreiption: `Danh sách sản phẩm của kho ${this.name} `,
        },
      ],
      columnWarehouse: [
        {
          key: 'code',
          label: 'Mã Hàng',
          sortable: true,
        },
        {
          key: 'name',
          label: 'Tên Hàng',
          sortable: true,
        },
        {
          key: 'Serial',
          label: 'Serial',
          sortable: true,
        },
        {
          key: 'Nhập kho',
          label: 'Nhập kho',
          sortable: true,
        },
        {
          key: 'Tồn Kho',
          label: 'Tồn Kho',
          sortable: true,
        },
        {
          key: 'Xuất kho',
          label: 'Xuất Kho',
          sortable: true,
        },
        {
          key: 'Nhà Cung Cấp',
          label: 'Nhà Cung Cấp',
          sortable: true,
        },
      ],
      data: [],

      validationState: {
        code: null,
        name: null,
        otherName: null,
        note: null,
      },
      error: {
        code: null,
        name: null,
        otherName: null,
        note: null,
      },
    };
  },
  computed: {
    tableAction() {
      if (this.form_type === 'detail') {
        return false;
      }
      return true;
    },
    disabledTab() {
      if (this.form_type === 'create') {
        return this.form.id === null;
      } else {
        return this.id === null;
      }
    },
    ...mapGetters('context', ['branch']),
    disabled() {
      if (this.form_type === 'detail') {
        return true;
      }
      return false;
    },
    warehouseData: {
      get() {
        let newWarehouse = {
          code: this.form.code,
          name: this.form.name,
          otherName: this.form.otherName,
          note: this.form.note,
          branchId: this.branch.id,
          isDeleted: this.form.isDeleted,
        };
        return newWarehouse;
      },
      set(data) {
        this.form.code = data.code;
        this.form.name = data.name;
        this.form.otherName = data.otherName;
        this.form.note = data.note;
        this.form.isDeleted = data.isDeleted;
      },
    },
    searchProductParams() {
      return {
        page: this.productpaging.page,
        size: this.productpaging.pageSize,
      };
    },
  },
  watch: {
    'productpageing.page'() {
      this.loadWarehouseProduct();
    },
    'productpageing.pageSize'() {
      this.loadWarehouseProduct();
    },
  },
  methods: {
    async warehouseAction(actionType, productId) {
      // if (actionType === "create" || actionType === "edit") {
      //   if (actionType === "create" && !this.id && !this.form.id) {
      //     let isFormValid = this.isFormValid();
      //     if (!isFormValid) {
      //       this.$refs.formTab.showTab(0);
      //       return;
      //     }
      //     try {
      //       await this.create(false);
      //     } catch {
      //       this.$refs.formTab.showTab(0);
      //       return;
      //     }
      //   }
      this.popupType = actionType;
      this.productId = productId;
      this.$nextTick(() => {
        this.$bvModal.show('product-modal');
      });
    },
    codeValidation() {
      let codeState = null;
      let codeValidation = null;
      if (!this.form.code || !this.form.code.trim().length) {
        codeState = false;
        codeValidation = 'Không được bỏ trống Mã kho';
      } else {
        codeState = true;
        codeValidation = null;
      }
      this.validationState.code = codeState;
      this.error.code = codeValidation;
    },
    nameValidation() {
      let nameState = null;
      let nameValidation = null;
      if (!this.form.name || !this.form.name.trim().length) {
        nameState = false;
        nameValidation = 'Không được bỏ trống Tên kho';
      } else {
        nameState = true;
        nameValidation = null;
      }
      this.validationState.name = nameState;
      this.error.name = nameValidation;
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
    edit() {
      this.$store.commit('context/setLoading', true);
      this.$api
        .put(`warehouse/${this.id}`, this.warehouseData)
        .then(() => {
          this.$toastr.s({
            title: 'Thành công!',
            msg: 'Cập nhật kho thành công',
          });
        })
        .finally(() => {
          this.$store.commit('context/setLoading', false);
        });
    },
    submit() {
      let isFormValid = this.isFormValid();
      if (isFormValid) {
        // if (this.form_type === "create") {
        // 	this.create();
        // } else
        if (this.form_type === 'edit') {
          this.edit();
        }
      }
      setTimeout(() => {
        window.scrollTo(0, 0);
      }, 200);
      return;
    },
    async loadWarehouseData() {
      await this.$api.get(`warehouse/${this.id}`).then(({ data }) => {
        this.warehouseData = data;
      });
    },
    async loadWarehouseProduct() {
      let warehouseId = this.id ? this.id : this.form.id ? this.form.id : null;
      if (warehouseId) {
        await this.$api
          .get(`product/${this.id}`, this.searchProductParams)
          .then(({ meta, data }) => {
            this.productList = data;
            this.productpaging.total = meta.total;
            this.data = data;
          })
          .finally(() => {
            this.$store.commit('context/setLoading', false);
          });
      }
    },
  },
  async mounted() {
    this.$store.commit('context/setLoading', true);
    if (this.id) {
      await this.loadWarehouseData();
      await this.loadWarehouseProduct();
    }
    this.$store.commit('context/setLoading', false);

    const wizard = new KTWizard('kt_wizard_v4', {
      startStep: 1, // initial active step number
      clickableSteps: true, // allow step clicking
    });

    // Validation before going to next page
    // wizard.on("beforeNext", function(/*wizardObj*/) {
    //   // validate the form and use below function to stop the wizard's step
    //   // wizardObj.stop();
    // });

    // Change event
    wizard.on('change', function (/*wizardObj*/) {
      setTimeout(() => {
        KTUtil.scrollTop();
      }, 500);
    });
  },
};
</script>
