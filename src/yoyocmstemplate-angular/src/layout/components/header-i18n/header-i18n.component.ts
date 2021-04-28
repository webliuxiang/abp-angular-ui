import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { ChangeUserLanguageDto, ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';

@Component({
  selector: 'header-i18n',
  templateUrl: './header-i18n.component.html',
  styles: [],
})
export class HeaderI18nComponent extends AppComponentBase implements OnInit {
  languages: abp.localization.ILanguageInfo[];
  currentLanguage: abp.localization.ILanguageInfo;

  constructor(injector: Injector, private _profileService: ProfileServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    this.languages = _.filter(this.localization.languages, l => !l.isDisabled);
    this.currentLanguage = this.localization.currentLanguage;
  }

  change(languageName: string): void {
    const input = new ChangeUserLanguageDto();
    input.languageName = languageName;

    this._profileService.changeLanguage(input).subscribe(() => {
      abp.utils.setCookieValue(
        'Abp.Localization.CultureName',
        languageName,
        new Date(new Date().getTime() + 5 * 365 * 86400000), // 5 year
        abp.appPath,
      );

      window.location.reload();
    });
  }
}
