import { Component, Injector, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { VideoResourceServiceProxy, VideoResourceListDto, QueryInput } from '@shared/service-proxies/service-proxies';
import { ImgShowComponent } from '@shared/components/img-show/img-show.component';
import { UploadVideoResourceComponent } from './upload-video-resource/upload-video-resource.component';
import { DynamicListViewComponentBase, IFetchData } from '@shared/sample/common';
import { finalize } from 'rxjs/operators';

@Component({
  templateUrl: './video-resource.component.html',
  styleUrls: ['./video-resource.component.less'],
  animations: [appModuleAnimation()],
})
export class VideoResourceComponent extends DynamicListViewComponentBase<VideoResourceListDto>
  implements OnInit {

  constructor(
    injector: Injector,
    private videoResourceSer: VideoResourceServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.pageName = 'mooc.video-resource';
  }

  fetchData(arg: IFetchData) {
    const input = new QueryInput();
    input.queryConditions = this.queryConditions;
    input.sortConditions = this.sortConditions;
    input.skipCount = arg.skipCount;
    input.maxResultCount = arg.pageSize;

    this.videoResourceSer.getPaged(input)
      .pipe(finalize(() => {
        if (arg.finishedCallback) {
          arg.finishedCallback();
        }
      }))
      .subscribe((res) => {
        arg.successCallback(res);
      });
  }

  // 上传视频
  uploadVodInfo(): void {
    this.modalHelper.static(UploadVideoResourceComponent).subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }

  /**
   * 新增或编辑DTO信息
   * @param input 课程信息
   */
  createOrEdit(input?: VideoResourceListDto): void {
    this.modalHelper.static(UploadVideoResourceComponent, { id: input?.id }).subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }

  synchronizeAliyunVod(): void {
    this.videoResourceSer.synchronizeAliyunVod().subscribe(() => {
      this.refresh();
      this.notify.info('同步阿里云VOD视频成功');
    });
  }

  /**
   * 删除功能
   * @param entity 角色的实体信息
   */
  delete(entity: VideoResourceListDto): void {
    this.videoResourceSer.delete(entity.videoId).subscribe(() => {
      this.refresh(true);
      this.notify.success(this.l('SuccessfullyDeleted'));
    });
  }

  /**
   * 批量删除
   */
  batchDelete(): void {
    const selectCount = this.checkedData.length;
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }

    abp.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        const ids = this.checkedData.map(o => o.videoId);
        this.videoResourceSer.batchDelete(ids).subscribe(() => {
          this.refresh(true);
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
  }

  showImg(url: string) {
    this.modalHelper.open(ImgShowComponent, { imgUrl: url }, 'md').subscribe(() => {
    });
  }
}
