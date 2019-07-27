const path = require('path');
const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');

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
            // Add '.ts' and '.tsx' as resolvable extensions.
            extensions: ['.js', '.jsx', ".ts", ".tsx"]
        },
        output: {
            path: path.resolve(__dirname, './build'),
            filename: '[name].js'
        },
        devServer: {
            contentBase: path.resolve(__dirname, 'src', 'views'),
            compress: true,
            historyApiFallback: true,
            port: 8085,
            publicPath: '/build/',
            watchContentBase: true,
            writeToDisk: true,
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
                filename: path.resolve(__dirname, 'src', 'views', 'index.html'),
                template: path.resolve(__dirname, 'src', 'views', 'index.html.template'),
                templateParameters: { baseHref: '' },
                chunks: [ 'main' ]
            }),
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative('/build/', '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
        ] : [])
    }];
};