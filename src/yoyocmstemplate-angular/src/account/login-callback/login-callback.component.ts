import { Component, OnInit, Injector, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoginService } from '../login/login.service';
import { AppComponentBase } from '@shared/component-base';
import { UrlHelper } from '@shared/helpers/UrlHelper';

@Component({
  selector: 'app-login-callback',
  templateUrl: './login-callback.component.html',
  styles: [],
})
export class LoginCallbackComponent extends AppComponentBase implements OnInit {

  constructor(private _route: ActivatedRoute,
              injector: Injector,
              private _loginService: LoginService) {
    super(injector);
  }

  ngOnInit() {
    const providerName = UrlHelper.getQueryParameters().providerName;
    if (providerName) {
      // this._loginService.initExternalLoginProviders(() => {
      //   const selectedProvider = this._loginService.externalLoginProviders.find(x => x.name === providerName);
      //   this._loginService.externalAuthenticate(selectedProvider);
      // });
    }
  }
}
