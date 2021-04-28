
import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { CreateOrUpdateProductInput, ProductEditDto, ProductServiceProxy, IKeyValuePairOfStringString } from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { zip } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-product',
  templateUrl: './create-or-edit-product.component.html',
  styleUrls: ['create-or-edit-product.component.less'],
})
export class CreateOrEditProductComponent extends ModalComponentBase implements OnInit {
  /**
   * 编辑时DTO的id
   */
  id: any;

  entity: ProductEditDto = new ProductEditDto();
  types: IKeyValuePairOfStringString[] = [];

  /**
   * 初始化的构造函数
   */
  constructor(injector: Injector, private _productService: ProductServiceProxy) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
  }

  /**
   * 初始化方法
   */
  init(): void {
    this._productService.getForEdit(this.id).subscribe(result => {
      this.entity = result.product;
      this.types = result.types;
    });
  }

  getProductTypeLocStr(label: string): string {
    return this.l(`ProductType${label}`);
  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    const input = new CreateOrUpdateProductInput();
    input.product = this.entity;

    this.saving = true;

    this._productService
      .createOrUpdate(input)
      .finally(() => (this.saving = false))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
  }
}
