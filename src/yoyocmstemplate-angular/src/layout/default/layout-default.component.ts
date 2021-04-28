import {
  Component,
  ViewChild,
  ComponentFactoryResolver,
  ViewContainerRef,
  AfterViewInit,
  OnInit,
  OnDestroy,
  ElementRef,
  Renderer2,
  Inject,
  Injector,
  NgZone,
  KeyValueDiffers,
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { Router, NavigationEnd, RouteConfigLoadStart, NavigationError, NavigationCancel } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzIconService } from 'ng-zorro-antd/icon';
import { Subscription, Observable, BehaviorSubject } from 'rxjs';
import { updateHostClass } from '@delon/util';
import { ScrollService, MenuService, SettingsService } from '@delon/theme';

// #region icons

import {
  MenuFoldOutline,
  MenuUnfoldOutline,
  SearchOutline,
  SettingOutline,
  FullscreenOutline,
  FullscreenExitOutline,
  BellOutline,
  LockOutline,
  PlusOutline,
  UserOutline,
  LogoutOutline,
  EllipsisOutline,
  GlobalOutline,
  ArrowDownOutline,
  // Optional
  GithubOutline,
  AppstoreOutline,
} from '@ant-design/icons-angular/icons';

const ICONS = [
  MenuFoldOutline,
  MenuUnfoldOutline,
  SearchOutline,
  SettingOutline,
  FullscreenOutline,
  FullscreenExitOutline,
  BellOutline,
  LockOutline,
  PlusOutline,
  UserOutline,
  LogoutOutline,
  EllipsisOutline,
  GlobalOutline,
  ArrowDownOutline,
  // Optional
  GithubOutline,
  AppstoreOutline,
];

// #endregion

import { environment } from '@env/environment';
import { AppComponentBase } from '@shared/component-base';

import { SignalRAspNetCoreHelper } from '@shared/helpers/SignalRAspNetCoreHelper';
import { AppMenus } from 'abpPro/AppMenus';
import { AppMainMenus } from 'abpPro/AppMainMenus';
import { AbpProMenusHelper } from '@shared/helpers/AbpProMenusHelper';
import { AppSessionService } from '@shared/session/app-session.service';
import { AbpSignalrService } from '@shared/auth/abp-signalr.service';

@Component({
  selector: 'layout-default',
  templateUrl: './layout-default.component.html',
  preserveWhitespaces: false,
  host: {
    '[class.alain-pro]': 'true',
  },
})
export class LayoutDefaultComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {
  private notify$: Subscription;

  quickChatVisible = true;

  isFetching = false;
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
    public abpSignal: AbpSignalrService,
  ) {
    super(injector);
    iconSrv.addIcon(...ICONS);
    // scroll to top in change page
    router.events.subscribe(evt => {
      if (!this.isFetching && evt instanceof RouteConfigLoadStart) {
        this.isFetching = true;
      }
      if (evt instanceof NavigationError || evt instanceof NavigationCancel) {
        this.isFetching = false;
        if (evt instanceof NavigationError) {
          this.message.error(`无法加载${evt.url}路由`);
        }
        return;
      }
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      setTimeout(() => {
        scroll.scrollToTop();
        this.isFetching = false;
      }, 100);
    });

    const menuItems = AbpProMenusHelper.getMenuItems([...AppMenus.Menus, ...AppMainMenus.Menus]);
    this.menuSrv.add(menuItems);
  }

  get collapsed(): boolean {
    return this.settings.layout.collapsed;
  }


  ngAfterViewInit(): void {}

  ngOnInit() {
    this.notify$ = this.settings.notify.subscribe(_ => this.setClass());
    this.setClass();

    // 初始化SignalR连接
    this.initiSignalR();

    if (this.appSession.user) {
      console.log('11111111111111111');
      this.abpSignal.init();
    }
  }

  ngOnDestroy() {
    this.notify$.unsubscribe();
  }

  initiSignalR() {
    // 连接到signalR
    SignalRAspNetCoreHelper.initSignalR(() => {});
  }


  protected setClass() {
    const { el, renderer, settings } = this;
    const layout = settings.layout;

    updateHostClass(
      el.nativeElement,
      renderer,
      {
        ['alain-pro']: true,
        // [`alain-pro__fixed`]: layout.fixed,
        [`alain-pro__boxed`]: layout.boxed,
        [`alain-pro__collapsed`]: layout.collapsed,
      },
      true,
    );

    this.doc.body.classList[layout.colorWeak ? 'add' : 'remove']('color-weak');
    this.doc.body.classList[layout.theme === undefined || layout.theme === 'light' ? 'add' : 'remove'](
      'alain-pro__light',
    );
    this.doc.body.classList[layout.theme === 'dark' ? 'add' : 'remove']('alain-pro__dark');
  }
}
