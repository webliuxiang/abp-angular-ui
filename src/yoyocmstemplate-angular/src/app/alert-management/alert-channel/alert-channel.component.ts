import { Component, OnInit, Injector } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { YSLogSearchObjectListDto, YSLogSearchObjectServiceProxy } from '@shared/service-proxies/api-service-proxies';
import { Router } from '@angular/router';
// import { CreateDiscoverModalComponent } from './component/create-discover-modal/create-discover-modal.component';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';

interface testDto {
  id: number;
  taskString: string;
}

@Component({
  selector: 'app-alert-channel',
  templateUrl: './alert-channel.component.html',
  styles: [],
})
export class AlertChannelComponent extends PagedListingComponentBase<testDto> implements OnInit {
  // 模糊搜索
  filterText = '';
  isVisible = false;
  channelForm!: FormGroup;
  createModalStatus = 'new';

  constructor(
    injector: Injector,
    private _YSLogSearchObjectServiceProxy: YSLogSearchObjectServiceProxy,
    private router: Router,
    private fb: FormBuilder,
  ) {
    super(injector);
    this.init();
  }

  init(): void {
    this.channelForm = this.fb.group({
      channelName: [null, [Validators.required]],
      desc: [],
      channelType: [null, [Validators.required]],
      receiver: [null, [Validators.required]],
    });
  }

  // 搜索
  onSearch(): void {
    this.refresh();
  }

  // 创建
  create() {
    this.isVisible = true;
    this.createModalStatus = 'new';
    this.init();
  }
  // 确认提交
  handleOk(): void {
    this.isVisible = false;
    for (const i in this.channelForm.controls) {
      this.channelForm.controls[i].markAsDirty();
      this.channelForm.controls[i].updateValueAndValidity();
    }
    console.log(this.channelForm.value);
  }
  // 关闭
  handleCancel(): void {
    this.isVisible = false;
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
    this.dataList = [
      {
        id: 1,
        taskString: 'string',
      },
      {
        id: 2,
        taskString: 'string',
      },
    ];
  }

  // 删除
  delete(id) { }

  // 编辑
  edit(id) {
    this.isVisible = true;
    this.createModalStatus = 'edit';
    this.channelForm.patchValue({
      channelName: '测试编辑告警渠道',
      desc: '这是测试编辑告警渠道的描述',
      channelType: 'wechat',
      receiver: '111;222;333',
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

    // abp.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), res => {
    //   if (res) {
    //     const ids = _.map(this.selectedDataItems, 'id');
    //     this._YSLogSearchObjectServiceProxy.batchDelete(ids).subscribe(() => {
    //       this.refresh();
    //       this.notify.success(this.l('SuccessfullyDeleted'));
    //     });
    //   }
    // });
  }
}
