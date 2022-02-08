<template>
  <b-avatar
    :class="styleClass"
    :variant="variant"
    :size="size"
    :text="generateTwoLetterFromName(text)"
    :src="src"
    :icon="src"
    :rounded="rounded"
    :square="square"
  ></b-avatar>
</template>

<script>
export default {
  props: {
    styleClass: {
      type: String,
      default: null,
    },
    variant: {
      type: String,
      default: 'light',
    },
    size: {
      type: String,
      default: '30px',
    },
    text: {
      type: String,
      default: '',
    },
    src: {
      type: String,
      default: null,
    },
    icon: {
      type: String,
      default: null,
    },
    rounded: {
      type: [Boolean, String],
      default: false,
    },
    square: {
      type: [Boolean],
      default: false,
    },
  },
  methods: {
    generateTwoLetterFromName(name = '') {
      if (!name) {
        return '';
      }

      let letters = '';
      const splitName = name.split(' ');
      const splitNameLength = splitName.length;

      if (splitNameLength === 1) {
        letters = splitName[0].charAt(0);
      } else if (splitNameLength >= 2) {
        const firstLetter = splitName[0].charAt(0);
        const secondLetter = splitName[splitNameLength - 1].charAt(0);
        letters = `${firstLetter}${secondLetter}`;
      }
      return letters;
    },
  },
};
</script>
