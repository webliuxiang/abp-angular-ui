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

@Component({
  selector: 'app-wechat-material-voice-view',
  templateUrl: './wechat-material-voice-view.component.html',
  styles: [],
})
export class WechatMaterialVoiceViewComponent extends PagedListingComponentBase<MediaList_Others_Item> {
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
    this.input.materialType = WechatMaterialType.Voice;
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
  }

  delete(item) {
  }

  batchDelete() {
  }
}
