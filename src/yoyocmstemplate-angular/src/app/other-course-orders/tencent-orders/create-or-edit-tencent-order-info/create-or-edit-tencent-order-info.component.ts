
import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { CreateOrUpdateTencentOrderInfoInput, TencentOrderInfoEditDto, TencentOrderInfoServiceProxy } from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';

@Component({
  selector: 'create-or-edit-tencent-order-info',
  templateUrl: './create-or-edit-tencent-order-info.component.html',
  styleUrls: [
	'create-or-edit-tencent-order-info.component.less'
  ],
})

export class CreateOrEditTencentOrderInfoComponent
  extends ModalComponentBase
    implements OnInit {
    /**
    * 编辑时DTO的id
    */
    id: any ;

	  entity: TencentOrderInfoEditDto = new TencentOrderInfoEditDto();

    /**
    * 初始化的构造函数
    */
    constructor(
		injector: Injector,
		private _tencentOrderInfoService: TencentOrderInfoServiceProxy
	) {
		super(injector);
    }

    ngOnInit(): void{
		this.init();
    }


    /**
    * 初始化方法
    */
    init(): void {
		this._tencentOrderInfoService.getForEdit(this.id).subscribe(result => {
			this.entity = result.tencentOrderInfo;
		});
    }

    /**
    * 保存方法,提交form表单
    */
    submitForm(): void {
		const input = new CreateOrUpdateTencentOrderInfoInput();
		input.tencentOrderInfo = this.entity;

		this.saving = true;

		this._tencentOrderInfoService.createOrUpdate(input)
		.finally(() => (this.saving = false))
		.subscribe(() => {
			this.notify.success(this.l('SavedSuccessfully'));
			this.success(true);
		});
    }
}
