import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  AccountServiceProxy,
  SendPasswordResetCodeInput,
} from '@shared/service-proxies/service-proxies';
import { AppUrlService } from '@shared/nav/app-url.service';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/component-base/app-component-base';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./password.less'],
  animations: [accountModuleAnimation()],
})
export class ForgotPasswordComponent extends AppComponentBase implements OnInit {
  model: SendPasswordResetCodeInput = new SendPasswordResetCodeInput();

  saving = false;

  constructor(
    injector: Injector,
    private _accountService: AccountServiceProxy,
    private _appUrlService: AppUrlService,
    private _router: Router,
  ) {
    super(injector);
  }


  ngOnInit(): void {
    this.titleSrvice.setTitle(this.l('ForgotPassword'));
  }

  save(): void {
    this.saving = true;
    this._accountService
      .sendPasswordResetCode(this.model)
      .pipe(
        finalize(() => {
          this.saving = false;
        }),
      )
      .subscribe(() => {
        this.message
          .success(this.l('PasswordResetMailSentMessage'), this.l('MailSent'))
          .done(() => {
            this._router.navigate(['account/login']);
          });
      });
  }
}
