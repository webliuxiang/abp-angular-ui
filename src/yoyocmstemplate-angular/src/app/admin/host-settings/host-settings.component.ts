import { Component, Injector, OnInit } from '@angular/core';
import {
  ComboboxItemDto,
  CommonLookupServiceProxy,
  SettingScopes,
  HostSettingsEditDto,
  HostSettingsServiceProxy,
  SendTestEmailInput,
  SubscribableEditionComboboxItemDto,
  ComboboxItemDtoTOfInt32
} from '@shared/service-proxies/service-proxies';
import { AppTimezoneScope, AppCaptchaType } from 'abpPro/AppEnums';
import { AppComponentBase } from '@shared/component-base';
import { finalize } from 'rxjs/operators';
import { zip } from 'rxjs';
import { AppConsts } from '../../../abpPro/AppConsts';

@Component({
  templateUrl: './host-settings.component.html'
})
export class HostSettingsComponent extends AppComponentBase implements OnInit {
  layoutValue = AppConsts.settingValues.theme.layout;

  hostSettings: HostSettingsEditDto = undefined;
  editions: SubscribableEditionComboboxItemDto[] = undefined;
  testEmailAddress: string = undefined;
  showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;
  defaultTimezoneScope: SettingScopes = AppTimezoneScope.Application;

  usingDefaultTimeZone = false;
  initialTimeZone: string = undefined;

  selectedEditionId = '';

  sendMailTest: boolean;

  validateCodeTypes: ComboboxItemDtoTOfInt32[] = [];

  isMultiTenancyEnabled: boolean = this.multiTenancy.isEnabled;

  constructor(
    injector: Injector,
    private _hostSettingService: HostSettingsServiceProxy,
    private _commonLookupService: CommonLookupServiceProxy
  ) {
    super(injector);
  }

  loadDatas(): void {
    zip(
      this._commonLookupService.getEditionsForCombobox(false),
      this._commonLookupService.getValidateCodeTypesForCombobox(),
      this._hostSettingService.getAllSettings()
    )
      .pipe(finalize(() => {}))
      .subscribe(([editionResult, validateCodeTypesResult, settingsResult]) => {
        //
        this.editions = editionResult.items;
        //
        this.validateCodeTypes = validateCodeTypesResult.items;
        //
        this.hostSettings = settingsResult;
        this.initialTimeZone = settingsResult.general.timezone;
        this.usingDefaultTimeZone =
          settingsResult.general.timezoneForComparison ===
          this.setting.get('Abp.Timing.TimeZone');
        if (this.hostSettings.tenantManagement.defaultEditionId) {
          this.selectedEditionId =
            this.hostSettings.tenantManagement.defaultEditionId + '';
        }
      });
  }

  init(): void {
    this.testEmailAddress = this.appSession.user.emailAddress;
    this.showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;
    this.loadDatas();
  }

  ngOnInit(): void {
    this.init();
  }

  sendTestEmail(): void {
    this.sendMailTest = true;
    const input = new SendTestEmailInput();
    input.emailAddress = this.testEmailAddress;
    this._hostSettingService
      .sendTestEmail(input)
      .pipe(
        finalize(() => {
          this.sendMailTest = false;
        })
      )
      .subscribe(result => {
        this.notify.success(this.l('TestEmailSentSuccessfully'));
      });
  }

  saveAll(): void {
    this.hostSettings.tenantManagement.defaultEditionId = parseInt(
      this.selectedEditionId
    );
    this._hostSettingService
      .updateAllSettings(this.hostSettings)
      .subscribe(result => {
        this.notify.success(this.l('SavedSuccessfully'));

        if (
          abp.clock.provider.supportsMultipleTimezone &&
          this.usingDefaultTimeZone &&
          this.initialTimeZone !== this.hostSettings.general.timezone
        ) {
          this.message
            .success(this.l('TimeZoneSettingChangedRefreshPageNotification'))
            .then(() => {
              window.location.reload();
            });
        }
      });
  }
}
