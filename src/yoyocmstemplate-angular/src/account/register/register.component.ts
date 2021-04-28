import { AppComponentBase } from '@shared/component-base/app-component-base';
import { Component, Injector, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Validators } from '@angular/forms';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import {
  AccountServiceProxy,
  RegisterInput,
  RegisterOutput,
  AuthenticateModel,
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from 'abpPro/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';

import { LoginService } from '../login/login.service';
import { AppCaptchaType } from 'abpPro/AppEnums';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less'],
  animations: [appModuleAnimation()],
})
export class RegisterComponent extends AppComponentBase implements OnInit {
  model: RegisterInput = new RegisterInput();
  saving = false;

  /**
   * 是否使用验证码
   */
  get useCaptcha(): boolean {
    return this.setting.getBoolean('App.UseCaptchaOnUserRegistration');
  }

  /**
   * 验证码类型
   */
  get captchaType(): number {
    return AppCaptchaType.TenantUserRegister;
  }
  /** 验证码时间戳 */
  captchaTimestamp: any;

  constructor(
    injector: Injector,
    private _accountService: AccountServiceProxy,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    readonly _loginService: LoginService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.titleSrvice.setTitle(this.l('CreateAnAccount'));


    if (!this.appSession.tenant) {
      this.back();
      return;
    }
    this.model = new RegisterInput();
  }

  back(isEmptyAuthen: boolean = true): void {
    if (isEmptyAuthen) {
      this._loginService.authentSuccessModel = undefined;
    }
    this._router.navigate(['/account/login']);
  }


  save(): void {

    // 判断是不是从第三方授权过来的
    if (this._loginService.authentSuccessModel) {
      this.model.providerKey = this._loginService.authentSuccessModel.providerKey;
      this.model.authProvider = this._loginService.authentSuccessModel.authProvider;
    }



    this.saving = true;
    this._accountService
      .register(this.model)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe((result: RegisterOutput) => {
        if (!result.canLogin) {
          this.notify.success(this.l('SuccessfullyRegistered'));
          this._router.navigate(['/login']);
          return;
        }

        this.saving = true;

        // Autheticate
        this._loginService.authenticateModel.userNameOrEmailAddress = this.model.emailAddress;
        this._loginService.authenticateModel.password = this.model.password;
        this._loginService.authenticate((success: boolean) => {
          this.saving = false;
          // 如果是第三方登录 没有历史跳转

        }, this.model.providerKey !== undefined ? AppConsts.appBaseUrl : undefined);


      });
  }

  /** 更新验证码时间戳 */
  private updateCaptchaTimestamp() {
    this.captchaTimestamp = new Date().getTime();
  }
}
