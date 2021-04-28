import { Component, OnInit, Injector } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { NeteaseOrderInfoEditDto, NeteaseOrderInfoServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-netease-order-info-details',
  templateUrl: './netease-order-info-details.component.html',
  styles: [],
})
export class NeteaseOrderInfoDetailsComponent extends ModalComponentBase implements OnInit {
  id: number;
  entity: NeteaseOrderInfoEditDto;

  constructor(injector: Injector, private _neteaseOrderInfoService: NeteaseOrderInfoServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    this._neteaseOrderInfoService
      .getDetailsById(this.id)
      .finally(() => {})
      .subscribe(result => {
        this.entity = result;
      });
  }
}
