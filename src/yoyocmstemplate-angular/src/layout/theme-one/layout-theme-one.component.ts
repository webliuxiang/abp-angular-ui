import { Component, ElementRef, Inject, Injector, NgZone, OnInit, Renderer2 } from '@angular/core';
import { LayoutDefaultComponent } from '../default';
import { NzIconService } from 'ng-zorro-antd/icon';
import { Router } from '@angular/router';
import { MenuService, ScrollService, SettingsService } from '@delon/theme';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'layout-theme-one',
  templateUrl: './layout-theme-one.component.html',
  styleUrls: ['./layout-theme-one.component.less'],
})
export class LayoutThemeOneComponent extends LayoutDefaultComponent {

  constructor(
    injector: Injector,
    iconSrv: NzIconService,
    router: Router,
    scroll: ScrollService,
    public menuSrv: MenuService,
    public settings: SettingsService,
    public el: ElementRef,
    public renderer: Renderer2,
    @Inject(DOCUMENT) public doc: any,
    public _zone: NgZone,
  ) {
    super(injector, iconSrv, router, scroll, menuSrv, settings, el, renderer, doc, _zone);
  }

}
