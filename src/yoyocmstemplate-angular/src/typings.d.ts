// # 3rd Party Library
// If the library doesn't have typings available at `@types/`,
// you can still use it by manually adding typings for it
///<reference path="../node_modules/abp-web-resources/Abp/Framework/scripts/abp.d.ts"/>
///<reference path="../node_modules/abp-web-resources/Abp/Framework/scripts/libs/abp.signalr.d.ts"/>
///<reference path="../node_modules/moment/moment.d.ts"/>
///<reference path="../node_modules/moment-timezone/index.d.ts"/>

import { Observable } from 'rxjs';

// G2
declare global {
  export var AliyunUpload: any;
  export var Aliplayer: any;
  export var G2: any;
  export var DataSet: any;
  export var Slider: any;
}

declare global {
  export var globalThis: any;
}

/** 第三方登录 */
declare global {
  export const QC: {
    Login: {
      showPopup: (oOpts: { appId: string; redirectURI: string }) => void;
      check(): boolean;
      getMe(callback: (openId: string, accessToken: string) => void);
    } & ((
      options: {
        btnId: string;
        showModal: boolean;
        scope: string;
        size: 'A_XL' | 'A_L' | 'A_M' | 'A_S' | 'B_M' | 'B_S' | 'C_S';
      },
      loginFun: Function,
      logoutFun: Function,
      outCallBackFun: Function,
    ) => void);
  };

  export class WxLogin {
    constructor(param: {
      self_redirect?: boolean;
      id: string;
      appid: string;
      scope: string;
      redirect_uri: string;
      state?: string;
      style?: string;
      href?: string;
    });
  }
}
