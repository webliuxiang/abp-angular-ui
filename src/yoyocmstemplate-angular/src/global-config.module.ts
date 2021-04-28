import { LOCALE_ID, ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { ALAIN_I18N_TOKEN, AlainThemeModule } from '@delon/theme';
import { AlainConfig, ALAIN_CONFIG } from '@delon/util';
import { throwIfAlreadyLoaded } from '@shared/module-import-guard';

// Please refer to: https://ng-alain.com/docs/global-config
// #region NG-ALAIN Config

import { DelonACLModule } from '@delon/acl';

const alainConfig: AlainConfig = {
  st: { modal: { size: 'lg' } },
  pageHeader: { homeI18n: 'home', recursiveBreadcrumb: true },
  lodop: {
    license: `A59B099A586B3851E0F0D7FDBF37B603`,
    licenseA: `C94CEE276DB2187AE6B65D56B3FC2848`,
  },
  acl: {
    guard_url: '/account/login',
  },
};

const alainModules = [
  AlainThemeModule.forRoot(),
  DelonACLModule.forRoot(),
];
const alainProvides = [
  { provide: ALAIN_CONFIG, useValue: alainConfig },
];


// #region reuse-tab
import { RouteReuseStrategy } from '@angular/router';
import { ReuseTabService, ReuseTabStrategy } from '@delon/abc/reuse-tab';

alainProvides.push({
  provide: RouteReuseStrategy,
  useClass: ReuseTabStrategy,
  deps: [ReuseTabService],
} as any);

// #endregion

// #endregion

// Please refer to: https://ng.ant.design/docs/global-config/en#how-to-use
// #region NG-ZORRO Config

import { NzConfig, NZ_CONFIG } from 'ng-zorro-antd/core/config';


const ngZorroConfig: NzConfig = {};

const zorroProvides = [{ provide: NZ_CONFIG, useValue: ngZorroConfig }];

// #endregion


// #region app Config

import { API_LOGMANAGER_BASE_URL } from '@shared/service-proxies';
import { AppConsts } from 'abpPro/AppConsts';
import { LocalizationService } from '@shared/i18n/localization.service';

function getRemoteServiceBaseUrl(): string {
  return AppConsts.remoteServiceBaseUrl;
}

function getCurrentLanguage(): string {
  return abp.localization.currentLanguage.name;
}

const appProvides = [
  { provide: API_LOGMANAGER_BASE_URL, useFactory: getRemoteServiceBaseUrl },
  { provide: LOCALE_ID, useFactory: getCurrentLanguage },
  { provide: ALAIN_I18N_TOKEN, useClass: LocalizationService, multi: false },
];

// #endregion

@NgModule({
  imports: [...alainModules],
})
export class GlobalConfigModule {
  constructor(@Optional() @SkipSelf() parentModule: GlobalConfigModule) {
    throwIfAlreadyLoaded(parentModule, 'GlobalConfigModule');
  }

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: GlobalConfigModule,
      providers: [
        ...alainProvides,
        ...zorroProvides,
        ...appProvides,
      ],
    };
  }
}
