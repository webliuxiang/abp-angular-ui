import { Component, Injector, OnInit } from '@angular/core';
import { AppConsts } from 'abpPro/AppConsts';
import {
  SendTestEmailInput,
  TenantSettingsEditDto,
  TenantSettingsServiceProxy,
  SettingScopes,
  ComboboxItemDtoTOfInt32,
  CommonLookupServiceProxy
} from '@shared/service-proxies/service-proxies';
import { AppTimezoneScope } from 'abpPro/AppEnums';
import { AppComponentBase } from '@shared/component-base';
import { zip } from 'rxjs';

@Component({
  templateUrl: './tenant-settings.component.html'
})
export class TenantSettingsComponent extends AppComponentBase
  implements OnInit {
  layoutValue = AppConsts.settingValues.theme.layout;

  usingDefaultTimeZone = false;
  initialTimeZone: string = null;
  testEmailAddress: string = undefined;

  isMultiTenancyEnabled: boolean = this.multiTenancy.isEnabled;
  showTimezoneSelection: boolean = abp.clock.provider.supportsMultipleTimezone;

  settings: TenantSettingsEditDto = undefined;

  remoteServiceBaseUrl = AppConsts.remoteServiceBaseUrl;
  defaultTimezoneScope: SettingScopes = SettingScopes.Tenant;
  validateCodeTypes: ComboboxItemDtoTOfInt32[] = [];

  constructor(
    injector: Injector,
    private _tenantSettingsService: TenantSettingsServiceProxy,
    private _commonLookupService: CommonLookupServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.testEmailAddress = this.appSession.user.emailAddress;
    this.getSettings();
  }

  getSettings(): void {
    zip(
      this._commonLookupService.getValidateCodeTypesForCombobox(),
      this._tenantSettingsService.getAllSettings()
    ).subscribe(([validateCodeTypesResult, settingsResult]) => {
      //
      this.validateCodeTypes = validateCodeTypesResult.items;
      //
      this.settings = settingsResult;
      if (this.settings.general) {
        this.initialTimeZone = this.settings.general.timezone;
        this.usingDefaultTimeZone =
          this.settings.general.timezoneForComparison ===
          abp.setting.values['Abp.Timing.TimeZone'];
      }
    });
  }

  saveAll(): void {
    this._tenantSettingsService
      .updateAllSettings(this.settings)
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));

        if (
          abp.clock.provider.supportsMultipleTimezone &&
          this.usingDefaultTimeZone &&
          this.initialTimeZone !== this.settings.general.timezone
        ) {
          this.message
            .info(this.l('TimeZoneSettingChangedRefreshPageNotification'))
            .then(() => {
              window.location.reload();
            });
        }
      });
  }

  sendTestEmail(): void {
    const input = new SendTestEmailInput();
    input.emailAddress = this.testEmailAddress;
    this._tenantSettingsService.sendTestEmail(input).subscribe(result => {
      this.notify.success(this.l('TestEmailSentSuccessfully'));
    });
  }
}
