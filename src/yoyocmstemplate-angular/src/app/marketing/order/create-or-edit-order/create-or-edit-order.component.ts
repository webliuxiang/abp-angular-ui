import { Component, Injector, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base';
import { EntityDtoOfGuid, OrderEditPriceDto, OrderListDto, OrderServiceProxy, OrderEditDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-create-or-edit-order',
  templateUrl: './create-or-edit-order.component.html',
  styles: [
  ]
})
export class CreateOrEditOrderComponent extends ModalComponentBase
  implements OnInit {

  loading: boolean;

  order: OrderListDto;
  /** 订单状态 */
  orderStatusEnum = {
    type: 'default',
    dataSource: 'enum',
    enumType: 'OrderStatusEnum',
    allowClear: false,
  };

  orderSourceType = {
    type: 'default',
    dataSource: 'enum',
    enumType: 'OrderSourceType',
    allowClear: false,
  };



  constructor(
    injector: Injector,
    private orderSer: OrderServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit() {
  }


  submitForm() {
    this.loading = true;

    const input = new OrderEditDto();

    input.id = this.order.id;
    input.status = this.order.status;
    input.productIndate = this.order.productIndate;
    input.orderSourceType = this.order.orderSourceType;

    this.orderSer.updateOrder(input)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        this.notify.success(this.l('SuccessfullyOperation'));
        this.success();
      });
  }
}
