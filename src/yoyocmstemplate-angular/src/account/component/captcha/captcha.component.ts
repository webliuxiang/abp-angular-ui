import { Component, OnInit, Input, forwardRef, SimpleChange, SimpleChanges, Injector } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { AppConsts } from 'abpPro/AppConsts';
import { AppSessionService } from '@shared/session/app-session.service';
import { ControlComponentBase } from '@shared/component-base';
import { SettingService } from 'abp-ng2-module';
import { AppCaptchaType } from '../../../abpPro/AppEnums';

@Component({
  selector: 'yoyo-captcha',
  templateUrl: './captcha.component.html',
  styleUrls: ['./captcha.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CaptchaComponent),
      multi: true,
    },
  ],
})
export class CaptchaComponent extends ControlComponentBase<string> {
  /** 验证码key */
  @Input() key: string;

  /** change key */
  @Input() timestamp: string;

  /** 验证码类型 */
  @Input() captchaType: number;

  /** 拥有焦点 */
  focused: boolean;

  /** 验证码长度 */
  captchaLength = 0;

  /** 验证码图片实际地址 */
  captchaUrl: string;

  placeholder = this.l('CAPTCHA');

  /** 最后使用的验证码键值 */
  lastKey = '';

  constructor(
    injector: Injector,
    private setting: SettingService,
    private appSession: AppSessionService,
  ) {
    super(injector);
  }

  /** 初始化 */
  onInit(): void {

  }

  /** 视图初始化完成 */
  onAfterViewInit(): void {

  }

  /** 输入参数发生改变 */
  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
    if (changes.key && changes.key.currentValue && changes.key.currentValue.trim() !== '') {
      this.initImg();
    }
    if (changes.captchaType && changes.captchaType.currentValue) {
      switch (changes.captchaType.currentValue) {
        case AppCaptchaType.HostTenantRegister:
          this.captchaLength = this.setting.getInt(AppConsts.settings.host.captchaOnTenantRegistrationLength);
          break;
        case AppCaptchaType.TenantUserRegister:
          this.captchaLength = this.setting.getInt(AppConsts.settings.captchaOnUserRegistrationLength);
          break;
        case AppCaptchaType.HostUserLogin:
        case AppCaptchaType.TenantUserLogin:
          this.captchaLength = this.setting.getInt(AppConsts.settings.captchaOnUserLoginLength);
          break;
      }

      this.initImg();
    }
    if (changes.timestamp && changes.timestamp.currentValue) {
      this.updateImage();
    }
  }

  /** 释放资源 */
  onDestroy(): void {

  }

  onInputFocusChange(val: boolean) {
    this.focused = val;

    if (this.lastKey === '') {
      this.initImg();
    } else if (this.lastKey !== this.key) {
      this.updateImage();
    }
  }

  onImageClick() {
    this.updateImage();
  }

  /**
   * 初始化验证码图片
   */
  private initImg(): void {
    if (!this.key || this.key === ''
      || this.captchaUrl || !this.focused
      || !this.captchaType || this.captchaLength === 0) {
      return;
    }
    this.updateImage();
  }

  /**
   * 清空图片
   */
  private updateImage(): void {
    if (!this.key || this.key === '') {
      // 未输入验证码key
      return;
    }
    if (this.lastKey.trim() !== '') {
      this.emitValueChange('');
    }

    this.lastKey = this.key;

    let tid: any = this.appSession.tenantId;
    if (!tid) {
      tid = '';
    }

    const timestamp = new Date().getTime();
    const queryParams = `name=${this.key}&t=${this.captchaType}&l=${this.captchaLength}&tid=${tid}&timestamp=${timestamp}`;
    this.captchaUrl = `${AppConsts.remoteServiceBaseUrl}/api/TokenAuth/GenerateVerification?${queryParams}`;
  }

}
