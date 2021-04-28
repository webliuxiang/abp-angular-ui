export const environment = {
  SERVER_URL: `./`,
  production: true,
  useHash: true,
  hmr: false,
  envName: 'staging',

  externalLogin: {
    redirectUri: 'http://yourdomain:8000/account/login-callback',
    qq: {
      appId: 'your_app_id',
    },
    wecaht: {
      appId: 'your_app_id',
    },
    microsoft: {
      consumerKey: 'your_consumer_key',
    },
    facebook: {
      appId: 'your_app_id',
    },
    google: {
      clientId: 'your_client_id',
    },
  },
};
