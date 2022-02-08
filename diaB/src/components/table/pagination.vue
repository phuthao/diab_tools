<template>
  <div class="row" v-show="totalPage > 0">
    <div class="col-sm-12 col-md-7 col-xl-7 dataTables_pager">
      <div class="dataTables_length" style="min-width: 150px">
        <!-- <div>Hiển thị</div> -->
        <label>
          <select
            name="kt_datatable_length"
            aria-controls="kt_datatable"
            class="custom-select custom-select-sm ml-3 mr-3"
            v-model="paging.pageSize"
          >
            <option value="5">5</option>
            <option value="10">10</option>
            <option value="25">25</option>
            <option value="50">50</option>
          </select>
        </label>
        <div>kết quả trên trang</div>
        <!-- <div class="ml-3">
          Hiển thị {{ paging.total > 0 ? startIdx + 1 : 0 }} - {{ endIdx }} /
          {{ paging.total }} dữ liệu
        </div> -->
      </div>
      <!-- <div class="dataTables_paginate paging_simple_numbers">
        <b-pagination
          v-model="paging.page"
          :total-rows="paging.total"
          :per-page="paging.pageSize"
          first-number
          last-number
          prev-class="paginate_button page-item previous"
          next-class="paginate_button page-item next"
        >
          <template v-slot:prev-text>
            <i class="ki ki-arrow-back"></i>
          </template>
          <template v-slot:next-text>
            <i class="ki ki-arrow-next"></i>
          </template>
        </b-pagination>
      </div> -->
    </div>
    <div class="col-sm-12 col-md-5 col-xl-5 dataTables_info">
      <!-- <div role="status" aria-live="polite" v-if="paging.total">
        Hiển thị {{ startIdx + 1 }} - {{ endIdx }} trong số
        {{ paging.total }} dòng
      </div> -->
      <div class="dataTables_paginate paging_simple_numbers">
        <b-pagination
          v-model="paging.page"
          :total-rows="paging.total"
          :per-page="paging.pageSize"
          first-number
          last-number
          prev-class="paginate_button page-item previous"
          next-class="paginate_button page-item next"
        >
          <template v-slot:prev-text>
            <i class="ki ki-arrow-back"></i>
          </template>
          <template v-slot:next-text>
            <i class="ki ki-arrow-next"></i>
          </template>
        </b-pagination>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    paging: {
      type: Object,
    },
  },
  computed: {
    totalPage() {
      return Math.floor(this.paging.total / this.paging.pageSize);
    },
    startIdx() {
      return (this.paging.page - 1) * this.paging.pageSize;
    },
    endIdx() {
      const tempEndIdx = this.startIdx * 1 + this.paging.pageSize * 1;
      return this.paging.total >= tempEndIdx ? tempEndIdx : this.paging.total;
    },
  },
};
</script>
