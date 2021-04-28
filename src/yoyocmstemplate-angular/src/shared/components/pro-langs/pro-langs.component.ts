import { Component, Inject, Input, OnInit, Injector } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { SettingsService } from '@delon/theme';
import { AppComponentBase } from '@shared/component-base';
import { UtilsService } from 'abp-ng2-module';
import { UserServiceProxy, ChangeUserLanguageDto, ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';

@Component({
  selector: 'app-pro-langs',
  templateUrl: './pro-langs.component.html',
  styles: [],
})
export class ProLangsComponent extends AppComponentBase implements OnInit {
  languages: abp.localization.ILanguageInfo[];
  currentLanguage: abp.localization.ILanguageInfo;

  @Input() placement = 'bottomRight';
  @Input() btnClass = 'alain-pro__header-item';
  @Input() btnIconClass = 'alain-pro__header-item-icon';

  constructor(injector: Injector, private utilsService: UtilsService) {
    super(injector);
  }

  ngOnInit() {
    this.languages = _.filter(this.localization.languages, l => !l.isDisabled);
    this.currentLanguage = this.localization.currentLanguage;
  }

  changeLanguage(languageName: string): void {
    this.utilsService.setCookieValue(
      'Abp.Localization.CultureName',
      languageName,
      new Date(new Date().getTime() + 5 * 365 * 86400000), // 5 year
      abp.appPath,
    );

    location.reload();
  }
}
