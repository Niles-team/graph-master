const path = require('path');
const webpack = require('webpack');
const bundleOutputDir = './wwwroot/dist';
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const tsNameof = require("ts-nameof");

const webpackServeOptions = {
    devMiddleware: {
        publicPath: '/dist/',
        writeToDisk: true,
        headers: {
            'Access-Control-Allow-Origin': '*'
        }
    },
    host: 'localhost',
    port: 8085
};

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    return [{
        mode: isDevBuild ? "development" : "production",
        devtool: isDevBuild ? "eval-source-map" : "none",
        stats: { modules: false },
        entry: {
            'main': './ClientApp/boot.tsx',
        },
        resolve: {
            alias: {
                '~': path.resolve(__dirname, 'ClientApp')
            },
            extensions: ['.js', '.jsx', '.ts', '.tsx']
        },
        output: {
            path: path.join(__dirname, bundleOutputDir),
            filename: '[name].js',
            publicPath: process.env.WEBPACK_SERVE ?
                `http://${webpackServeOptions.host}:${webpackServeOptions.port}/dist/` :
                '/dist/'
        },
        serve: webpackServeOptions,
        module: {
            rules: [
                {
                    test: /\.tsx?$/,
                    include: path.resolve(__dirname, 'ClientApp'),
                    loader: "ts-loader",
                    options: {
                        transpileOnly: true,
                        getCustomTransformers: () => ({ before: [tsNameof] })
                    }
                },
                {
                    test: /\.(woff|woff2|eot|ttf|svg)(\?|$)/,
                    include: path.resolve(__dirname, 'ClientApp'),
                    use: 'url-loader?limit=100000'
                },
                // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
                {
                    enforce: "pre",
                    test: /\.js$/,
                    loader: "source-map-loader"
                }
            ]
        },
        plugins: [
            new ForkTsCheckerWebpackPlugin({
                memoryLimit: 7500,
            }),
            new HtmlWebpackPlugin({
                filename: path.resolve(__dirname, 'Views', 'Shared', '_Layout.cshtml'),
                template: path.resolve(__dirname, 'Views', 'Shared', '_Layout.cshtml.template'),
                templateParameters: { baseHref: '~/' },
                chunks: ['main']
            }),
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(bundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
        ] : [])
    }];
};
