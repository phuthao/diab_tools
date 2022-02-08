<template>
  <b-form-group
    :id="`${name}-group`"
    :description="description"
    :label="label"
    :label-for="`${name}-ID`"
    :invalid-feedback="invalidFeedback"
    :valid-feedback="validFeedback"
    :state="state"
    :class="`${required ? 'required' : ''}`"
  >
    <div class="text-editors">
      <quill-editor
        class="editor"
        ref="textEditors"
        :value="value"
        :options="editorOption"
        :disabled="disabled"
        @change="onEditorChange"
        @blur="onEditorBlur($event)"
        @focus="onEditorFocus($event)"
        @ready="onEditorReady($event)"
      />
      <!-- Display Value Text Editors
      <div class="output code">
        <code class="hljs" v-html="contentCode"></code>
      </div>
      <div class="output ql-snow">
        <div class="ql-editor" v-html="value"></div>
      </div> -->
    </div>
  </b-form-group>
</template>

<script>
// import dedent from 'dedent';
import hljs from 'highlight.js';
import debounce from 'lodash/debounce';
import { quillEditor } from 'vue-quill-editor';

// highlight.js style
import 'highlight.js/styles/tomorrow.css';

// import theme style
import 'quill/dist/quill.core.css';
import 'quill/dist/quill.snow.css';

export default {
  components: {
    quillEditor,
  },
  props: {
    required: {
      type: Boolean,
      default: false,
    },
    value: {
      type: String,
      default: '',
    },
    name: {
      type: String,
      default: '',
    },
    label: {
      type: String,
      default: null,
    },
    type: {
      type: String,
      default: 'text',
    },
    placeholder: {
      type: String,
      default: null,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    state: {
      type: Boolean,
      default: null,
    },
    invalidFeedback: {
      type: String,
      default: null,
    },
    validFeedback: {
      type: String,
      default: null,
    },
    description: {
      type: String,
      default: null,
    },
    toolbarOptions: {
      type: Array,
      default: () => [],
    },
  },
  computed: {
    test() {
      return this.toolbarOptions;
    },
    editor() {
      return this.$refs.textEditors.quill;
    },
    contentCode() {
      return hljs.highlightAuto(this.value).value;
    },
  },
  mounted() {
    let customButton = document.querySelector('.ql-omega');
    let image = document.createElement('img');
    image.src = '/media/svg/icons/Code/Code.svg';
    customButton.appendChild(image);
  },
  data() {
    return {
      editorOption: {
        placeholder: this.placeholder,
        modules: {
          toolbar: {
            container: [
              [{ size: ['small', false, 'large', 'huge'] }],
              ['bold', 'italic', 'underline'],
              [{ list: 'ordered' }, { list: 'bullet' }],
              ['link', 'image', 'video'],
              [{ color: [] }, { background: [] }], // dropdown with defaults from theme
              [{ align: [] }],
              ['clean'],
              ['omega'],
            ],
            handlers: {
              omega: this.imageHandler,
            },
          },

          syntax: {
            highlight: (text) => hljs.highlightAuto(text).value,
          },
        },
      },
      content: '',
    };
  },
  methods: {
    imageHandler() {
      let range = this.editor.getSelection();
      let value = prompt('Nhập đường dẫn ảnh ');
      if (value) {
        this.editor.insertEmbed(range.index, 'image', value);
      }
    },
    onEditorChange: debounce(function (value) {
      this.$emit('update:value', value.html);
      this.$emit('input', value.html);
    }, 466),
    onEditorBlur() {},
    onEditorFocus() {},
    onEditorReady() {},
  },
};
</script>
<style lang="scss">
.ql-editor.ql-blank {
  min-height: 155px;
}
</style>
