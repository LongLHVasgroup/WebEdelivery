const CracoLessPlugin = require('craco-less');

module.exports = {
  plugins: [
    {
      plugin: CracoLessPlugin,
      options: {
        lessLoaderOptions: {
          lessOptions: {
            modifyVars: {
              '@primary-color': '#f26600', '@font-family': 'Glypha VO'
              ,'@background-color-light': '#f5f5f5'
        },
        javascriptEnabled: true,
      },
    },
      },
    },
  ],
};