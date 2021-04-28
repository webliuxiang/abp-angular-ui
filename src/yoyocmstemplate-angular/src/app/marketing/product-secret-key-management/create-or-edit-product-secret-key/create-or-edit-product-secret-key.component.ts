
import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { ProductSecretKeyServiceProxy, BatchCreateProductSecretKeyInput, ProductServiceProxy, KeyValuePairOfStringString } from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-product-secret-key',
  templateUrl: './create-or-edit-product-secret-key.component.html',
  styleUrls: ['create-or-edit-product-secret-key.component.less'],
})
export class CreateOrEditProductSecretKeyComponent extends ModalComponentBase implements OnInit {
  products: KeyValuePairOfStringString[];
  entity: BatchCreateProductSecretKeyInput = new BatchCreateProductSecretKeyInput();

  /**
   * 初始化的构造函数
   */
  constructor(
    injector: Injector,
    private _productSecretKeyService: ProductSecretKeyServiceProxy,
    private _productService: ProductServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
  }

  /**
   * 初始化方法
   */
  init(): void {
    this.entity.quantity = 1;
    this._productService.getProducts().subscribe(result => {
      this.products = result;
    });
  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    this.saving = true;
    const self = this;

    setTimeout(() => {
      self._productSecretKeyService
        .batchCreate(self.entity)
        .pipe(finalize(() => (self.saving = false)))
        .subscribe(() => {
          self.notify.success(self.l('SavedSuccessfully'));
          self.success(true);
        });
    }, 50);
  }
}
