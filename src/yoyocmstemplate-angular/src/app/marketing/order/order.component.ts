import { Component, Injector, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { OrderServiceProxy, PagedResultDtoOfOrderListDto, OrderListDto, EntityDtoOfGuid } from '@shared/service-proxies/service-proxies';
import { AppConsts } from 'abpPro/AppConsts';
import { ProductOrderStatisticsComponent } from './product-order-statistics/product-order-statistics.component';
import { ModalHelper } from '@delon/theme';
import { EditOrderPriceComponent } from '@app/marketing/order/edit-order-price';
import { finalize } from 'rxjs/operators';
import { CreateOrEditOrderComponent } from './create-or-edit-order/create-or-edit-order.component';

//  import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.less'],
  animations: [appModuleAnimation()],
})
export class OrderComponent extends PagedListingComponentBase<OrderListDto>
  implements OnInit {

  constructor(
    injector: Injector,
    private _orderService: OrderServiceProxy,
  ) {
    super(injector);
  }

  /**
   * 获取后端数据列表信息
   * @param request 请求的数据的dto 请求必需参数 skipCount: number; maxResultCount: number;
   * @param pageNumber 当前页码
   * @param finishedCallback 完成后回调函数
   */
  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._orderService
      .getPaged(this.filterText, request.sorting, request.maxResultCount, request.skipCount)
      .finally(() => {
        finishedCallback();
      })
      .subscribe(result => {
        this.dataList = result.items;
        this.showPaging(result);
      });
  }

  /**
   * 删除功能
   * @param entity 角色的实体信息
   */
  delete(entity: OrderListDto): void {
    this._orderService.delete(entity.id).subscribe(() => {
      /**
       * 刷新表格数据并跳转到第一页（`pageNumber = 1`）
       */
      this.refreshGoFirstPage();
      this.notify.success(this.l('SuccessfullyDeleted'));
    });
  }

  /**
   * 批量删除
   */
  batchDelete(): void {
    const selectCount = this.selectedDataItems.length;
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }

    abp.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        const ids = _.map(this.selectedDataItems, 'id');
        this._orderService.batchDelete(ids).subscribe(() => {
          this.refreshGoFirstPage();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  /**
   * 导出为Excel表
   */
  exportToExcel(): void {
    abp.message.error('功能开发中！！！！');
    // this._orderService.getOrderexportToExcel().subscribe(result => {
    // this._fileDownloadService.downloadTempFile(result);
    // });
  }

  showStatistics() {
    this.modalHelper.open(ProductOrderStatisticsComponent).subscribe(() => {
    });
  }

  /** 赠送订单 */
  present(entity: OrderListDto) {
    this.isTableLoading = true;
    const input = new EntityDtoOfGuid({
      id: entity.id,
    });
    this._orderService.present(input)
      .pipe(finalize(() => {
        this.isTableLoading = false;
      }))
      .subscribe(() => {
        this.notify.success(this.l('SuccessfullyOperation'));
      });
  }

  /** 编辑订单价格 */
  oncEditPrice(entity: OrderListDto) {
    this.modalHelper.createStatic(
      EditOrderPriceComponent,
      {
        order: entity,
      },
    ).subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }


  onEdit(entity: OrderListDto) {
    this.modalHelper.createStatic(
      CreateOrEditOrderComponent,
      {
        order: entity,
      },
    ).subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }
}
