// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  SERVER_URL: `./`,
  production: false,

  envName: 'dev',
  useHash: true,
  hmr: false,
  externalLogin: {
    redirectUri: 'http://localhost:4200/account/login-callback',
    qq: {
      appId: '101615423',
    },
    wecaht: {
      appId: 'your_app_id',
    },
    microsoft: {
      consumerKey: '6985604e-5ac8-4c9f-899c-717e3b423c0d',
    },
    facebook: {
      appId: '2483742038386623',
    },
    google: {
      clientId: '1059737066448-rhcatg45sp7jlpo1kr13hoavcmo36a0k.apps.googleusercontent.com',
    },
  },
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
