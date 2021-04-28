import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  CreateOrUpdateBannerAdInput,
  BannerAdEditDto,
  KeyValuePairOfStringString,
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { BannerImgServiceProxy } from '../../../../shared/service-proxies/service-proxies';

@Component({
  selector: 'create-or-edit-banner-ad',
  templateUrl: './create-or-edit-banner-ad.component.html',
  styleUrls: ['create-or-edit-banner-ad.component.less'],
})
export class CreateOrEditBannerAdComponent extends ModalComponentBase implements OnInit {
  /**
   * 编辑时DTO的id
   */
  id: any;

  entity: BannerAdEditDto = new BannerAdEditDto();

  /**
   * 构造函数，在此处配置依赖注入
   */
  constructor(injector: Injector, private _bannerAdService: BannerImgServiceProxy) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
  }

  /**
   * 初始化方法
   */
  init(): void {
    this._bannerAdService.getForEdit(this.id).subscribe(result => {
      this.entity = result.bannerAd;
    });
  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    const input = new CreateOrUpdateBannerAdInput();
    input.bannerAd = this.entity;

    this.saving = true;

    this._bannerAdService
      .createOrUpdate(input)
      .pipe(finalize(() => (this.saving = false)))

      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
  }

  _change(event, type) {
    switch (type) {
      case 'imageUrl':
        this.entity.imageUrl = event.path;
        break;
      case 'thumbImgUrl':
        this.entity.thumbImgUrl = event.path;
        break;
    }
  }
}
