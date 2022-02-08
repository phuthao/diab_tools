const path = require('path');
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
  publicPath: '/',
  configureWebpack: {
    resolve: {
      alias: {
        // If using the runtime only build
        vue$: 'vue/dist/vue.runtime.esm.js', // 'vue/dist/vue.runtime.common.js' for webpack 1
        // Or if using full build of Vue (runtime + compiler)
        // vue$: 'vue/dist/vue.esm.js'      // 'vue/dist/vue.common.js' for webpack 1
      },
    },
    plugins: [
      new CopyWebpackPlugin({
        patterns: [
          {
            from: 'node_modules/oidc-client/dist/oidc-client.min.js',
            to: 'js',
          },
          {
            from: 'node_modules/oidc-client/dist/oidc-client.js',
            to: 'js',
          },
        ],
      }),
    ],
  },
  chainWebpack: (config) => {
    config.module
      .rule('eslint')
      .use('eslint-loader')
      .tap((options) => {
        options.configFile = path.resolve(__dirname, '.eslintrc.js');
        return options;
      });
  },
  css: {
    loaderOptions: {
      postcss: {
        config: {
          path: __dirname,
        },
      },
      scss: {
        prependData: `@import "@/assets/sass/vendors/vue/vuetify/variables.scss";`,
      },
    },
  },
  transpileDependencies: ['vuetify'],
};
