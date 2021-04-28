import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import { Router } from '@angular/router';
import { LoginService } from '../login/login.service';
import {
  TenantRegistrationServiceProxy,
  CreateTenantDto,
} from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { finalize } from 'rxjs/operators';
import { AppCaptchaType } from 'abpPro/AppEnums';

@Component({
  selector: 'app-tenant-register',
  templateUrl: './tenant-register.component.html',
  styleUrls: ['./tenant-register.component.less'],
  animations: [appModuleAnimation()],
})
export class TenantRegisterComponent extends AppComponentBase
  implements OnInit {
  model: CreateTenantDto = new CreateTenantDto();

  /** 是否使用验证码 */
  get useCaptcha(): boolean {
    return this.setting.getBoolean('App.Host.UseCaptchaOnTenantRegistration');
  }

  /** 验证码类型 */
  get captchaType(): number {
    return AppCaptchaType.HostTenantRegister;
  }

  /** 验证码时间戳 */
  captchaTimestamp: any;

  constructor(
    injector: Injector,
    private _router: Router,
    private readonly _loginService: LoginService,
    private readonly _tenantRegisterService: TenantRegistrationServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.titleSrvice.setTitle(this.l('TenantRegister'));

    if (this.appSession.tenant) {
      this.back();
      return;
    }
  }

  back(): void {
    this._router.navigate(['/account/login']);
  }

  save(): void {
    this.saving = true;
    this._tenantRegisterService
      .registerTenant(this.model)
      .pipe(
        finalize(() => {
          this.saving = false;
        }),
      )
      .subscribe(result => {
        this.notify.success(this.l('SavedSuccessfully'));
        abp.multiTenancy.setTenantIdCookie(result.tenantId);

        // 租户默认激活，并且登陆没有使用验证码，自动登陆
        if (result.isActive && !result.useCaptchaOnUserLogin) {
          this.saving = true;
          this._loginService.authenticateModel.userNameOrEmailAddress = this.model.adminEmailAddress;
          this._loginService.authenticateModel.password = this.model.tenantAdminPassword;
          this._loginService.authenticate(() => {
            this.saving = false;
          });
        } else {
          this.back();
        }
      });
  }

  /** 更新验证码时间戳 */
  private updateCaptchaTimestamp() {
    this.captchaTimestamp = new Date().getTime();
  }
}
