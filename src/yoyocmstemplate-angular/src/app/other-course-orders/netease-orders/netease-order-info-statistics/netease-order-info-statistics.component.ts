import { Component, OnInit, Injector } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {  NeteaseOrderInfoServiceProxy, NeteaseOrderInfoStatisticsDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-netease-order-info-statistics',
  templateUrl: './netease-order-info-statistics.component.html',
  styles: [],
})
export class NeteaseOrderInfoStatisticsComponent extends ModalComponentBase implements OnInit {
  entitys: NeteaseOrderInfoStatisticsDto[];

  constructor(injector: Injector, private _neteaseOrderInfoService: NeteaseOrderInfoServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    this._neteaseOrderInfoService
      .getStatistics()
      .finally(() => {})
      .subscribe(result => {
        this.entitys = result.items;
      });
  }

  getAllSum(): string {
    let res = 0;
    this.entitys.forEach(item => {
      res += item.sum;
    });

    return res.toFixed(2);
  }

  getAllServiceCharge(): string {
    let res = 0;
    this.entitys.forEach(item => {
      res += item.serviceCharge;
    });

    return res.toFixed(2);
  }

  getAllValue(): string {
    let res = 0;
    this.entitys.forEach(item => {
      res += item.value;
    });

    return res.toFixed(2);
  }
}
