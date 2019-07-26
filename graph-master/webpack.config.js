const path = require('path');
const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');

const bundleOutputDir = './dist';

const webpackServeOptions = {
    devMiddleware: {
        publicPath: bundleOutputDir,
        writeToDisk: true,
        headers: {
            'Access-Control-Allow-Origin': '*'
        }
    },
    host: 'localhost',
    port: 80
};

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);

    return [{
        mode: isDevBuild ? "development" : "production",
        devtool: isDevBuild ? "eval-source-map" : "none",
        stats: {modules: false},
        entry: {
            'main': './src/index.tsx'
        },
        resolve: {
            alias: {
                '~': path.resolve(__dirname)
            },
            // Add '.ts' and '.tsx' as resolvable extensions.
            extensions: ['.js', '.jsx', ".ts", ".tsx"]
        },
        output: {
            path: path.join(__dirname, bundleOutputDir),
            filename: '[name].js',
            publicPath: process.env.WEBPACK_SERVE ?
                        `http://${webpackServeOptions.host}:${webpackServeOptions.port}/dist/` :
                        ''
        },
        devServer: {
            contentBase: './dist',
            open: path.resolve(__dirname, 'dist', 'index.html'),
            hot: true,
            port: 8085
          },
        module: {
            rules: [
                {
                    test: /\.ts(x?)$/,
                    include: __dirname,
                    use: [
                        {
                            loader: "ts-loader"
                        }
                    ]
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
            new HtmlWebpackPlugin({
                filename: 'index.html',
                template: path.resolve(__dirname, 'views', 'index.html.template'),
                templateParameters: { baseHref: '~/' },
                chunks: [ 'main' ]
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