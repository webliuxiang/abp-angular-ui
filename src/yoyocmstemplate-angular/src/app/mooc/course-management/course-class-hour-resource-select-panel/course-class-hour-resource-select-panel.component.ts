import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { DynamicListViewComponentBase, IFetchData } from '@shared/sample/common';
import { QueryInput, VideoResourceListDto, VideoResourceServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'course-class-hour-resource-select-panel',
  templateUrl: './course-class-hour-resource-select-panel.component.html',
  styleUrls: ['./course-class-hour-resource-select-panel.component.less'],
})
export class CourseClassHourResourceSelectPanelComponent
  extends DynamicListViewComponentBase<VideoResourceListDto>
  implements OnInit {

  /** 上传成功回调视频信息 */
  @Output() videoInfoChange = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private videoResourceSer: VideoResourceServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.pageName = 'mooc.video-resource-select';
    this.pageInfo.scroll = { x: '1300px', y: '300px' };
  }

  fetchData(arg: IFetchData) {
    const input = new QueryInput();
    input.queryConditions = arg.queryConditions;
    input.sortConditions = arg.sortConditions;
    input.maxResultCount = arg.pageSize;
    input.skipCount = arg.skipCount;

    console.log('ad');
    this.videoResourceSer.getPaged(input)
      .pipe(finalize(() => {
        arg!.finishedCallback();
      }))
      .subscribe((res) => {
        arg!.successCallback(res);
      });
  }

  search(event: any) {
    this.refresh(true);
    event.preventDefault();
    return false;
  }

  handleUpload(event: any) {
    this.videoInfoChange.emit(this.checkedData[0]);
    event.preventDefault();
    return false;
  }

}
