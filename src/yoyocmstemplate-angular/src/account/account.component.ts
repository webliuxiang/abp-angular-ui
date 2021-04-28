import {
  Component,
  ViewContainerRef,
  OnInit,
  ViewEncapsulation,
  Injector,
} from '@angular/core';
import { LoginService } from './login/login.service';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import { AbpSessionService } from 'abp-ng2-module';
import { SessionServiceProxy } from '@shared/service-proxies';

@Component({
  selector: 'layout-account',

  templateUrl: './account.component.html',
  styleUrls: ['./account.component.less'],
})
export class AccountComponent extends AppComponentBase {
  versionText: string;
  currentYear: number;
  links = [
    {
      title: 'ABP',
      href: '',
    },
    {
      title: 'Privacy',
      href: '',
    },
    {
      title: 'Clause',
      href: '',
    },
  ];

  accountLeftStyle = {
    'background-image': 'url("assets/images/img-big/bg-1.jpeg")',
  };

  constructor(
    injector: Injector,
    private _abpSessionService: AbpSessionService,
    private _sessionAppService: SessionServiceProxy,
  ) {
    super(injector);
    this.currentYear = new Date().getFullYear();
    this.versionText =
      this.appSession.application.version + ' [' + this.appSession.application.releaseDate.format('YYYYMMDD') + ']';
  }
}
