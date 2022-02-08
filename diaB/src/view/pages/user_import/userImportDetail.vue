<template>
  <div class="user-import-detail-page w-100">
    <basic-subheader
      :title="
        form_type === 'create'
          ? 'Thêm nhóm người dùng mới'
          : 'Chỉnh sửa nhóm người dùng'
      "
    >
      <template v-slot:actions>
        <b-button class="btn ml-3" type="button" pill @click="returnPage">
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Arrows/arrow-left.svg" />
          </span>
          Huỷ
        </b-button>
        <b-button class="btn ml-3" type="button">
          <span class="svg-icon">
            <inline-svg src="/media/svg/icons/Neolex/Basic/refresh-cw.svg" />
          </span>
          Reset dữ liệu
        </b-button>
        <template v-if="form_type === 'create'">
          <b-button class="btn btn-success ml-3" type="button">
            <span class="svg-icon">
              <inline-svg src="/media/svg/icons/Neolex/Basic/plus.svg" />
            </span>
            Tạo mới
          </b-button>
        </template>
        <template v-else>
          <b-button class="btn btn-danger ml-3" type="button">
            <span class="svg-icon">
              <inline-svg src="/media/svg/icons/Neolex/Basic/trash-2.svg" />
            </span>
            Xóa nhóm
          </b-button>
          <b-button class="btn btn-success ml-3" type="button">
            <span class="svg-icon">
              <inline-svg src="/media/svg/icons/Neolex/Basic/check.svg" />
            </span>
            Cập nhật
          </b-button>
        </template>
      </template>
    </basic-subheader>
    <b-container fluid class="user-import-detail-page__body mb-6 mt-6">
      <div class="card card-custom">
        <b-row class="border-bottom">
          <b-col class="col-6 col-lg-4 col-xl-3">
            <h6 class="d-flex align-items-center p-6 mb-0 text-success">
              <span class="svg-icon mr-3">
                <inline-svg src="/media/svg/icons/Neolex/Basic/info.svg" />
              </span>
              Thông tin nhóm
            </h6>
          </b-col>
          <b-col class="col-7 col-lg-8 col-xl-9">
            <h6 class="d-flex align-items-center p-6 mb-0 text-success">
              <span class="svg-icon mr-3">
                <inline-svg
                  src="/media/svg/icons/Neolex/Basic/check-square.svg"
                />
              </span>
              Chức năng thuộc nhóm
            </h6>
          </b-col>
        </b-row>
        <b-row>
          <b-col class="col-5 col-lg-4 col-xl-3">
            <div class="p-6">
              <label>Tên nhóm:</label>
              <b-input
                type="text"
                class="form-control datatable-input"
                placeholder="Nhập tên nhóm"
                data-col-index="0"
                v-model="form.name"
              />
            </div>
          </b-col>
          <b-col class="col-7 col-lg-8 col-xl-9">
            <div class="p-6">
              <b-checkbox>
                Chọn toàn bộ các quyền trong tất cả các chức năng
              </b-checkbox>
              <div class="mt-2">
                <template-table
                  :column="column"
                  :data="data"
                  :tableAction="false"
                  :selectAction="false"
                  :searchAction="false"
                  :pagingAction="false"
                >
                  <template v-slot:body="{ item }">
                    <td style="width: 50px; text-align: center">
                      <b-checkbox size="lg"> </b-checkbox>
                    </td>
                    <td>
                      {{ item.name }}
                    </td>
                    <td style="width: 100px; text-align: center">
                      <b-checkbox size="lg"> </b-checkbox>
                    </td>
                    <td style="width: 100px; text-align: center">
                      <b-checkbox size="lg"> </b-checkbox>
                    </td>
                    <td style="width: 100px; text-align: center">
                      <b-checkbox size="lg"> </b-checkbox>
                    </td>
                    <td style="width: 100px; text-align: center">
                      <b-checkbox size="lg"> </b-checkbox>
                    </td>
                  </template>
                </template-table>
              </div>
            </div>
          </b-col>
        </b-row>
      </div>
    </b-container>
  </div>
</template>

<script>
export default {
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
      form: {
        name: null,
        functionTypes: [],
      },
      column: [
        {
          key: 'actionCheckbox',
          label: '',
          sortable: false,
        },
        {
          key: 'name',
          label: 'Chức Năng',
          sortable: false,
        },
        {
          key: 'hasViewFunction',
          label: 'Xem',
          sortable: false,
          style: 'width: 100px; text-align: center',
        },
        {
          key: 'hasAddFunction',
          label: 'Thêm mới',
          sortable: false,
          style: 'width: 100px; text-align: center',
        },
        {
          key: 'hasEditFunction',
          label: 'Chỉnh Sửa',
          sortable: false,
          style: 'width: 100px; text-align: center',
        },
        {
          key: 'hasDeleteFunction',
          label: 'Xoá',
          sortable: false,
          style: 'width: 100px; text-align: center',
        },
      ],
      data: [
        {
          id: 'c8f334f8-6002-4560-adf7-3f70f39a1030',
          name: 'Nhóm người dùng',
        },
        {
          id: '601a9808-4706-499d-9f94-26afedf1dc72',
          name: 'Tài khoản portal',
        },
        {
          id: 'e9c2ac7f-2db3-429d-b0fe-98ee1f9c6b97',
          name: 'Tài khoản bệnh nhân',
        },
        {
          id: '74faa208-4398-406f-8da8-5112f09ef451',
          name: 'Khoá học',
        },
        {
          id: '8e4022eb-5fb1-415c-9ce2-7f2176ffe33a',
          name: 'Huấn luyện viên & chuyên gia',
        },
      ],
    };
  },
  computed: {},
  watch: {},
  methods: {
    returnPage() {
      this.$router.push({
        name: 'user_group_list',
      });
    },
    loadData() {
      this.$store.commit('context/setLoading', true);
      this.$api
        .get('user_groups', {
          params: this.searchParams,
        })
        // eslint-disable-next-line no-unused-vars
        .then(({ meta, data }) => {
          this.data = data || [];
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
