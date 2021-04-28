import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { AppAuthService, ImpersonationService } from '@shared/auth';
import { ChangePasswordModalComponent } from '@layout/default/profile/change-password-modal.component';
import { LoginAttemptsModalComponent } from '@layout/default/profile/login-attempts-modal.component';
import { MySettingsModalComponent } from '@layout/default/profile/my-settings-modal.component';

@Component({
  selector: 'header-user',
  templateUrl: './header-user.component.html',
  styles: [],
})
export class HeaderUserComponent extends AppComponentBase implements OnInit {
  loginUserName: string;
  isImpersonatedLogin: boolean;

  constructor(
    injector: Injector,
    private authService: AppAuthService,
    private impersonationService: ImpersonationService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.isImpersonatedLogin = this.abpSession.impersonatorUserId > 0;
    this.loginUserName = this.appSession.getShownLoginName();
  }

  changePassword(): void {
    this.modalHelper.open(ChangePasswordModalComponent).subscribe(result => {
      if (result) {
        this.logout();
      }
    });
  }

  showLoginAttempts(): void {
    this.modalHelper.open(LoginAttemptsModalComponent).subscribe(result => {
    });
  }

  changeMySettings(): void {
    this.modalHelper.open(MySettingsModalComponent).subscribe(result => {
    });
  }

  backToMyAccount(): void {
    this.impersonationService.backToImpersonator();
  }

  logout(): void {
    this.authService.logout();
  }
}
