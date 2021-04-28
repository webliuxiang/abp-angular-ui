import { Component, OnInit, Input, Injector } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { AppComponentBase } from '@shared/component-base';

import { ICommonLookupModalOptions } from '@app/admin/common/common-lookup/interface';
import { AppConsts } from 'abpPro/AppConsts';
import { PagedResultDtoOfNameValueDto, NameValueDto } from '@shared/service-proxies/service-proxies';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-lookup',
  templateUrl: './lookup.component.html',
  styles: [],
})
export class LookupComponent extends AppComponentBase implements OnInit {
  @Input() dataSource: (
    skipCount: number,
    maxResultCount: number,
    filter: string,
    tenantId?: number,
  ) => Observable<PagedResultDtoOfNameValueDto>;

  @Input() filterText = '';
  @Input() tenantId?: number;

  pageIndex = 1;
  pageSize = 10;
  total = 1;
  listOfData: PagedResultDtoOfNameValueDto = new PagedResultDtoOfNameValueDto();

  constructor(injector: Injector, private modal: NzModalRef) {
    super(injector);
  }

  ngOnInit() {
    this.dataSource((this.pageIndex - 1) * this.pageSize, this.pageSize, this.filterText, this.tenantId).subscribe(
      result => {
        this.listOfData = result;
      },
    );
  }

  selectItem(item: NameValueDto) {
    this.modal.destroy(item);
  }
}
