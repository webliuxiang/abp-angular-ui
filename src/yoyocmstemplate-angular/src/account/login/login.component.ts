import { Component, Injector, ViewChild, OnInit } from '@angular/core';

import { LoginService } from './login.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import { AbpSessionService } from 'abp-ng2-module';
import { SessionServiceProxy, UpdateUserSignInTokenOutput } from '@shared/service-proxies';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { AppCaptchaType } from 'abpPro/AppEnums';
import { AppConsts } from 'abpPro/AppConsts';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
  animations: [appModuleAnimation()],
})
export class LoginComponent extends AppComponentBase implements OnInit {

  submitting = false;

  verificationImgUrl = '';

  /** 是否启用多租户 */
  get isMultiTenancyEnabled(): boolean {
    return abp.multiTenancy.isEnabled;
  }

  /** 是否已选中租户 */
  get multiTenancySideIsTeanant(): boolean {
    return this.appSession.tenantId > 0;
  }

  /** 允许注册租户 */
  get isTenantSelfRegistrationAllowed(): boolean {
    if (this.appSession.tenantId) {
      return false;
    }
    return this.setting.getBoolean('App.Host.AllowSelfRegistration');
  }


  /** 允许注册用户 */
  get isSelfRegistrationAllowed(): boolean {
    if (!this.appSession.tenantId) {
      return false;
    }
    return this.setting.getBoolean('App.AllowSelfRegistrationUser');
  }

  /** 是否使用登陆验证码 */
  get useCaptcha(): boolean {
    return this.setting.getBoolean('App.UserManagement.EnableCapthaOnLogin');
  }

  /** 验证码时间戳 */
  captchaTimestamp: any;


  /** 激活的三方登陆类型 */
  get enabledExternalLoginTypes(): string[] {
    return JSON.parse(this.setting.get('App.UserManagement.ExternalLoginProviders'));
  }

  constructor(
    injector: Injector,
    public loginService: LoginService,
    private _abpSessionService: AbpSessionService,
    private _sessionAppService: SessionServiceProxy,
  ) {
    super(injector);
  }

  getCaptchaType(): number {
    if (this.appSession.tenantId) {
      return AppCaptchaType.TenantUserLogin;
    } else {
      return AppCaptchaType.HostUserLogin;
    }
  }



  async ngOnInit() {
    this.titleSrvice.setTitle(this.l('LogIn'));

    if (this._abpSessionService.userId > 0 && UrlHelper.getReturnUrl()) {
      this._sessionAppService.updateUserSignInToken().subscribe((result: UpdateUserSignInTokenOutput) => {
        const initialReturnUrl = UrlHelper.getReturnUrl();
        const returnUrl =
          initialReturnUrl +
          (initialReturnUrl.indexOf('?') >= 0 ? '&' : '?') +
          'accessToken=' +
          result.signInToken +
          '&userId=' +
          result.encodedUserId +
          '&tenantId=' +
          result.encodedTenantId;

        location.href = returnUrl;
      });
    }
  }

  /** 登录按钮 */
  login(): void {
    this.submitting = true;
    this.loginService.authenticate((success: boolean) => {
      if (!success) {
        // 登陆失败,刷新验证码
        this.updateCaptchaTimestamp();
      }

      this.submitting = false;
    });
  }

  //#region 验证码功能
  onKey(e: KeyboardEvent): any {
    if (e.key === 'Tab') {
      this.initImg();
    }
  }
  initImg(): void {
    const userName = this.loginService.authenticateModel.userNameOrEmailAddress;
    if (!userName || userName === '' || this.verificationImgUrl !== '') {
      return;
    }
    this.clearimg();
  }

  clearimg(): void {
    const userName = this.loginService.authenticateModel.userNameOrEmailAddress;
    if (!userName || userName === '') {
      // 未输入账号
      return;
    }

    let tid: any = this.appSession.tenantId;
    if (!tid) {
      tid = '';
    }
    const timestamp = new Date().getTime();

    this.verificationImgUrl =
      AppConsts.remoteServiceBaseUrl +
      '/api/TokenAuth/GenerateVerification' +
      '?name=' +
      userName +
      '&tid=' +
      tid +
      '&t=' +
      timestamp;
  }

  /** 更新验证码时间戳 */
  private updateCaptchaTimestamp() {
    this.captchaTimestamp = new Date().getTime();
  }
}
