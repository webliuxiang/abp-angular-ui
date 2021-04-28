import { Component, OnInit, Injector } from '@angular/core';
import {
  CreateTenantInput,
  TenantServiceProxy,
  SubscribableEditionComboboxItemDto,
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';

@Component({
  selector: 'app-create-tenant',
  templateUrl: './create-tenant.component.html',
  styles: [],
})
export class CreateTenantComponent extends ModalComponentBase
  implements OnInit {

  confirmPassword = '';
  useDefaultPassword = true;
  useHostDatabase = true;
  editionId: any = null;
  isUnlimited = false; // 是否无限期订阅
  model: CreateTenantInput = new CreateTenantInput();
  edition: SubscribableEditionComboboxItemDto;

  constructor(injector: Injector, private tenantService: TenantServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    this.model.init({ isActive: true });
  }


  save(): void {
    this.saving = true;
    // 版本
    this.model.editionId = this.edition ? parseInt(this.edition.value, ) : null;
    // 如果没有选择版本或勾选了无限期使用，订阅时间设置为null
    this.model.subscriptionEndUtc = (!this.model.editionId || this.isUnlimited) ? null : this.model.subscriptionEndUtc;

    console.log(this.model);
    this.tenantService
      .create(this.model)
      .finally(() => {
        this.saving = false;
      })
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success();
      });
  }

  selectedEditionChange(edtion: SubscribableEditionComboboxItemDto) {
    this.edition = edtion;
    this.model.isInTrialPeriod = false;
  }
}
