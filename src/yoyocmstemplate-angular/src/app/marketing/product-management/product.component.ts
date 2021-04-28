import { Component, Injector, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  ProductServiceProxy,
  PagedResultDtoOfProductListDto,
  ProductListDto,
  NullableIdDtoOfGuid,
} from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductComponent } from './create-or-edit-product/create-or-edit-product.component';
import { AppConsts } from 'abpPro/AppConsts';
import { finalize } from 'rxjs/operators';
//  import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.less'],
  animations: [appModuleAnimation()],
})
export class ProductComponent extends PagedListingComponentBase<ProductListDto> implements OnInit {
  constructor(injector: Injector, private _productService: ProductServiceProxy) {
    super(injector);
  }

  /**
   * 获取后端数据列表信息
   * @param request 请求的数据的dto 请求必需参数 skipCount: number; maxResultCount: number;
   * @param pageNumber 当前页码
   * @param finishedCallback 完成后回调函数
   */
  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._productService
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
   * 新增或编辑DTO信息
   * @param id 当前DTO的Id
   */
  createOrEdit(id?: number): void {
    this.modalHelper.static(CreateOrEditProductComponent, { id: id }).subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }

  /**
   * 删除功能
   * @param entity 角色的实体信息
   */
  delete(entity: ProductListDto): void {
    this._productService.delete(entity.id).subscribe(() => {
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

    abp.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined,  res => {
      if (res) {
        const ids = _.map(this.selectedDataItems, 'id');
        this._productService.batchDelete(ids).subscribe(() => {
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
    // this._productService.getProductexportToExcel().subscribe(result => {
    // this._fileDownloadService.downloadTempFile(result);
    // });
  }

  getProductTypeLocStr(label: string): string {
    return this.l(`ProductType${label}`);
  }

  publish(entity: ProductListDto) {
    this.isTableLoading = true;

    const input = new NullableIdDtoOfGuid();
    input.id = entity.id;

    this._productService
      .publishProduct(input)
      .pipe(finalize(() => {}))
      .subscribe(() => {
        this.refresh();
        this.notify.success(`产品 ${entity.name} 发布成功!`);
      });
  }

  unshelveProduct(entity: ProductListDto) {
    this.isTableLoading = true;
    const input = new NullableIdDtoOfGuid();
    input.id = entity.id;

    this._productService
      .unshelveProduct(input)
      .pipe(finalize(() => {}))
      .subscribe(() => {
        this.refresh();
        this.notify.success(`产品 ${entity.name} 下架成功!`);
      });
  }
}
