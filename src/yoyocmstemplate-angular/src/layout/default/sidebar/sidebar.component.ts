import { Component, OnInit, Injector } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { SettingsService } from '@delon/theme';
import { AppComponentBase } from '@shared/component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.less'],
})
export class SidebarComponent extends AppComponentBase implements OnInit {
  constructor(
    injector: Injector,
    public settings: SettingsService,
  ) {
    super(injector);
  }

  ngOnInit() {

  }

}
