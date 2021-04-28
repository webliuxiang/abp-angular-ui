import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  CreateOrUpdateWechatAppConfigInput,
  WechatAppConfigEditDto,
  WechatAppConfigServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-wechat-app-config',
  templateUrl: './create-or-edit-wechat-app-config.component.html',
  styleUrls: ['create-or-edit-wechat-app-config.component.less'],
})
export class CreateOrEditWechatAppConfigComponent extends ModalComponentBase implements OnInit {
  /**
   * 编辑时DTO的id
   */
  id: any;

  entity: WechatAppConfigEditDto = new WechatAppConfigEditDto();

  wechatAppTypeList: any[] = [];

  /**
   * 初始化的构造函数
   */
  constructor(injector: Injector, private _wechatAppConfigService: WechatAppConfigServiceProxy) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
  }

  /**
   * 初始化方法
   */
  init(): void {
    this._wechatAppConfigService.getForEdit(this.id).subscribe(result => {
      this.entity = result.wechatAppConfig;
      this.wechatAppTypeList = result.wechatAppTypeList;
      if (!this.id) {
        this.entity.appType = this.wechatAppTypeList[0].value;
      }
    });
  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    const input = new CreateOrUpdateWechatAppConfigInput();
    input.wechatAppConfig = this.entity;

    this.saving = true;

    this._wechatAppConfigService
      .createOrUpdate(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
  }
}
