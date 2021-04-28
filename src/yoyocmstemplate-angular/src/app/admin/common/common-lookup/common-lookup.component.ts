import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  Injector
} from '@angular/core';
import { AppConsts } from 'abpPro/AppConsts';
import { NameValueDto } from '@shared/service-proxies/service-proxies';
import { ICommonLookupModalOptions } from './interface';
import {
  ModalPagedListingComponentBase,
  PagedRequestDto
} from '@shared/component-base';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-common-lookup',
  templateUrl: './common-lookup.component.html',
  styles: []
})
export class CommonLookupComponent extends ModalPagedListingComponentBase<
  NameValueDto
> {
  static defaultOptions: ICommonLookupModalOptions = {
    dataSource: null,
    canSelect: () => true,
    loadOnStartup: true,
    isFilterEnabled: true,
    pageSize: AppConsts.grid.defaultPageSize
  };

  @Output() itemSelected: EventEmitter<NameValueDto> = new EventEmitter<
    NameValueDto
  >();

  options: ICommonLookupModalOptions;

  isShown = false;
  isInitialized = false;
  filterText = '';
  tenantId?: number;

  dataItems: NameValueDto[] = [];

  constructor(injector: Injector) {
    super(injector);
  }

  selectItem(item: NameValueDto) {
    const boolOrPromise = this.options.canSelect(item);
    if (!boolOrPromise) {
      return;
    }

    if (boolOrPromise === true) {
      this.itemSelected.emit(item);
      this.success(item);
      return;
    }

    // assume as observable
    (boolOrPromise as Observable<boolean>).subscribe(result => {
      if (result) {
        this.itemSelected.emit(item);
        this.success(item);
      }
    });
  }

  protected delete(entity: NameValueDto): void {}

  protected getDataList(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    this.options
      .dataSource(
        request.skipCount,
        request.maxResultCount,
        this.filterText,
        this.tenantId
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe(result => {
        this.dataItems = result.items;
        this.showPaging(result);
      });
  }
}
