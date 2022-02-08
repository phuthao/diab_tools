const fs = require('fs');
const path = require('path');

const prettierOptions = JSON.parse(
  fs.readFileSync(path.resolve(__dirname, '.prettierrc'), 'utf8'),
);

module.exports = {
  root: true,
  env: {
    node: true,
    es6: true,
  },
  extends: [
    'plugin:vue/essential',
    'eslint:recommended',
    'prettier',
    '@vue/prettier',
  ],
  rules: {
    'class-methods-use-this': 0,
    'no-confusing-arrow': 0,
    'no-unused-vars': 2,
    'no-use-before-define': 0,
    'no-console': 2,
    'no-debugger': 2,
    'no-empty': 1,
    'max-len': ['error', { code: 360 }],
    'vue/no-use-v-if-with-v-for': [
      'error',
      {
        allowUsingIterationVar: true,
      },
    ],
  },
  parserOptions: {
    parser: 'babel-eslint',
  },
};
