import { Component, OnInit, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { YSLogSearchObjectListDto, YSLogSearchObjectServiceProxy } from '@shared/service-proxies/api-service-proxies';
import { Router } from '@angular/router';
// import { CreateDiscoverModalComponent } from './component/create-discover-modal/create-discover-modal.component';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-alert-subscribe',
  templateUrl: './alert-subscribe.component.html',
  styleUrls: ['./alert-subscribe.component.less'],
})
export class AlertSubscribeComponent extends PagedListingComponentBase<YSLogSearchObjectListDto> implements OnInit {
  // 模糊搜索
  filterText = '';

  constructor(
    injector: Injector,
    private _YSLogSearchObjectServiceProxy: YSLogSearchObjectServiceProxy,
    private router: Router,
  ) {
    super(injector);
  }

  // 搜索
  onSearch(): void {
    this.refresh();
  }

  // 创建
  createDesicover(): void {
    const self = this;
    // setTimeout(() => {
    //   self.router.navigate(['app/data-analyze/create-discover', { id: this.dataSet.dataSetId, isEdit: false }]);
    // }, 300);
  }

  // 编辑
  editDiscover(id): void {
    const self = this;
    setTimeout(() => {
      self.router.navigate(['app/data-analyze/create-discover', { discoverId: id, isEdit: true }]);
    }, 300);
  }
  // 打开创建数据检索弹出框
  createOrEdit(): void {
    // this.modalHelper.open(CreateDiscoverModalComponent, {}, 'md').subscribe(res => {
    //   if (res) {
    //     // this.refresh();
    //     // console.log(res);
    //     this.dataList = res;
    //     this.createDesicover();
    //   }
    // });
  }

  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    // this._YSLogSearchObjectServiceProxy
    //   .getPaged(this.filterText, request.sorting, request.maxResultCount, request.skipCount)
    //   .pipe(finalize(() => finishedCallback()))
    //   .subscribe(result => {
    //     // console.log(result);
    //     this.dataList = result.items;
    //     // this.pageSize = result.items.length;
    //     this.showPaging(result);
    //   });
    this.dataList = [];
  }

  /**
   * 删除
   * @param YSLogSearchObject 可视化配置实体
   */
  delete(YSLogSearchObject: YSLogSearchObjectListDto): void {
    this._YSLogSearchObjectServiceProxy.delete(YSLogSearchObject.id).subscribe(() => {
      this.refresh();
      this.notify.success(this.l('SuccessfullyDeleted'));
    });
  }

  /**
   * 批量删除
   */
  batchDelete(): void {
    const selectCount = this.selectedDataItems.length;
    if (selectCount <= 0) {
      // abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }

    abp.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        const ids = _.map(this.selectedDataItems, 'id');
        this._YSLogSearchObjectServiceProxy.batchDelete(ids).subscribe(() => {
          this.refresh();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }
}
