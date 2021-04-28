const webpack = require('webpack');
const path = require('path');
const fs = require('fs');
const del = require('del');
const glob = require('glob');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const WebpackRTLPlugin = require('webpack-rtl-plugin');
const TerserJSPlugin = require('terser-webpack-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const WebpackMessages = require('webpack-messages');
const ExcludeAssetsPlugin = require('webpack-exclude-assets-plugin');
const MergeIntoSingle = require('webpack-merge-and-include-globally');

// paths
const rootPath = path.resolve(__dirname, '..');

// arguments/params from the line command
const args = getParameters();
// get theme name
let theme = 'keen';
// get selected demo, default wwwroot
let demo = getDemos(rootPath)[0];

// under demo paths
const demoPath = rootPath + '/' + demo;
const distPath = demoPath + '/dist';
const assetDistPath = distPath + '/assets';
const srcPath = demoPath + '/src';

const extraPlugins = [];
const exclude = [];
const extraConfig = [];

const js = args.indexOf('js') !== -1;
const css = args.indexOf('css') !== -1 || args.indexOf('scss') !== -1;

addtionalSettings();
importDatatables();

function importDatatables() {
  var rtlExt = '';
  if (args.indexOf('rtl') !== -1) {
    rtlExt = 'rtl.';
  }
  // Optional: bundle datatables.net
  extraPlugins.push(new MergeIntoSingle({
    files: [
      {
        src: [
          'node_modules/datatables.net/js/jquery.dataTables.js',
          'node_modules/datatables.net-bs4/js/dataTables.bootstrap4.js',
          '@/src/js/vendors/plugins/datatables.init.js',
          'node_modules/datatables.net-autofill/js/dataTables.autoFill.min.js',
          'node_modules/datatables.net-autofill-bs4/js/autoFill.bootstrap4.min.js',
          'node_modules/jszip/dist/jszip.min.js',
          'node_modules/pdfmake/build/pdfmake.min.js',
          'node_modules/pdfmake/build/vfs_fonts.js',
          'node_modules/datatables.net-buttons/js/dataTables.buttons.min.js',
          'node_modules/datatables.net-buttons-bs4/js/buttons.bootstrap4.min.js',
          'node_modules/datatables.net-buttons/js/buttons.colVis.js',
          'node_modules/datatables.net-buttons/js/buttons.flash.js',
          'node_modules/datatables.net-buttons/js/buttons.html5.js',
          'node_modules/datatables.net-buttons/js/buttons.print.js',
          'node_modules/datatables.net-colreorder/js/dataTables.colReorder.min.js',
          'node_modules/datatables.net-fixedcolumns/js/dataTables.fixedColumns.min.js',
          'node_modules/datatables.net-fixedheader/js/dataTables.fixedHeader.min.js',
          'node_modules/datatables.net-keytable/js/dataTables.keyTable.min.js',
          'node_modules/datatables.net-responsive/js/dataTables.responsive.min.js',
          'node_modules/datatables.net-responsive-bs4/js/responsive.bootstrap4.min.js',
          'node_modules/datatables.net-rowgroup/js/dataTables.rowGroup.min.js',
          'node_modules/datatables.net-rowreorder/js/dataTables.rowReorder.min.js',
          'node_modules/datatables.net-scroller/js/dataTables.scroller.min.js',
          'node_modules/datatables.net-select/js/dataTables.select.min.js',
        ],
        dest: 'plugins/custom/datatables/datatables.bundle.js',
      },
      {
        src: [
          'node_modules/datatables.net-bs4/css/dataTables.bootstrap4.css',
          'node_modules/datatables.net-buttons-bs4/css/buttons.bootstrap4.min.css',
          'node_modules/datatables.net-autofill-bs4/css/autoFill.bootstrap4.min.css',
          'node_modules/datatables.net-colreorder-bs4/css/colReorder.bootstrap4.min.css',
          'node_modules/datatables.net-fixedcolumns-bs4/css/fixedColumns.bootstrap4.min.css',
          'node_modules/datatables.net-fixedheader-bs4/css/fixedHeader.bootstrap4.min.css',
          'node_modules/datatables.net-keytable-bs4/css/keyTable.bootstrap4.min.css',
          'node_modules/datatables.net-responsive-bs4/css/responsive.bootstrap4.min.css',
          'node_modules/datatables.net-rowgroup-bs4/css/rowGroup.bootstrap4.min.css',
          'node_modules/datatables.net-rowreorder-bs4/css/rowReorder.bootstrap4.min.css',
          'node_modules/datatables.net-scroller-bs4/css/scroller.bootstrap4.min.css',
          'node_modules/datatables.net-select-bs4/css/select.bootstrap4.min.css',
        ],
        dest: 'plugins/custom/datatables/datatables.bundle.' + rtlExt + 'css',
      },
    ],
  }));
}

function addtionalSettings() {
  if (args.indexOf('rtl') !== -1) {
    // enable rtl for css
    extraPlugins.push(new WebpackRTLPlugin({
      filename: '[name].rtl.css',
    }));
  }

  if (!js && css) {
    // exclude js files
    exclude.push('\.js$');
  }

  if (js && !css) {
    // exclude css files
    exclude.push('\.s?css$');
  }

  if (exclude.length) {
    // add plugin for exclude assets (js/css)
    extraPlugins.push(new ExcludeAssetsPlugin({
      path: exclude,
    }));
  }
}

function getEntryFiles() {
  const entries = {
    // 3rd party plugins css/js
    'plugins/global/plugins.bundle': ['./webpack/plugins/plugins.js', './webpack/plugins/plugins.scss'],
    // Keen css/js
    'css/style.bundle': path.relative('./', srcPath) + '/sass/style.scss',
    'js/scripts.bundle': './webpack/scripts.' + demo + '.js',
  };

  // Custom 3rd party plugins
  (glob.sync('./webpack/plugins/custom/**/*.+(js)') || []).forEach(file => {
    let loc = file.replace('webpack/', '').replace('./', '');
    if (path.basename(file) === 'gmaps.js') {
      loc = loc.replace('.js', '');
    }
    else {
      loc = loc.replace('.js', '.bundle');
    }
    entries[loc] = file;
  });

  // Keen css pages (single page use)
  (glob.sync(path.relative('./', srcPath) + '/sass/pages/**/!(_)*.scss') || []).forEach(file => {
    entries[file.replace(/.*sass\/(.*?)\.scss$/ig, 'css/$1')] = file;
  });
  (glob.sync(path.relative('./', srcPath) + '/js/pages/**/!(_)*.js') || []).forEach(file => {
    entries[file.replace(/.*js\/(.*?)\.js$/ig, 'js/$1')] = file;
  });

  // Keen theme
  (glob.sync(path.relative('./', srcPath) + '/sass/themes/**/!(_)*.scss') || []).forEach(file => {
    entries[file.replace(/.*sass\/(.*?)\.scss$/ig, 'css/$1')] = file;
  });

  return entries;
}

function mainConfig() {
  return {
    // enabled/disable optimizations
    mode: args.indexOf('prod') === 0 ? 'production' : 'development',
    // console logs output, https://webpack.js.org/configuration/stats/
    stats: 'errors-warnings',
    performance: {
      // disable warnings hint
      hints: false,
    },
    optimization: {
      // js and css minimizer
      minimizer: [new TerserJSPlugin({}), new OptimizeCSSAssetsPlugin({})],
    },
    entry: getEntryFiles(),
    output: {
      // main output path in assets folder
      path: assetDistPath,
      // output path based on the entries' filename
      filename: '[name].js',
    },
    resolve: {
      alias: {
        jquery: path.join(__dirname, 'node_modules/jquery/src/jquery'),
        '@': demoPath,
      },
      extensions: ['.js', '.scss'],
    },
    devtool: 'source-map',
    plugins: [
      new WebpackMessages({
        name: theme,
        logger: str => console.log(`>> ${str}`),
      }),
      // create css file
      new MiniCssExtractPlugin({
        filename: '[name].css',
      }),
      new CopyWebpackPlugin([
        {
          // copy media
          from: srcPath + '/media',
          to: assetDistPath + '/media',
        },
        {
          // copy tinymce skins
          from: path.resolve(__dirname, 'node_modules') + '/tinymce/skins',
          to: assetDistPath + '/plugins/custom/tinymce/skins',
        },
        {
          // copy tinymce plugins
          from: path.resolve(__dirname, 'node_modules') + '/tinymce/plugins',
          to: assetDistPath + '/plugins/custom/tinymce/plugins',
        },
      ]),
    ].concat(extraPlugins),
    module: {
      rules: [
        {
          test: /\.css$/,
          use: [
            MiniCssExtractPlugin.loader,
            'css-loader',
          ],
        },
        {
          test: /\.scss$/,
          use: [
            MiniCssExtractPlugin.loader,
            {
              loader: 'css-loader',
              options: {
                url: (url, resourcePath) => {
                  // Don't handle local urls
                  return !!url.includes('media');
                },
              },
            },
            {
              loader: 'postcss-loader', // Run post css actions
              options: {
                plugins: function() { // post css plugins, can be exported to postcss.config.js
                  return [
                    // require('precss'),
                    require('autoprefixer'),
                  ];
                },
              },
            },
            {
              loader: 'sass-loader',
              options: {
                sourceMap: false,
                includePaths: [demoPath],
              },
            },
          ],
        },
        {
          test: /\.(ttf|otf|eot|svg|woff(2)?)(\?[a-z0-9]+)?$/,
          include: [
            path.resolve(__dirname, 'node_modules'),
            rootPath,
          ],
          use: [
            {
              loader: 'file-loader',
              options: {
                // emitFile: false,
                // prevent name become hash
                name: '[name].[ext]',
                // move files
                outputPath: 'plugins/global/fonts',
                // rewrite path in css
                publicPath: 'fonts',
              },
            },
          ],
        },
        {
          test: /\.(gif|png|jpe?g)$/,
          include: [
            path.resolve(__dirname, 'node_modules'),
          ],
          use: [
            {
              loader: 'file-loader',
              options: {
                // emitFile: false,
                name: '[path][name].[ext]',
                publicPath: (url, resourcePath, context) => {
                  return path.basename(url);
                },
                outputPath: (url, resourcePath, context) => {
                  var plugin = url.match(/node_modules\/(.*?)\//i);
                  if (plugin) {
                    return `plugins/custom/${plugin[1]}/${path.basename(url)}`;
                  }
                  return url;
                },
              },
            },
          ],
        },
      ],
    },
    // webpack dev server config
    devServer: {
      contentBase: demoPath + '/dist',
      compress: true,
      port: 3000,
    },
  };
}

function getParameters() {
  // remove first 2 unused elements from array
  let argv = JSON.parse(process.env.npm_config_argv).cooked.slice(2);
  argv = argv.map((arg) => {
    return arg.replace(/--/i, '');
  });
  return argv;
}

function getDemos(pathDemos) {
  // get possible demo from parameter command
  let demos = [];
  args.forEach((arg) => {
    const demo = arg.match(/^demo.*/g);
    if (demo) {
      demos.push(demo[0]);
    }
  });

  if (demos.length === 0) {
    demos = ['wwwroot'];
    if (args.indexOf('alldemos') !== -1) {
      try {
        // sync reusable source code with wwwroot for all other demos
        demos = fs.readdirSync(pathDemos).filter((file) => {
          return !/(^|\/)\.[^\/\.]/g.test(file) && /^demo\d+$/g.test(file) && file !== 'demo0';
        });
      }
      catch (err) {
        console.error('Failed to read demo folder: ' + pathDemos);
      }
    }
  }

  return demos;
}

module.exports = () => {
  return [mainConfig()];
};
