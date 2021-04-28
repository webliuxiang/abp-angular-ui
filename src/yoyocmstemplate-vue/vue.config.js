const CopyWebpackPlugin = require('copy-webpack-plugin');


module.exports = {
    lintOnSave: false,
    transpileDependencies: [

    ],
    css: {
        loaderOptions: {
            less: {
                javascriptEnabled: true,
            }
        }
    },
    devServer: {
        before: app => {

        },
    },
    chainWebpack: config => {

    },
    configureWebpack: config => {
        if (process.env.NODE_ENV === 'production') {
            return {
                plugins: [
                    new CopyWebpackPlugin([
                        {
                            from: 'libs/abp/abp.js',
                            to: 'libs/abp'
                        },
                        {
                            from: './node_modules/@microsoft/signalr/dist/browser/signalr.js',
                            to: 'libs/signalr'
                        },
                        {
                            from: './node_modules/abp-web-resources/Abp/Framework/scripts/libs',
                            to: './public/assets/abp-web-resources'
                        },
                        {
                            from: './web.config',
                            to: './'
                        }
                    ])
                ]
            }
        } else {
            return {
                plugins: [
                    new CopyWebpackPlugin([
                        {
                            from: 'libs/abp/abp.js',
                            to: 'libs/abp'
                        },
                        {
                            from: './node_modules/@microsoft/signalr/dist/browser/signalr.js',
                            to: 'libs/signalr'
                        },
                        {
                            from: './node_modules/abp-web-resources/Abp/Framework/scripts/libs',
                            to: './public/assets/abp-web-resources'
                        }
                    ])
                ]
            }
        }
    }
}
