import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountComponent } from './account.component';
import { ForgotPasswordComponent } from './passwords/forgot-password.component';
import { ResetPasswordComponent } from './passwords/reset-password.component';
import { TenantRegisterComponent } from './tenant-register/tenant-register.component';
import { LoginCallbackComponent } from './login-callback/login-callback.component';
import { ActivationComponent } from './activation/activation.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '',
        component: AccountComponent,

        children: [
          { path: 'login', component: LoginComponent },
          { path: 'login-callback', component: LoginCallbackComponent },
          { path: 'register', component: RegisterComponent },
          { path: 'forgot-password', component: ForgotPasswordComponent },
          { path: 'reset-password', component: ResetPasswordComponent },
          { path: 'activation', component: ActivationComponent },
          { path: 'tenant-register', component: TenantRegisterComponent },
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class AccountRoutingModule {
}
