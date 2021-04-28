import { ForgotPasswordComponent } from './passwords/forgot-password.component';
import { CommonModule } from '@angular/common';
import { NgModule, NgZone } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AccountRoutingModule } from './account-routing.module';

import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';

import { SharedModule } from '@shared/shared.module';

import { AccountComponent } from './account.component';
import { TenantChangeComponent } from './tenant/tenant-change.component';
import { TenantChangeModalComponent } from './tenant/tenant-change-modal.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountLanguagesComponent } from './account-languages/account-languages.component';

import { LoginService } from './login/login.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { AbpModule, UtilsService, MessageService, TokenService, LogService } from 'abp-ng2-module';
import { ResetPasswordComponent } from './passwords/reset-password.component';
import { TenantRegisterComponent } from './tenant-register/tenant-register.component';
import { CaptchaComponent } from './component/captcha/captcha.component';
import { HttpClientModule } from '@angular/common/http';
import { LoginCallbackComponent } from './login-callback/login-callback.component';
import { ActivationComponent } from './activation/activation.component';
import { TokenAuthServiceProxy } from '@shared/service-proxies';
import { Router } from '@angular/router';
import { SocialLoginModule, AuthServiceConfig, AuthService } from 'angularx-social-login';
import { environment } from '@env/environment';


@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    HttpClientModule,
    SharedModule,
    AbpModule,
    SocialLoginModule,
    AccountRoutingModule,
  ],
  declarations: [
    AccountComponent,
    TenantChangeComponent,
    TenantChangeModalComponent,
    LoginComponent,
    LoginCallbackComponent,
    RegisterComponent,
    AccountLanguagesComponent,
    ResetPasswordComponent,
    ForgotPasswordComponent,
    TenantRegisterComponent,
    CaptchaComponent,
    ActivationComponent,
  ],
  providers: [
    {
      provide: LoginService,
      useFactory: (
        tokenAuthService: TokenAuthServiceProxy,
        router: Router,
        zone: NgZone,
        utilsService: UtilsService,
        messageService: MessageService,
        tokenService: TokenService,
        logService: LogService,
        modalService: NzModalService,
      ) =>
        new LoginService(
          tokenAuthService,
          router,
          zone,
          utilsService,
          messageService,
          tokenService,
          logService,
          modalService,
        ),
      deps: [
        TokenAuthServiceProxy,
        Router,
        NgZone,
        UtilsService,
        MessageService,
        TokenService,
        LogService,
        NzModalService,
      ],
    },
  ],
  entryComponents: [TenantChangeModalComponent],
})
export class AccountModule {
}
