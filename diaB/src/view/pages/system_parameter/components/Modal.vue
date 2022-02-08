<template>
  <b-modal id="system-modal" ref="system-modal" title="Cập nhật tham số">
    <b-container fluid class="p-0">
      <form ref="form" @submit.stop.prevent="handleSubmit">
        <b-row>
          <b-col>
            <basic-input
              label="Mã tham số"
              placeholder="Mã tham số"
              name="code"
              :required="true"
              disabled
              :value.sync="form.key"
            ></basic-input>
          </b-col>
        </b-row>

        <b-row>
          <b-col>
            <basic-input
              label="Tên tham số"
              placeholder="Tên tham số"
              name="name"
              :required="true"
              :value.sync="form.name"
              data-vv-as="Tên tham số"
              v-validate="'required'"
              :state="validateState('name')"
              :invalidFeedback="errors.first('name')"
            ></basic-input>
          </b-col>
        </b-row>

        <b-row>
          <b-col>
            <basic-input
              label="Giá trị "
              name="value"
              :required="true"
              :value.sync="form.value"
              data-vv-as="Giá trị  tham số"
              v-validate="'required'"
              :state="validateState('value')"
              :invalidFeedback="errors.first('value')"
            ></basic-input>
          </b-col>
        </b-row>

        <b-row>
          <b-col>
            <basic-text-area
              label="Ghi chú"
              placeholder="Ghi chú"
              name="note"
              :value.sync="form.description"
            ></basic-text-area>
          </b-col>
        </b-row>
      </form>
    </b-container>
    <template #modal-footer>
      <div class="w-100 d-flex align-items-center justify-content-end">
        <b-button
          class="btn btn-success ml-2"
          href="#"
          @click.stop="handelValidation"
          tabindex="0"
        >
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/save.svg" />
          </span>
          Cập nhật
        </b-button>
      </div>
    </template>
  </b-modal>
</template>

<script>
import { SET_PARAMS } from '@/core/services/store/setting.module';
export default {
  name: 'Modal',
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
    detail: {
      type: Object,
      default: () => {},
    },
  },
  data() {
    return {
      validationState: {},
      error: {},
    };
  },
  computed: {
    form() {
      return this.detail;
    },
  },
  methods: {
    validateState(ref) {
      if (
        this.veeFields[ref] &&
        (this.veeFields[ref].dirty || this.veeFields[ref].validated)
      ) {
        return !this.errors.has(ref);
      }
      return null;
    },
    handelValidation() {
      this.$validator.validateAll().then((result) => {
        if (!result) {
          return;
        }
        this.handleSubmit();
      });
    },
    async handleSubmit() {
      try {
        await this.$api.put('CommonConfigureTask', this.form);
        this.$bvModal.hide('system-modal');
        this.$emit('loadData');
        this.$toastr.s({
          title: 'Thành công !',
          msg: 'Cập nhật thành công',
        });
        this.$store.dispatch(SET_PARAMS);
      } catch (error) {
        this.$toastr.e({
          title: 'Lỗi !',
          msg: error,
        });
      }
    },
  },
};
</script>

<style lang="scss">
#glucose-modal {
  .modal-dialog {
    width: 370px;
    height: 582px;
  }
}
</style>
