import { finalize } from 'rxjs/operators';
import { Component, OnInit, Injector } from '@angular/core';
import {
  OrderServiceProxy,
  OrderStatisticsDto
} from '@shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base';

@Component({
  selector: 'app-product-order-statistics',
  templateUrl: './product-order-statistics.component.html',
  styles: []
})
export class ProductOrderStatisticsComponent extends ModalComponentBase
  implements OnInit {
  entities: OrderStatisticsDto[];

  constructor(injector: Injector, private _orderService: OrderServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    this._orderService.getProductOrderStatistics().subscribe(result => {
      this.entities = result.items;
    });
  }
}
