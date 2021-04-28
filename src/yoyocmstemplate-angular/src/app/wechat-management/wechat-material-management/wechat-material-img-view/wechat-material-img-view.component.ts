import {
  PagedListingComponentBase,
  PagedRequestDto,
} from '@shared/component-base';
import { Component, OnInit, Injector, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import {
  WechatMediaServiceProxy,
  MediaList_Others_Item,
  GetOtherMaterialsInput,
  UploadMediaFileType,
} from '@shared/service-proxies/service-proxies';
import { WechatMaterialType } from 'abpPro/AppEnums';
import { finalize } from 'rxjs/operators';
import { CreateImageMaterialComponent } from '../create-image-material/create-image-material.component';

@Component({
  selector: 'app-wechat-material-img-view',
  templateUrl: './wechat-material-img-view.component.html',
  styles: [],
})
export class WechatMaterialImgViewComponent extends PagedListingComponentBase<MediaList_Others_Item> {
  @Input()
  wechatMediaService: WechatMediaServiceProxy;
  @Input()
  appId: string;

  input: GetOtherMaterialsInput = new GetOtherMaterialsInput();

  constructor(
    injector: Injector,
    private activatedRoute: ActivatedRoute,
    private reuseTabService: ReuseTabService,
  ) {
    super(injector);
  }

  protected fetchDataList(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
  ): void {
    this.input.skipCount = request.skipCount;
    this.input.maxResultCount = request.maxResultCount;
    this.input.materialType = WechatMaterialType.Image;
    this.input.appId = this.appId;
    this.wechatMediaService
      .getOtherMaterialPaged(this.input)
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
      )
      .subscribe(result => {
        this.dataList = result.items;
        this.showPaging(result);
      });
  }

  create() {
    this.modalHelper
      .static(CreateImageMaterialComponent, {
        appId: this.appId,
        wechatMediaService: this.wechatMediaService,
      })
      .subscribe(res => {
        if (res) {
          this.refresh();
        }
      });
  }

  delete(item: any) {
  }

  batchDelete() {
  }
}
