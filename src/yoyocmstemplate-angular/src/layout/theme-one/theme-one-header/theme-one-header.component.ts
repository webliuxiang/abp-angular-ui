import { Component, OnInit } from '@angular/core';
import { SettingsService } from '@delon/theme';

@Component({
  selector: 'theme-one-header',
  templateUrl: './theme-one-header.component.html',
  styleUrls: ['./theme-one-header.component.less']
})
export class ThemeOneHeaderComponent {

  constructor(public settings: SettingsService) {
  }
}
