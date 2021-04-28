import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Nav } from '@delon/abc/sidebar-nav/sidebar-nav.types';
import { MenuService, SettingsService } from '@delon/theme';
import { ReuseTabService } from '@delon/abc';
import { Router } from '@angular/router';
import { YoYoSidebarNavComponent } from '@layout/components';

@Component({
  selector: 'theme-one-sidebar',
  templateUrl: './theme-one-sidebar.component.html',
  styleUrls: ['./theme-one-sidebar.component.less'],
  host: {
    '[class.alain-pro__side-nav]': 'true',
  },
  changeDetection: ChangeDetectionStrategy.Default,
  preserveWhitespaces: false,
})
export class ThemeOneSidebarComponent extends YoYoSidebarNavComponent {

  constructor(
    public menuService: MenuService,
    public settings: SettingsService,
    public reuseTabService: ReuseTabService,
    public cd: ChangeDetectorRef,
    public router: Router,
  ) {
    super(menuService, settings, reuseTabService, cd, router);
  }


}
