require.config({
    baseUrl: '/scripts/',
    paths: {
        'react': '../vendor/react',
        'react-dom': '../vendor/react-dom',
        'react-router': '../vendor/ReactRouter',
        'marked': '../vendor/marked',
        'transliteration': '../vendor/transliteration',
        'fetch': '../vendor/fetch'
    }
});

require(['app']);