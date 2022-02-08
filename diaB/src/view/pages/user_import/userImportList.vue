
<template>

  <div class="user-import-list-page w-100">
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
                    label="Tên nhóm"
                    :required="true"
                    placeholder="Nhập tên nhóm"
                    name="name"
                    :value.sync="filter.searchKey"
                  ></basic-input>
                </b-col>
              </b-row>
              <b-row>
                <b-col>
                  <basic-select
                    label="Chức năng"
                    placeholder="Chọn nhóm chức năng"
                    name="exerciseCategory"
                    apiPath="/exerciseCategory"
                    :value.sync="filter.searchKey"
                  />
                </b-col>
              </b-row>
              <b-row>
                <b-col>
                  <basic-check-box
                    :options="[{ text: 'Lọc các nhóm Inactive', value: false }]"
                  ></basic-check-box>
                </b-col>
              </b-row>
            </b-container>
          </b-dropdown-form>
          <b-dropdown-divider> </b-dropdown-divider>
          <div class="d-flex align-items-center justify-content-lg-end m-0 p-4">
            <b-button class="btn ml-2" href="#" tabindex="0">
              <span class="svg-icon">
                <inline-svg
                  src="/media/svg/icons/Neolex/Basic/refresh-cw.svg"
                />
              </span>
              Reset bộ lọc
            </b-button>
            <b-button class="btn ml-2" href="#" tabindex="0">
              <span class="svg-icon">
                <inline-svg src="/media/svg/icons/Neolex/Basic/filter.svg" />
              </span>
              Lọc dữ liệu
            </b-button>
          </div>
        </b-dropdown>


  <b-button  class="btn btn-success ml-2"
          type="button" id="show-btn" @click="$bvModal.show('bv-modal-example')">Import</b-button>
         

  <b-modal id="bv-modal-example" hide-footer>
    <label for="name-input" class="d-block" id="__BVID__797__BV_label_">Import file</label>
    <label id="file-name" type="text" required="required" aria-required="true" class="form-control" @change="onFileChange" ></label>

    <!-- <label id="file-name"> -->
 <template #modal-title>
     Thêm File
    </template>    
     <div class="modal-footer">
        <button type="button" class="btn btn-secondary" block @click="$bvModal.hide('bv-modal-example')" >Hủy</button>
        <!-- <b-button type="button" class="btn btn btn-success ml-2 btn-secondary" >Import <inline-svg src="/media/svg/icons/Neolex/Basic/upload-cloud.svg" /></b-button> -->
       <button class="btn btn btn-success ml-2 btn-secondary" onclick="document.getElementById('getFile').click()">Import</button>
       <input type='file' id="getFile" style="display:none" >
       
   

  

      </div>
  </b-modal>

  <!-- <input id="file-upload" name='upload_cont_img' type="file" style="display:none;"> -->

  


      </template>
      
     
    </basic-subheader>
    <b-container fluid class="user-import-list-page__body mb-6 mt-6">
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
                @sortBy="sortRequest"
              >
                <template v-slot:body="{ item }">
                  <td>
                    {{ item.name }}
                  </td>
                  <td style="width: 20px">
                    <action-dropdown
                      :value="item"
                      @view="viewItem"
                      @edit="editItem"
                      @delete="deleteItem"
                      @copy="copyItem"
                    >
                    </action-dropdown>
                  </td>
                  <td>{{ item.note }}</td>
                  <td>
                    <div>
                      <avatar
                        v-for="member in item.members"
                        :key="member.id"
                        styleClass="mr-2"
                        :size="'40px'"
                        :text="member.fullName"
                        :src="member.picture"
                        :rounded="true"
                      ></avatar>
                    </div>
                  </td>
                  <td>{{ $moment(item.lastUpdated).format('DD/MM/YYYY') }}</td>
                  <td>
                    <div class="d-flex align-items-center">
                      <avatar
                        size="40px"
                        :text="item.updatedPerson.fullName"
                        :src="item.updatedPerson.picture"
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
                          {{ item.updatedPerson.fullName }}
                        </p>
                        <p
                          class="mt-2 mb-0"
                          style="font-size: 12px; color: #888c9f"
                        >
                          {{ item.updatedPerson.username }}
                        </p>
                      </div>
                    </div>
                  </td>
                </template>
              </template-table>
            </div>
          </div>
        </b-col>
      </b-row>
    </b-container>
  </div>
</template>




<style lang="scss" scoped>
.dropdown-form-filter {
  .dropdown-menu {
    .container {
      width: 430px;
    }
  }
}




</style>

<script>
export default {
  
  data() {
    return {
      paging: {
        page: 1,
        pageSize: 10,
        total: 0,
      },
      filter: {
        searchKey: null,
      },
      sort: {
        by: null,
        order: null,
      },
      column: [
        {
          key: 'name',
          label: 'Bệnh nhân',
          sortable: false,
        },
        {
          key: 'actionDropdown',
          label: '',
          sortable: false,
        },
        {
          key: 'functionType',
          label: 'Loại bệnh',
          sortable: false,
        },
        {
          key: 'members',
          label: 'Ngày cập nhập',
          sortable: false,
        },
        {
          key: 'lastUpdated',
          label: 'Gói tham gia',
          sortable: false,
        },
        {
          key: 'updatedPerson',
          label: 'Loại khảo sát',
          sortable: false,
        },
         {
          key: 'updatedPerson2',
          label: 'Tên khảo sát',
          sortable: false,
        },
      ],
      data: [],
    };
  },
  computed: {
    searchParams() {
      return {
        searchKey: this.filter.searchKey,
        page: this.paging.page,
        size: this.paging.pageSize,
      };
    },
  },
  watch: {
    'paging.page'() {
      this.loadData();
    },
    'paging.pageSize'() {
      this.loadData();
    },
    sort: {},
  },
  methods: {
    pagingAction() {
      this.loadData();
    },
    createItem() {
      this.$router.push({
        name: 'windown_import',
        params: {
          form_type: 'create',
        },
      });
    },
    editItem(item) {
      this.$router.push({
        name: 'user_group_detail',
        params: {
          form_type: 'edit',
          id: item.id,
        },
      });
    },
    // viewItem(item) {
    // },
    // editItem(item) {
    // },
    // copyItem(item) {
    // },
    // deleteItem(item) {
    // },
    searchRequest() {
      this.loadData();
    },
    resetRequest() {
      this.filter.searchKey = null;
      this.$nextTick(() => {
        this.$validator.reset();
      });
      this.loadData();
    },
    sortRequest() {
      return;
    },
    loadData() {
      this.$store.commit('context/setLoading', true);
      this.$api
        .get('user_groups', {
          params: this.searchParams,
        })
        .then(({ meta, data }) => {
          this.data = data || [];
          this.paging.total = meta.total;
        })
        .finally(() => {
          this.$store.commit('context/setLoading', false);
        });
      return;
    },
  },
  mounted() {
    // this.loadData();
    
  },
 
};


</script>


