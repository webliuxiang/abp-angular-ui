import { Component, OnInit, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  YSLogVisualizeObjectListDto,
  YSLogVisualizeObjectServiceProxy,
} from '@shared/service-proxies/api-service-proxies';
import { Router } from '@angular/router';
import { CreateChartComponent } from './component/create-chart-modal/create-chart-modal.component';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';

interface dataSetAndChartType {
  dataSetId: number;
  chartType: string;
}
@Component({
  selector: 'app-visualize',
  templateUrl: './visualize.component.html',
  styleUrls: ['./visualize.component.less'],
})
export class VisualizeComponent extends PagedListingComponentBase<YSLogVisualizeObjectListDto> {
  dataSetAndChartType: dataSetAndChartType = undefined;
  // 模糊搜索
  filterText = '';

  constructor(
    injector: Injector,
    private _YSLogVisualizeObjectService: YSLogVisualizeObjectServiceProxy,
    private router: Router,
  ) {
    super(injector);
  }

  // 搜索
  onSearch(): void {
    this.refresh();
  }

  // 创建
  createVisualize(): void {
    const self = this;
    setTimeout(() => {
      self.router.navigate([
        'app/data-analyze/create-visualize',
        { id: this.dataSetAndChartType.dataSetId, type: this.dataSetAndChartType.chartType, isEdit: false },
      ]);
    }, 300);
  }

  // 设置图表名称
  setChartName(type) {
    let name = '';
    switch (type) {
      case 'lineChart':
        name = '折线图';
        break;
      case 'pieChart':
        name = '饼图';
        break;
      case 'barChart':
        name = '柱状图';
        break;
      case 'tableChart':
        name = '表格';
        break;
      case 'areaChart':
        name = '面积图';
        break;
    }
    return name;
  }

  // 编辑
  editVisualize(id): void {
    const self = this;
    setTimeout(() => {
      self.router.navigate(['app/data-analyze/create-visualize', { visualizeId: id, isEdit: true }]);
    }, 300);
  }

  openCreateChartModal(): void {
    this.modalHelper.open(CreateChartComponent, {}, 'md').subscribe(res => {
      if (res) {
        // this.refresh();
        // console.log(res);
        this.dataSetAndChartType = res;
        this.createVisualize();
      }
    });
  }

  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._YSLogVisualizeObjectService
      .getPaged(this.filterText, request.sorting, request.maxResultCount, request.skipCount)
      .pipe(finalize(() => finishedCallback()))
      .subscribe(result => {
        // console.log(result);
        this.dataList = result.items;
        // // this.pageSize = result.items.length;
        this.showPaging(result);
      });
  }

  /**
   * 删除
   * @param YSLogVisualizeObject 可视化配置实体
   */
  delete(YSLogVisualizeObject: YSLogVisualizeObjectListDto): void {
    this._YSLogVisualizeObjectService.delete(YSLogVisualizeObject.id).subscribe(() => {
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
        this._YSLogVisualizeObjectService.batchDelete(ids).subscribe(() => {
          this.refresh();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  log(data: string): void {
    console.log(data);
  }
}
