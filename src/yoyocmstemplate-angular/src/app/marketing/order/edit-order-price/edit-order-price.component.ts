import { Component, Injector, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base';
import { EntityDtoOfGuid, OrderEditPriceDto, OrderListDto, OrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-edit-order-price',
  templateUrl: './edit-order-price.component.html',
  styles: [],
})
export class EditOrderPriceComponent extends ModalComponentBase
  implements OnInit {

  loading: boolean;

  order: OrderListDto;

  actualPayment: number;

  constructor(
    injector: Injector,
    private orderSer: OrderServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.actualPayment = this.order.actualPayment;
  }


  submitForm() {
    this.loading = true;
    const input = new OrderEditPriceDto({
      id: this.order.id,
      actualPayment: this.order.actualPayment,
    });
    this.orderSer.updateOraderPrice(input)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        this.notify.success(this.l('SuccessfullyOperation'));
        this.success();
      });
  }
}
