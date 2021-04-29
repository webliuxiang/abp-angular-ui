import { Component, OnInit, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  YSLogDataAnalyzeObjectListDto,
  YSLogDataAnalyzeObjectServiceProxy,
} from '@shared/service-proxies/api-service-proxies';
import { Router } from '@angular/router';
// import { CreateDiscoverModalComponent } from './component/create-discover-modal/create-discover-modal.component';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';

interface testDto {
  id: number;
  taskString: string;
}

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styles: [],
})
export class ReportComponent extends PagedListingComponentBase<YSLogDataAnalyzeObjectListDto> implements OnInit {
  // 模糊搜索
  filterText = '';

  constructor(
    injector: Injector,
    private _ySLogDataAnalyzeObjectServiceProxy: YSLogDataAnalyzeObjectServiceProxy,
    private router: Router,
  ) {
    super(injector);
  }

  // 搜索
  onSearch(): void {
    this.refresh();
  }

  /**
   * 强制刷新
   */
  forceRefresh() {
    this.refreshGoFirstPage();
  }

  // 创建
  createDataReport() {
    const self = this;
    setTimeout(() => {
      self.router.navigate(['app/data-analyze/create-report', { isDetail: false }]);
    }, 300);
  }
  // 更多
  showMore(id): void {
    const self = this;
    setTimeout(() => {
      self.router.navigate(['app/data-analyze/create-report', { reportId: id, isDetail: true }]);
    }, 300);
  }

  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._ySLogDataAnalyzeObjectServiceProxy
      .getPaged(this.filterText, request.sorting, request.maxResultCount, request.skipCount)
      .pipe(finalize(() => finishedCallback()))
      .subscribe(result => {
        // console.log(result);
        this.dataList = result.items;
        // this.pageSize = result.items.length;
        this.showPaging(result);
      });
  }

  // 删除
  delete(id): void {
    this._ySLogDataAnalyzeObjectServiceProxy.delete(id).subscribe(() => {
      this.refresh();
      this.notify.success(this.l('SuccessfullyDeleted'));
    });
  }
  // 运行
  run(id): void {
    const body: any = {
      id: id,
    };
    this._ySLogDataAnalyzeObjectServiceProxy.startJob(body).subscribe(() => {
      this.notify.success(this.l('启动成功'));
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
        this._ySLogDataAnalyzeObjectServiceProxy.batchDelete(ids).subscribe(() => {
          this.refresh();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }
}
