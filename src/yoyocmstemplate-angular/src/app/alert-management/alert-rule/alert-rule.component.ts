import { Component, OnInit, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  YSLogDataAlertObjectListDto,
  YSLogDataAlertObjectServiceProxy,
} from '@shared/service-proxies/api-service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
// import { CreateDiscoverModalComponent } from './component/create-discover-modal/create-discover-modal.component';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';
import { ImpersonationService } from '@shared/auth';
import { AppConsts } from 'abpPro/AppConsts';

interface testDto {
  id: number;
  taskString: string;
}

@Component({
  selector: 'app-alert-rule',
  templateUrl: './alert-rule.component.html',
  styleUrls: [],
})
export class AlertRuleComponent extends PagedListingComponentBase<YSLogDataAlertObjectListDto> implements OnInit {
  // 模糊搜索
  filterText = '';
  advancedFiltersVisible = false; // 是否显示高级过滤

  alertObject: YSLogDataAlertObjectListDto = undefined;

  constructor(
    injector: Injector,
    private _activatedRoute: ActivatedRoute,
    public _impersonationService: ImpersonationService,
    private _ySLogDataAlertObjectServiceProxy: YSLogDataAlertObjectServiceProxy,
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
    this.filterText = '';
    this.refreshGoFirstPage();
  }

  /**
   * 获取告警任务数据
   * @param request
   * @param pageNumber
   * @param finishedCallback
   */
  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._ySLogDataAlertObjectServiceProxy
      .getPaged(this.filterText, request.sorting, request.maxResultCount, request.skipCount)
      .pipe(finalize(() => finishedCallback()))
      .subscribe(result => {
        // console.log(result);
        this.dataList = result.items;
        // this.pageSize = result.items.length;
        this.showPaging(result);
      });
  }

  // 创建
  createAlertRule() {
    const self = this;
    setTimeout(() => {
      self.router.navigate(['app/alert-management/alert-rule-config', { isDetail: false }]);
    }, 300);
  }
  // 更多
  edit(id): void {
    const self = this;
    setTimeout(() => {
      self.router.navigate(['app/alert-management/alert-rule-config', { alertId: id, isDetail: true }]);
    }, 300);
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

    this.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        const ids = _.map(this.selectedDataItems, 'id');
        this._ySLogDataAlertObjectServiceProxy.batchDelete(ids).subscribe(() => {
          this.refresh();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  /**
   * 删除功能
   */
  delete(id): void {
    // if (entity.userName === AppConsts.userManagement.defaultAdminUserName) {
    //   abp.message.warn(this.l('XUserCannotBeDeleted', AppConsts.userManagement.defaultAdminUserName));
    //   return;
    // }
    this._ySLogDataAlertObjectServiceProxy.delete(id).subscribe(() => {
      this.refreshGoFirstPage();
      this.notify.success(this.l('SuccessfullyDeleted'));
    });
  }

  refreshCheckStatus(entityList: any[]): void {
    entityList.forEach(item => {
      if (item.userName === AppConsts.userManagement.defaultAdminUserName) {
        item.checked = false;
      }
    });

    // 是否全部选中
    const allChecked = entityList.every(value => value.checked === true);
    // 是否全部未选中
    const allUnChecked = entityList.every(value => !value.checked);
    // 是否全选
    this.allChecked = allChecked;
    // 全选框样式控制
    this.checkboxIndeterminate = !allChecked && !allUnChecked;
    // 已选中数据
    this.selectedDataItems = entityList.filter(value => value.checked);
  }

  // 运行
  run(id) {
    let body: any = {
      id: id,
    };
    this._ySLogDataAlertObjectServiceProxy.startJob(body).subscribe(() => {
      this.refreshGoFirstPage();
      this.notify.success(this.l('启动成功'));
    });
  }
}
