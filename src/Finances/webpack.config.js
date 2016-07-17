var webpack = require('webpack');
var path = require('path');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var extractSCSS = new ExtractTextPlugin('bundle.css');

module.exports = {
    entry: [
        './app/scripts/index.js',
        './app/styles/app.scss'
    ],
    module: {
        loaders: [
            {
                test: /\.jsx?$/,
                exclude: /node_modules/,
                loader: 'babel'
            },
            {
                test: /\.s?css$/,
                loader: extractSCSS.extract(['css', 'sass'])
            }
        ]
    },
    resolve: {
        root: path.resolve('./app/scripts'),
        extensions: ['', '.js', '.jsx']
    },
    output: {
        path: __dirname + '/wwwroot',
        publicPath: '/',
        filename: 'bundle.js'
    },
    devServer: {
        contentBase: './wwwroot',
        historyApiFallback: true
    },
    plugins: [
        extractSCSS
    ]
};
