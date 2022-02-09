<template>
  <b-modal
    id="user-import-modal"
    ref="user-import-modal"
    :title="'Import file'"
    ok-title="Lưu"
    cancel-title="Hủy"
  >
    <b-overlay :show="loading">
      <b-container fluid class="p-0">
        <user-import-form
          ref="form"
          @onUpload="onUpload"
          @importForm="importForm"
        />
      </b-container>
    </b-overlay>
    <template #modal-footer>
      <div class="w-100 d-flex align-items-center justify-content-end">
        <!--<b-button class="btn btn-secondary" href="#" tabindex="0">
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/power.svg" />
          </span>
          Hủy
        </b-button> -->
        <b-button
          class="btn btn-success ml-2"
          href="#"
          :disabled="!isSelectFile"
          @click="handleClick"
          tabindex="0"
        >
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/upload-cloud.svg" />
          </span>
          Import
        </b-button>
      </div>
    </template>
  </b-modal>
</template>

<script>
import UserImportForm from './Form';
export default {
  name: 'Modal',
  components: { 'user-import-form': UserImportForm },
  props: {
    popupType: {
      type: String,
      default: 'detail',
    },
  },
  computed: {},
  watch: {},
  mounted() {},
  data() {
    return {
      isMounted: false,
      loading: false,
      isSelectFile: false,
      form: {},
      validationState: {},
      error: {},
    };
  },
  methods: {
    handleClick() {
      this.$refs.form.importForm();
    },
    onUpload(payload) {
      this.isSelectFile = payload;
    },
    importForm(payload) {
      this.handleSubmit(payload);
    },
    async handleSubmit(payload) {
      this.loading = true;
      let result = [];
      try {
        if (true) {
          await this.$api
            .post('Admin/Import/staff', payload, {
              headers: {
                'Content-Type': 'multipart/form-data',
              },
            })
            .then(({ meta, data }) => {
              result = data.staffList;
            })
            .finally(() => {
              //this.$store.commit('context/setLoading', false);
            });
        }
        this.$toastr.s({
          title: 'Thành công!',
          msg: `Import thành công`,
        });
        this.$emit('loadData', result);
        this.$bvModal.hide('user-import-modal');
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

<style lang="scss">
#glucose-modal {
  .modal-dialog {
    width: 370px;
    height: 582px;
  }
}
</style>
