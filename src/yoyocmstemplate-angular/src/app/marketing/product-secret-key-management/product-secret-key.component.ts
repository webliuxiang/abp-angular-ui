import { Component, Injector, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  ProductSecretKeyServiceProxy,
  PagedResultDtoOfProductSecretKeyListDto,
  ProductSecretKeyListDto,
} from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductSecretKeyComponent } from './create-or-edit-product-secret-key/create-or-edit-product-secret-key.component';
import { AppConsts } from 'abpPro/AppConsts';
import { BindProductSecretKeyToUserComponent } from './bind-product-secret-key-to-user/bind-product-secret-key-to-user.component';
//  import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  templateUrl: './product-secret-key.component.html',
  styleUrls: ['./product-secret-key.component.less'],
  animations: [appModuleAnimation()],
})
export class ProductSecretKeyComponent extends PagedListingComponentBase<ProductSecretKeyListDto> implements OnInit {
  constructor(injector: Injector, private _productSecretKeyService: ProductSecretKeyServiceProxy) {
    super(injector);
  }

  /**
   * 获取后端数据列表信息
   * @param request 请求的数据的dto 请求必需参数 skipCount: number; maxResultCount: number;
   * @param pageNumber 当前页码
   * @param finishedCallback 完成后回调函数
   */
  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._productSecretKeyService
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
    this.modalHelper.static(CreateOrEditProductSecretKeyComponent, { id: id }).subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }

  /**
   * 删除功能
   * @param entity 角色的实体信息
   */
  delete(entity: ProductSecretKeyListDto): void {
    this._productSecretKeyService.delete(entity.id).subscribe(() => {
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

    abp.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount),  undefined, res => {
      if (res) {
        const ids = _.map(this.selectedDataItems, 'id');
        this._productSecretKeyService.batchDelete(ids).subscribe(() => {
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
    // this._productSecretKeyService.getProductSecretKeyexportToExcel().subscribe(result => {
    // this._fileDownloadService.downloadTempFile(result);
    // });
  }

  bindToUser(entity: ProductSecretKeyListDto) {
    this.modalHelper.static(BindProductSecretKeyToUserComponent, { secretKey: entity.secretKey }).subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }
}
