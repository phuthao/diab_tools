<template>
  <form ref="form" @submit.stop.prevent="importForm">
    <b-row>
      <b-col style="min-height: 100px" class="mt-4">
        <div>
          <v-file-input
            v-model="file"
            accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            @change="onUpload"
            truncate-length="15"
            label="File input"
          >
          </v-file-input>
        </div>
      </b-col>
    </b-row>
  </form>
</template>

<script>
export default {
  name: 'UserImportForm',
  props: {
    data: {
      type: Object,
      default: () => ({}),
    },
  },
  watch: {},
  computed: {},
  data() {
    return {
      file: null,
      form: {
        file: null,
      },
      rules: [],
    };
  },

  methods: {
    onUpload() {
      if (this.file) {
        this.$emit('onUpload', true);
      } else {
        this.$emit('onUpload', false);
      }
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
    importForm() {
      this.$validator.validateAll().then((result) => {
        if (!result) {
          return;
        }
        let formData = new FormData();
        this.file && formData.append('file', this.file);
        this.$emit('importForm', formData);
      });
    },
    onChange() {},
  },
};
</script>
