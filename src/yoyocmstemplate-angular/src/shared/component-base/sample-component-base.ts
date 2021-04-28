import { Injector, Input, Directive } from '@angular/core';
import { LocalizationService } from '@shared/i18n/localization.service';
import { AppConsts } from 'abpPro/AppConsts';
import { PermissionService } from '@shared/auth';
import { ALAIN_I18N_TOKEN } from '@delon/theme';

@Directive()
export abstract class SampleComponentBase {

  @Input() loading: boolean;

  localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;

  localization: LocalizationService;
  permission: PermissionService;


  constructor(
    public injector: Injector,
  ) {
    this.localization = injector.get<LocalizationService>(ALAIN_I18N_TOKEN);
    this.permission = injector.get(PermissionService);
  }

  l(key: string, ...args: any[]): string {
    let localizedText = this.localization.localization(key, this.localizationSourceName);

    if (!localizedText) {
      localizedText = key;
    }

    if (!args || !args.length) {
      return localizedText;
    }

    return this.localization.formatString(localizedText, args);
  }


  isGranted(permissionName: string | string[]): boolean {
    if (!permissionName) {
      return true;
    }

    return this.permission.isGranted(permissionName);
  }
}
