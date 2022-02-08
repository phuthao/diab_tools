/**
 * Main file of webpack config for RTL.
 * Please do not modified unless you know what to do
 */
const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const WebpackRTLPlugin = require('webpack-rtl-plugin');
const WebpackMessages = require('webpack-messages');
const del = require('del');

// theme name
const themeName = 'metronic';
// global variables
const rootPath = path.resolve(__dirname);
const distPath = rootPath + '/src/assets';

const entries = {
  'css/style.vue': './src/assets/sass/style.vue.scss',
};

// remove older folders and files
(async () => {
  await del.sync(distPath + '/css', { force: true });
})();

const mainConfig = function () {
  return {
    mode: 'development',
    stats: 'errors-only',
    performance: {
      hints: false,
    },
    entry: entries,
    output: {
      // main output path in assets folder
      path: distPath,
      // output path based on the entries' filename
      filename: '[name].js',
    },
    resolve: { extensions: ['.scss'] },
    plugins: [
      // webpack log message
      new WebpackMessages({
        name: themeName,
        logger: (str) => console.log(`>> ${str}`),
      }),
      // create css file
      new MiniCssExtractPlugin({
        filename: '[name].css',
      }),
      new WebpackRTLPlugin({
        filename: '[name].rtl.css',
      }),
      {
        apply: (compiler) => {
          // hook name
          compiler.hooks.afterEmit.tap('AfterEmitPlugin', () => {
            (async () => {
              await del.sync(distPath + '/css/*.js', { force: true });
            })();
          });
        },
      },
    ],
    module: {
      rules: [
        {
          test: /\.s(c|a)ss$/,
          use: [
            'vue-style-loader',
            'css-loader',
            {
              loader: 'sass-loader',
              // Requires sass-loader@^7.0.0

              // Requires sass-loader@^8.0.0
              options: {
                implementation: require('sass'),
                sassOptions: {
                  indentedSyntax: true, // optional
                },
              },
            },
          ],
        },
        {
          test: /\.scss$/,
          use: [
            MiniCssExtractPlugin.loader,
            'css-loader',
            {
              loader: 'sass-loader',
              options: {
                sourceMap: true,
              },
            },
          ],
        },
      ],
    },
  };
};

module.exports = function () {
  return [mainConfig()];
};
