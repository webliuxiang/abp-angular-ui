import { NgModule, Injector } from '@angular/core';
import { CommonModule, PlatformLocation, registerLocaleData } from '@angular/common';

import { RootComponent } from './root.component';
import { AppSessionService } from '@shared/session/app-session.service';
import { AppPreBootstrap } from './AppPreBootstrap';
import { AppConsts } from 'abpPro/AppConsts';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { HttpClientModule } from '@angular/common/http';
import { NzIconService } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzI18nService } from 'ng-zorro-antd/i18n';

import { APP_INITIALIZER } from '@angular/core';
import { RootRoutingModule } from './root-routing.module';
import { SharedModule } from '@shared/shared.module';

import { DelonLocaleService } from '@delon/theme';

import { ICONS_AUTO } from './style-icons-auto';
import { ICONS } from './style-icons';

import { AbpModule } from 'abp-ng2-module';
import * as _ from 'lodash';
import { MessageExtension } from '@shared/helpers/message.extension';
import { AppAuthService } from '@shared/auth';
import { CustomNgZorroModule } from '@shared/ng-zorro';
import { JsonSchemaModule } from '@shared/json-schema/json-schema.module';

export function appInitializerFactory(injector: Injector) {
  // 导入图标
  const iconSrv = injector.get(NzIconService);
  iconSrv.addIcon(...ICONS_AUTO, ...ICONS);

  return () => {
    return new Promise<boolean>((resolve, reject) => {
      // 覆盖ABP默认通知和消息提示
      overrideAbpMessageAndNotify(injector);

      // 启动程序初始化，获取基本配置信息
      AppPreBootstrap.run(injector, () => {
        // 获取当前登陆信息
        const appSessionService = injector.get(AppSessionService);
        appSessionService.init().then(
          result => {
            // 注册语言
            if (shouldLoadLocale()) {
              registerNgZorroLocales(injector);
              registerNgAlianLocales(injector);
              registerLocales(resolve, reject);
            } else {
              resolve(result);
            }
          },
          err => {
            // 这里获取登陆信息报错了的话，退出登陆，并刷新浏览器
            injector.get(AppAuthService).logout(true);
            reject(err);
          },
        );
      });
    });
  };
}

export function getBaseHref(platformLocation: PlatformLocation): string {
  const baseUrl = platformLocation.getBaseHrefFromDOM();
  if (baseUrl) {
    return baseUrl;
  }

  return '/';
}

/**
 * 覆盖abp自带的弹窗和通知
 * @param injector
 */
function overrideAbpMessageAndNotify(injector: Injector) {
  const nzNotifySerivce = injector.get(NzNotificationService);
  const nzModalService = injector.get(NzModalService);

  // 覆盖abp自带的通知和mssage
  // MessageExtension.overrideAbpMessageByMini(
  //   nzMsgService,
  //   nzModalService,
  // );

  //  覆盖abp.message替换为ng-zorro的模态框
  MessageExtension.overrideAbpMessageByNgModal(nzModalService);

  //  覆盖abp.notify替换为ng-zorro的notify
  MessageExtension.overrideAbpNotify(nzNotifySerivce);
}


/**
 * 注册ng-zorro的本地化
 * @param injector
 */
function registerNgZorroLocales(injector: Injector) {
  if (shouldLoadLocale()) {
    const ngZorroLcale = convertAbpLocaleToNgZorroLocale(abp.localization.currentLanguage.name);
    import(`ng-zorro-antd/esm5/i18n/languages/${ngZorroLcale}.js`).then(module => {
      const nzI18nService = injector.get(NzI18nService);
      nzI18nService.setLocale(module.default);
    });
  }
}

/**
 * 注册ng-alain的本地化
 * @param injector
 */
function registerNgAlianLocales(injector: Injector) {
  if (shouldLoadLocale()) {
    const ngAlianLocale = convertAbpLocaleToNgAlianLocale(abp.localization.currentLanguage.name);
    import(`@delon/theme/esm5/src/locale/languages/${ngAlianLocale}.js`).then(module => {
      const delonLocaleService = injector.get(DelonLocaleService);
      delonLocaleService.setLocale(module.default);
    });
  }
}

/**
 * 注册angular本地化
 * @param resolve
 * @param reject
 */
function registerLocales(resolve: (value?: boolean | Promise<boolean>) => void, reject: any) {
  if (shouldLoadLocale()) {
    const angularLocale = convertAbpLocaleToAngularLocale(abp.localization.currentLanguage.name);
    import(`@angular/common/locales/${angularLocale}.js`).then(module => {
      registerLocaleData(module.default);
      resolve(true);
    }, reject);
  } else {
    resolve(true);
  }
}

export function shouldLoadLocale(): boolean {
  return abp.localization.currentLanguage.name && abp.localization.currentLanguage.name !== 'en-US';
}

/**
 * 后端多语言编码转 Angular 前端多语言编码
 * @param locale
 */
export function convertAbpLocaleToAngularLocale(locale: string): string {
  const defaultLocale = 'zh';
  if (!AppConsts.localeMappings) {
    return defaultLocale;
  }

  const localeMapings = _.filter(AppConsts.localeMappings, { from: locale });
  if (localeMapings && localeMapings.length) {
    return localeMapings[0].to;
  }

  return defaultLocale;
}

/**
 * 后端多语言编码转 Ng-Zorro 前端多语言编码
 * @param locale
 */
export function convertAbpLocaleToNgZorroLocale(locale: string): string {
  const defaultLocale = 'zh_CN';
  if (!AppConsts.ngZorroLocaleMappings) {
    return defaultLocale;
  }

  const localeMapings = _.filter(AppConsts.ngZorroLocaleMappings, {
    from: locale,
  });
  if (localeMapings && localeMapings.length) {
    return localeMapings[0].to;
  }

  return defaultLocale;
}

/**
 * 后端多语言编码转 Ng-Alian 前端多语言编码
 * @param locale
 */
export function convertAbpLocaleToNgAlianLocale(locale: string): string {
  const defaultLocale = 'zh-CN';
  if (!AppConsts.ngAlainLocaleMappings) {
    return defaultLocale;
  }

  const localeMapings = _.filter(AppConsts.ngAlainLocaleMappings, {
    from: locale,
  });
  if (localeMapings && localeMapings.length) {
    return localeMapings[0].to;
  }

  return defaultLocale;
}


import { OverlayModule } from '@angular/cdk/overlay';
import { GlobalConfigModule } from './global-config.module';

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    BrowserModule,
    AbpModule,
    OverlayModule,
    // 引入DelonMdule
    GlobalConfigModule.forRoot(),
    ServiceProxyModule,
    RootRoutingModule,
    HttpClientModule,
    /** 导入 ng-zorro-antd 模块 */


    /** 必须导入 ng-zorro 才能导入此项 */
    SharedModule.forRoot(),
    // 自定义ng-zorro模块
    CustomNgZorroModule.forRoot(),
    JsonSchemaModule,
  ],
  declarations: [RootComponent],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializerFactory,
      deps: [Injector, PlatformLocation],
      multi: true,
    },
  ],
  bootstrap: [RootComponent],
})
export class RootModule {
}
