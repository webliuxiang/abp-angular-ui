import { Component } from '@angular/core';
import { SettingService } from 'abp-ng2-module';
import { AppConsts } from '../abpPro/AppConsts';

@Component({
  selector: 'app-component',
  templateUrl: './app.component.html',
  styles: [],
})
export class AppComponent {

  layoutValue = AppConsts.settingValues.theme.layout;

  get themeLayout(): string {
    return this.settings.get(AppConsts.settings.theme.layout);
  }

  constructor(
    private settings: SettingService,
  ) {

  }
}
