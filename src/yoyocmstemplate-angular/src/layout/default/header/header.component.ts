import { Component, Injector } from '@angular/core';
import { SettingsService } from '@delon/theme';
import { SampleComponentBase } from '@shared/component-base';

@Component({
  selector: 'layout-header',
  templateUrl: './header.component.html',
})
export class HeaderComponent extends SampleComponentBase {
  searchToggleStatus: boolean;
  constructor(injector: Injector, public settings: SettingsService) {
    super(injector);
  }

  toggleCollapsedSidebar() {
    this.settings.setLayout('collapsed', !this.settings.layout.collapsed);
  }

  searchToggleChange() {
    this.searchToggleStatus = !this.searchToggleStatus;
  }
}
