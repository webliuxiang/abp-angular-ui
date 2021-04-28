import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AuthenticateResultModel,
  TokenAuthServiceProxy,
  ActivateAccountModel,
  ActivateType,
  AuthenticateModel,
} from '@shared/service-proxies/service-proxies';
import { AppCaptchaType } from 'abpPro/AppEnums';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LoginService } from '../login/login.service';

@Component({
  selector: 'app-activation',
  templateUrl: './activation.component.html',
  styleUrls: ['./activation.component.less'],
  animations: [appModuleAnimation()],
})
export class ActivationComponent extends AppComponentBase implements OnInit {
  ActivateType = ActivateType;
  public model: ActivateAccountModel & { passwordConfirm?: string };
  private loginResult: AuthenticateResultModel;


  /** 是否使用验证码 */
  get useCaptcha(): boolean {
    return this.setting.getBoolean('App.UseCaptchaOnUserRegistration');
  }

  /** 验证码类型 */
  get captchaType(): number {
    return AppCaptchaType.TenantUserRegister;
  }

  /** 验证码时间戳 */
  captchaTimestamp: any;

  constructor(
    injector: Injector,
    private _router: Router,
    private _tokenAuthServiceProxy: TokenAuthServiceProxy,
    private _loginService: LoginService,
    private _activatedRoute: ActivatedRoute,
  ) {
    super(injector);
  }


  async ngOnInit() {
    this.titleSrvice.setTitle(this.l('ActivateAccount'));
    this._activatedRoute.paramMap.subscribe(async _ => {
      this.loginResult = window.history.state;
      this.model = new ActivateAccountModel({
        password: undefined,
        verificationCode: undefined,
        emailAddress: undefined,
        activateType: ActivateType.NewAccount,
        userId: this.loginResult.userId.toString(),
      });
      this.loginResult.waitingForActivation = true;
      if (!this.loginResult.waitingForActivation) {
        this._router.navigate(['account/login']);
      }
    });
  }


  toggleActivateType() {
    if (this.model.activateType === ActivateType.NewAccount) {
      this.model.activateType = ActivateType.BindExistAccount;
    } else {
      this.model.activateType = ActivateType.NewAccount;
    }
  }

  async activateAccount() {
    const res = await this._tokenAuthServiceProxy
      .activateAccount(this.model)
      .toPromise();
    this._loginService.authenticateModel = new AuthenticateModel({
      userNameOrEmailAddress: this.model.emailAddress,
      password: this.model.password,
      verificationCode: this.model.verificationCode,
      rememberClient: false,
      returnUrl: undefined,
      authProvider: undefined,
      providerKey: undefined,
    });
    this._loginService.authenticate((success: boolean) => {
      if (!success && this.useCaptcha) {
        this.updateCaptchaTimestamp();
      }
    });
  }

  /** 更新验证码时间戳 */
  private updateCaptchaTimestamp() {
    this.captchaTimestamp = new Date().getTime();
  }
}
