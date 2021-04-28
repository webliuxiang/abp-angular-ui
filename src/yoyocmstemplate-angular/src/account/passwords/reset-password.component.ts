import { Component, Injector, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountServiceProxy, ResetPasswordOutput, AuthenticateModel } from '@shared/service-proxies/service-proxies';
import { ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import { LoginService } from '../login/login.service';
import { AppSessionService } from '@shared/session/app-session.service';
import { AppUrlService } from '@shared/nav/app-url.service';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { ResetPasswordModel } from './reset-password.model';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/component-base/app-component-base';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./password.less'],
  animations: [accountModuleAnimation()],
})
export class ResetPasswordComponent extends AppComponentBase implements OnInit {
  model: ResetPasswordModel = new ResetPasswordModel();
  saving = false;

  constructor(
    injector: Injector,
    private _accountService: AccountServiceProxy,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _loginService: LoginService,
    private _appUrlService: AppUrlService,
    private _appSessionService: AppSessionService,
    private _profileService: ProfileServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.titleSrvice.setTitle(this.l('ResetPassword'));
    // this.model. = this._activatedRoute.snapshot.queryParams['userId'];
    this.model.resetCode = this._activatedRoute.snapshot.queryParams.resetCode;

    this._appSessionService.changeTenantIfNeeded(
      this.parseTenantId(this._activatedRoute.snapshot.queryParams.tenantId),
    );
  }

  save(): void {
    this.saving = true;
    this.saving = true;

    this.model.confirmPassword = this.model.passwordRepeat;

    this._accountService
      .resetPassword(this.model)
      .pipe(
        finalize(() => {
          this.saving = false;
        }),
      )
      .subscribe((result: ResetPasswordOutput) => {
        if (result.canLogin) {
          this._router.navigate(['account/login']);
          return;
        }

        // Autheticate
        // this.saving = true;
        // this._loginService.authenticateModel.userNameOrEmailAddress = result.;
        // this._loginService.authenticateModel.password = this.model.password;
        // this._loginService.authenticate(() => {
        //   this.saving = false;
        // });
      });
  }

  parseTenantId(tenantIdAsStr?: string): number {
    // tslint:disable-next-line:radix
    let tenantId = !tenantIdAsStr ? undefined : parseInt(tenantIdAsStr);
    if (tenantId === NaN) {
      tenantId = undefined;
    }

    return tenantId;
  }
}
