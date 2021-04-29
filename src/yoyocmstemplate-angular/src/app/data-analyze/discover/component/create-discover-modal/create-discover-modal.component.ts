import { Component, OnInit, Injector } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { YSLogDataSetObjectListDto, YSLogDataSetObjectServiceProxy } from '@shared/service-proxies/api-service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-create-discover-modal',
  templateUrl: './create-discover-modal.component.html',
  styles: [],
})
export class CreateDiscoverModalComponent extends PagedListingComponentBase<YSLogDataSetObjectListDto>
  implements OnInit {
  createForm: FormGroup;
  datasourcesList = [];
  queryResultList = [];
  dataSource = null;
  constructor(
    injector: Injector,
    private nzModalRef: NzModalRef,
    private fb: FormBuilder,
    private _noti: NzNotificationService,
    private _dataSetService: YSLogDataSetObjectServiceProxy,
  ) {
    super(injector);
  }
  ngOnInit(): void {
    this.createForm = this.fb.group({
      chartType: [null],
      dataSetId: [null, [Validators.required]],
    });
    this.queryResultList = [];
  }

  create() {
    for (const i in this.createForm.controls) {
      this.createForm.controls[i].markAsDirty();
      this.createForm.controls[i].updateValueAndValidity();
    }
    if (this.createForm.valid) {
      this.nzModalRef.close(this.createForm.value);
    }
  }

  // 当数据集选择器打开关闭时
  onOpenChange(e) {
    if (e) {
      this.refresh();
    }
  }

  /**
   * 获取远端数据
   * @param request
   * @param pageNumber
   * @param finishedCallback
   */
  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._dataSetService
      .getPaged(undefined, undefined, undefined, undefined, undefined)
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
      )
      .subscribe(result => {
        this.datasourcesList = result.items;
      });
  }

  cancel() {
    this.nzModalRef.destroy();
  }
}
