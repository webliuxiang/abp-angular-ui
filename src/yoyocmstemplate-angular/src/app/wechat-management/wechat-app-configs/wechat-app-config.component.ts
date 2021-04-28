import { Component, Injector, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  WechatAppConfigServiceProxy,
  PagedResultDtoOfWechatAppConfigListDto,
  WechatAppConfigListDto,
} from '@shared/service-proxies/service-proxies';
import { CreateOrEditWechatAppConfigComponent } from './create-or-edit-wechat-app-config/create-or-edit-wechat-app-config.component';

import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { ImgShowComponent } from '@shared/components/img-show/img-show.component';
//  import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  templateUrl: './wechat-app-config.component.html',
  styleUrls: ['./wechat-app-config.component.less'],
  animations: [appModuleAnimation()],
})
export class WechatAppConfigComponent extends PagedListingComponentBase<WechatAppConfigListDto> implements OnInit {
  constructor(
    injector: Injector,
    private router: Router,
    private _wechatAppConfigService: WechatAppConfigServiceProxy,
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
    this._wechatAppConfigService
      .getPaged(this.filterText, request.sorting, request.maxResultCount, request.skipCount)
      .pipe(finalize(() => finishedCallback()))
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
    this.modalHelper.static(CreateOrEditWechatAppConfigComponent, { id: id }).subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }

  /**
   * 删除功能
   * @param entity 角色的实体信息
   */
  delete(entity: WechatAppConfigListDto): void {
    this._wechatAppConfigService.delete(entity.id).subscribe(() => {
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
        this._wechatAppConfigService.batchDelete(ids).subscribe(() => {
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
    // this._wechatAppConfigService.getWechatAppConfigexportToExcel().subscribe(result => {
    // this._fileDownloadService.downloadTempFile(result);
    // });
  }

  /**
   * 显示二维码图片
   * @param url 二维码链接
   */
  showImg(url: string) {
    this.modalHelper.open(ImgShowComponent, { imgUrl: url }, 'md').subscribe(() => {});
  }

  /**
   * 编辑自定义菜单
   * @param item
   */
  editMenu(item: WechatAppConfigListDto): void {
    const self = this;
    setTimeout(() => {
      self.router.navigate(['app/wechat/create-or-edit-wechat-menu', { appId: item.appId, appName: item.name }]);
    }, 300);
  }

  /**
   * 注册app
   * @param item
   */
  registerApp(item: WechatAppConfigListDto) {
    this._wechatAppConfigService
      .registerWechatApp(item.appId)
      .pipe(finalize(() => {}))
      .subscribe(() => {
        this.notify.success(this.l('RegisterWechatAppSuccessfully'));
        this.refresh();
      });
  }

  editMaterial(item: WechatAppConfigListDto) {
    const self = this;
    setTimeout(() => {
      self.router.navigate(['app/wechat/wechat-materials', { appId: item.appId, appName: item.name }]);
    }, 300);
  }
}
