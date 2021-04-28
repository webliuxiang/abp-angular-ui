import {
  Component,
  OnInit,
  ElementRef,
  Renderer2,
  NgZone
} from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzMessageService } from 'ng-zorro-antd/message';
import { VERSION as VERSION_ZORRO } from 'ng-zorro-antd/version';
import { TitleService, VERSION as VERSION_ALAIN } from '@delon/theme';

import { preloaderFinished } from '@delon/theme';
import { SignalRAspNetCoreHelper } from '@shared/helpers/SignalRAspNetCoreHelper';
preloaderFinished();

@Component({
  selector: 'app-root',
  template: `
    <router-outlet></router-outlet>
  `
})
export class RootComponent implements OnInit {
  constructor(
    private _modalService: NzModalService,
    private _messageService: NzMessageService,
    private _notifyService: NzNotificationService,
    el: ElementRef,
    private renderer: Renderer2,
    private router: Router,
    private titleSrv: TitleService
  ) {
    renderer.setAttribute(
      el.nativeElement,
      'ng-alain-version',
      VERSION_ALAIN.full
    );
    renderer.setAttribute(
      el.nativeElement,
      'ng-zorro-version',
      VERSION_ZORRO.full
    );
  }

  ngOnInit(): void {
    //  设置标题
    this.titleSrv.prefix = '52ABP';

    this.router.events
      .pipe(filter(evt => evt instanceof NavigationEnd))
      .subscribe(() => {
        this.titleSrv.setTitle();
        this._modalService.closeAll();
      });
  }
}
