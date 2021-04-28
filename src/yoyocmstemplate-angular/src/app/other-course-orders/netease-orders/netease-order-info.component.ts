import { Component, Injector, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { appModuleAnimation } from '@shared/animations/routerTransition';


import {
  NullableIdDtoOfInt64,
  NeteaseOrderInfoListDto,
  NeteaseOrderInfoServiceProxy,
  QueryInput,
} from '@shared/service-proxies/service-proxies';
import { NeteaseOrderInfoDetailsComponent } from './netease-order-info-details/netease-order-info-details.component';
import { NeteaseOrderInfoStatisticsComponent } from './netease-order-info-statistics/netease-order-info-statistics.component';
import { DynamicListViewComponentBase, IFetchData } from '@shared/sample/common';
import { finalize } from 'rxjs/operators';
// import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  templateUrl: './netease-order-info.component.html',
  styleUrls: ['./netease-order-info.component.less'],
  animations: [appModuleAnimation()],
})
export class NeteaseOrderInfosComponent extends DynamicListViewComponentBase<NeteaseOrderInfoListDto> implements OnInit {

  transactionStatus = '';
  isChecked: any = '';

  constructor(injector: Injector, private _neteaseOrderInfoService: NeteaseOrderInfoServiceProxy) {
    super(injector);
  }


  /**
   * 初始化的类
   *
   */
  ngOnInit(): void {
    super.ngOnInit();
    this.pageName = 'othercourse.netease-orders';
  }

  /**
   * 获取分页信息表格数据
   */
  fetchData(arg: IFetchData) {
    const input = new QueryInput();
    input.queryConditions = this.queryConditions;
    input.sortConditions = this.sortConditions;
    input.skipCount = arg.skipCount;
    input.maxResultCount = arg.pageSize;

    this._neteaseOrderInfoService.getPaged(input)
      .pipe(finalize(() => {
        if (arg.finishedCallback) {
          arg.finishedCallback();
        }
      }))
      .subscribe((res) => {
        //  console.log(res);
        arg.successCallback(res);
      });


  }





  /**
   * 一个数据测试的demo。为了验证sample-table的可能性。
   */
  dataTest(): void {
    const selectCount = this.checkedData.length;
    console.log(selectCount);


    console.log(this.checkedData);

    // 获取选中买家的列表信息
    const buyerNameList = this.checkedData.map(o => o.buyerName);

    console.log(buyerNameList);

  }










  showDetails(id: number) {
    this.modalHelper.open(NeteaseOrderInfoDetailsComponent, { id: id }).subscribe(() => { });
  }

  showStatistics() {
    this.modalHelper.open(NeteaseOrderInfoStatisticsComponent).subscribe(() => { });
  }

  // updateCheced(id: number, isChecked: boolean): void {
  //   this.isTableLoading = true;
  //   const idInput: NullableIdDtoOfInt64 = new NullableIdDtoOfInt64();
  //   idInput.id = id;
  //   this._neteaseOrderInfoService
  //     .updateChecked(isChecked, idInput)

  //     .finally(() => {
  //       this.isTableLoading = false;
  //     })
  //     .subscribe(() => {
  //       this.refresh();
  //       this.notify.success(this.l('SuccessfullyUpdate'));
  //     });
  // }

  // updateGiteeCheced(id: number, isChecked: boolean): void {
  //   this.isTableLoading = true;
  //   const idInput: NullableIdDtoOfInt64 = new NullableIdDtoOfInt64();
  //   idInput.id = id;
  //   this._neteaseOrderInfoService
  //     .updateChecked(isChecked, idInput)
  //     .finally(() => {
  //       this.isTableLoading = false;
  //     })
  //     .subscribe(() => {
  //       this.refresh();
  //       this.notify.success(this.l('SuccessfullyUpdate'));
  //     });
  // }

  // refreshEntity(id: number): void {
  //   this.isTableLoading = true;
  //   const idInput: NullableIdDtoOfInt64 = new NullableIdDtoOfInt64();
  //   idInput.id = id;
  //   this._neteaseOrderInfoService
  //     .refreshOrderInfo(idInput)
  //     .finally(() => {
  //       this.isTableLoading = false;
  //     })
  //     .subscribe(() => {
  //       this.refresh();
  //       this.notify.success(this.l('SuccessfullySynchronize'));
  //     });
  // }

  // synchronize(): void {
  //   this.isTableLoading = true;
  //   this._neteaseOrderInfoService
  //     .synchronize()
  //     .finally(() => {
  //       this.isTableLoading = false;
  //     })
  //     .subscribe(() => {
  //       this.refreshGoFirstPage();
  //       this.notify.success(this.l('SuccessfullySynchronize'));
  //     });
  // }

  // batchUpdateCheced(isChecked: boolean): void {
  //   const selectCount = this.selectedDataItems.length;
  //   if (selectCount <= 0) {
  //     abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));

  //     return;
  //   }

  //   abp.message.confirm(this.l('ConfirmUpdateXItemsWarningMessage', selectCount), undefined, (res) => {
  //     if (res) {
  //       const ids = _.map(this.selectedDataItems, 'id');
  //       this._neteaseOrderInfoService.batchUpdateChecked(isChecked, ids).subscribe(() => {
  //         this.refresh();
  //         this.notify.success(this.l('SuccessfullyUpdate'));
  //       });
  //     }
  //   });
  // }

  // batchUpdateGiteeCheced(isChecked: boolean): void {
  //   const selectCount = this.selectedDataItems.length;
  //   if (selectCount <= 0) {
  //     abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));

  //     return;
  //   }

  //   abp.message.confirm(this.l('ConfirmUpdateXItemsWarningMessage', selectCount), undefined, (res) => {
  //     if (res) {
  //       const ids = _.map(this.selectedDataItems, 'id');
  //       this._neteaseOrderInfoService.batchUpdateGiteeChecked(isChecked, ids).subscribe(() => {
  //         this.refresh();
  //         this.notify.success(this.l('SuccessfullyUpdate'));
  //       });
  //     }
  //   });
  // }



}
