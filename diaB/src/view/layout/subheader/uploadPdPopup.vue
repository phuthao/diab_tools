<template>
  <b-modal
    id="upload-modal"
    ref="upload-modal"
    title="Tải phiếu giao hàng"
    ok-title="Tải lên"
    cancel-title="Hủy"
    size="lg"
    @show="show"
    @ok="handleOk"
  >
    <b-form>
      <div
        :class="{
          'is-dragging-over': isDraggingOver || filelist.length,
        }"
        class="flex items-center justify-center w-full h-screen text-center"
        id="drag-zone"
      >
        <input
          type="file"
          multiple
          name="fields[assetsFieldHandle][]"
          id="assetsFieldHandle"
          class="w-px h-px opacity-0 overflow-hidden absolute"
          @change="onChange"
          ref="file"
          accept=".xls,.xlsx"
        />

        <label for="assetsFieldHandle" class="block cursor-pointer">
          <div
            class="px-12 py-5"
            id="file-upload"
            @dragover="dragover"
            @dragleave="dragleave"
            @drop="drop"
          >
            <span class="svg-icon svg-icon-primary svg-icon-5x"
              ><!--begin::Svg Icon | path:C:\wamp64\www\keenthemes\themes\metronic\theme\html\demo1\dist/../src/media/svg/icons\Files\Upload.svg--><svg
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                width="24px"
                height="24px"
                viewBox="0 0 24 24"
                version="1.1"
              >
                <g
                  stroke="none"
                  stroke-width="1"
                  fill="none"
                  fill-rule="evenodd"
                >
                  <rect x="0" y="0" width="24" height="24" />
                  <path
                    d="M2,13 C2,12.5 2.5,12 3,12 C3.5,12 4,12.5 4,13 C4,13.3333333 4,15 4,18 C4,19.1045695 4.8954305,20 6,20 L18,20 C19.1045695,20
        20,19.1045695 20,18 L20,13 C20,12.4477153 20.4477153,12 21,12 C21.5522847,12 22,12.4477153 22,13 L22,18 C22,20.209139 20.209139,22
        18,22 L6,22 C3.790861,22 2,20.209139 2,18 C2,15 2,13.3333333 2,13 Z"
                    fill="#000000"
                    fill-rule="nonzero"
                    opacity="0.3"
                  />
                  <rect
                    fill="#000000"
                    opacity="0.3"
                    x="11"
                    y="2"
                    width="2"
                    height="14"
                    rx="1"
                  />
                  <path
                    d="M12.0362375,3.37797611 L7.70710678,7.70710678 C7.31658249,8.09763107 6.68341751,8.09763107 6.29289322,7.70710678
        C5.90236893,7.31658249 5.90236893,6.68341751 6.29289322,6.29289322 L11.2928932,1.29289322 C11.6689749,0.916811528 12.2736364,0.900910387
        12.6689647,1.25670585 L17.6689647,5.75670585 C18.0794748,6.12616487 18.1127532,6.75845471 17.7432941,7.16896473 C17.3738351,7.57947475
        16.7415453,7.61275317 16.3310353,7.24329415 L12.0362375,3.37797611 Z"
                    fill="#000000"
                    fill-rule="nonzero"
                  />
                </g></svg
              ><!--end::Svg Icon--></span
            >
            <h3 class="pt-5 px-20">
              Nhấn vào hoặc kéo file excel vào đây để tải lên phiếu giao hàng
              mới.
            </h3>
          </div>
        </label>
      </div>
      <div
        v-if="filelist.length"
        class="flex items-center justify-center w-full h-screen text-center"
      >
        <span
          class="label label-lg font-weight-bolder label-light-success label-inline"
        >
          <span class="svg-icon svg-icon-primary svg-icon-2x"
            ><!--begin::Svg Icon | path:C:\wamp64\www\keenthemes\themes\metronic\theme\html\demo1\dist/../src/media/svg/icons\General\Clip.svg--><svg
              xmlns="http://www.w3.org/2000/svg"
              xmlns:xlink="http://www.w3.org/1999/xlink"
              width="24px"
              height="24px"
              viewBox="0 0 24 24"
              version="1.1"
            >
              <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                <rect x="0" y="0" width="24" height="24" />
                <path
                  d="M14,16 L12,16 L12,12.5 C12,11.6715729 11.3284271,11 10.5,11 C9.67157288,11 9,11.6715729 9,12.5 L9,17.5 C9,19.4329966
        10.5670034,21 12.5,21 C14.4329966,21 16,19.4329966 16,17.5 L16,7.5 C16,5.56700338 14.4329966,4 12.5,4 L12,4 C10.3431458,4 9,5.34314575
        9,7 L7,7 C7,4.23857625 9.23857625,2 12,2 L12.5,2 C15.5375661,2 18,4.46243388 18,7.5 L18,17.5 C18,20.5375661 15.5375661,23 12.5,23
        C9.46243388,23 7,20.5375661 7,17.5 L7,12.5 C7,10.5670034 8.56700338,9 10.5,9 C12.4329966,9 14,10.5670034 14,12.5 L14,16 Z"
                  fill="#000000"
                  fill-rule="nonzero"
                  transform="translate(12.500000, 12.500000) rotate(-315.000000) translate(-12.500000, -12.500000) "
                />
              </g></svg
            ><!--end::Svg Icon--></span
          >
          <span class="pl-5">
            {{ filelist[0].name }}
          </span>
        </span>
      </div>
    </b-form>
  </b-modal>
</template>

<script>
export default {
  props: {},
  components: {},
  data() {
    return {
      filelist: [],
      isDraggingOver: false,
    };
  },
  computed: {},
  watch: {},
  methods: {
    show() {
      this.filelist = [];
    },
    handleOk(bvModalEvt) {
      bvModalEvt.preventDefault();
      this.handleSubmit();
    },
    handleSubmit() {
      var file = this.filelist[0];

      let formData = new FormData();
      formData.append('file', file);

      this.$api
        .post('purchasedDelivery/import', formData, {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        })
        .then(() => {
          this.$bvModal.hide('upload-modal');
        })
        .catch((error) => {
          let errorMessage = error.message;
          if (
            error.code === 'not_found' &&
            error.data.model === 'PurchasedDelivery'
          ) {
            errorMessage = 'Không tìm thấy';
            if (error.data.purchasedOrderCode) {
              errorMessage += ` đơn hàng: ${error.data.purchasedOrderCode}`;
            } else if (error.data.branchCode) {
              errorMessage += ` chi nhánh: ${error.data.purchasedOrderCode}`;
            } else if (error.data.brandCode) {
              errorMessage += ` thương hiệu: ${error.data.purchasedOrderCode}`;
            } else if (error.data.productCode) {
              errorMessage += ` sản phẩm: ${error.data.purchasedOrderCode}`;
            }
          }
          this.$toastr.e({
            title: 'Đã xảy ra lỗi!',
            msg: errorMessage,
          });
        });
    },
    changeHandler(evt) {
      evt.stopPropagation();
      evt.preventDefault(); // FileList object.
      var files = evt.target.files;
      var file = files[0];

      let formData = new FormData();
      formData.append('file', file);

      this.$api.post('purchasedDelivery/import', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
    },
    onChange() {
      this.filelist = [...this.$refs.file.files];
    },
    remove(i) {
      this.filelist.splice(i, 1);
    },
    dragover(event) {
      event.preventDefault();
      // Add some visual fluff to show the user can drop its files
      this.isDraggingOver = true;
    },
    dragleave() {
      this.isDraggingOver = false;
    },
    drop(event) {
      event.preventDefault();
      this.$refs.file.files = event.dataTransfer.files;
      this.onChange(); // Trigger the onChange event manually
    },
  },
  mounted() {},
};
</script>

<style lang="scss">
#drag-zone {
  border: 3px dotted gray;
  border-radius: 5px;
  padding: 5px;
  margin: 10px;
}

.is-dragging-over {
  background: aliceblue;
}
#drag-zone > input[type='file'] {
  display: none;
}
</style>
