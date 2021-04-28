import { Injectable, FactoryProvider, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import {
  ExternalLoginProviderInfoModel,
  ExternalAuthenticateModel,
  ExternalAuthenticateResultModel,
} from '@shared/service-proxies/service-proxies';

import {
  TokenAuthServiceProxy,
  AuthenticateModel,
  AuthenticateResultModel,
} from '@shared/service-proxies/api-service-proxies';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { AppConsts } from 'abpPro/AppConsts';

import * as _ from 'lodash';
import { TokenService, UtilsService, MessageService, LogService } from 'abp-ng2-module';
import { environment } from '@env/environment';
import { ScriptLoaderService } from '@shared/utils/script-loader.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { SocialUser } from 'angularx-social-login';
import { finalize } from 'rxjs/operators';

import { html } from 'lit-html';

/** 登录服务 */
@Injectable()
export class LoginService {
  static readonly twoFactorRememberClientTokenName = 'TwoFactorRememberClientToken';

  authenticateModel: AuthenticateModel = new AuthenticateModel();
  authenticateResult: AuthenticateResultModel;
  rememberMe: boolean;
  originLogin: boolean;
  // 存储成功授权登录后的授权变量
  authentSuccessModel: ExternalAuthenticateModel = undefined;

  constructor(
    private _tokenAuthService: TokenAuthServiceProxy,
    private _router: Router,
    public zone: NgZone,
    private _utilsService: UtilsService,
    private _messageService: MessageService,
    private _tokenService: TokenService,
    private _logService: LogService,
    private _modalService: NzModalService
  ) {
  }

  /** 系统内置登录 */
  authenticate(finallyCallback?: (success: boolean) => void, redirectUrl?: string): void {
    finallyCallback = finallyCallback || ((successState: boolean) => {
      console.log('finallyCallback NULL');
    });


    let success = false;
    this._tokenAuthService
      .authenticate(this.authenticateModel)
      .pipe(
        finalize(() => {
          finallyCallback(success);
        }),
      )
      .subscribe(result => {
        success = true;
        this.processAuthenticateResult(result, redirectUrl);
      });
  }

  private processAuthenticateResult(authenticateResult: AuthenticateResultModel, redirectUrl?: string) {
    this.originLogin = authenticateResult.needToChangeThePassword;

    if (authenticateResult.needToChangeThePassword) {
      // 需要重置密码
      this._router.navigate(['account/reset-password'], {
        queryParams: {
          userId: authenticateResult.userId,
          tenantId: abp.session.tenantId,
        },
      });
    }
    if (authenticateResult.accessToken) {
      // Successfully logged in
      // tslint:disable-next-line:max-line-length
      this.login(
        authenticateResult.accessToken,
        authenticateResult.encryptedAccessToken,
        authenticateResult.expireInSeconds,
        this.rememberMe,
      );
    } else {
      // Unexpected result!
      this._logService.warn('Unexpected authenticateResult!');
      this._router.navigate(['account/login']);
    }
  }

  /**
   * 登录
   * @param accessToken token
   * @param encryptedAccessToken 加密过的token
   * @param expireInSeconds 过期秒数
   * @param rememberMe 记住我
   * @param twoFactorRememberClientToken 双重验证
   * @param redirectUrl 重定向url
   */
  private login(
    accessToken: string,
    encryptedAccessToken: string,
    expireInSeconds: number,
    rememberMe?: boolean,
    twoFactorRememberClientToken?: string,
    redirectUrl?: string,
  ): void {
    const tokenExpireDate = rememberMe ? new Date(new Date().getTime() + 1000 * expireInSeconds) : undefined;

    this._tokenService.setToken(accessToken, tokenExpireDate);

    this._utilsService.setCookieValue(
      AppConsts.authorization.encrptedAuthTokenName,
      encryptedAccessToken,
      tokenExpireDate,
      abp.appPath,
    );

    let initialUrl = UrlHelper.initialUrl;
    if (redirectUrl !== undefined) {
      initialUrl = redirectUrl;
    }
    if (initialUrl.indexOf('/login') > 0) {
      initialUrl = AppConsts.appBaseUrl;
    }

    location.href = initialUrl;
  }
}
