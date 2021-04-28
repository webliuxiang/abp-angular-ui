import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { SampleComponentBase } from '@shared/component-base';

@Component({
  selector: 'course-class-hour-resource-upload-panel',
  templateUrl: './course-class-hour-resource-upload-panel.component.html',
  styleUrls: ['./course-class-hour-resource-upload-panel.component.less'],
})
export class CourseClassHourResourceUploadPanelComponent extends SampleComponentBase
  implements OnInit {

  /** 调用上传保存 */
  callSave = false;
  /** 上传数据量 */
  dataListLength = 0;
  /** 上传保存状态 */
  saving = false;
  /** 上传状态 */
  uploading = false;
  /** 上传成功回调视频信息 */
  @Output() videoInfoChange = new EventEmitter<any>();

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  ngOnInit(): void {
  }

  /** 开始上传 */
  handleUpload(event: any): boolean {
    this.callSave = true;
    setTimeout(() => {
      this.callSave = false;
    }, 10);

    event.preventDefault();
    return false;
  }

  /** 上传选中的数据长度发生改变 */
  onUploadDataListLengthChange(input: number) {
    this.dataListLength = input;
  }

  /*** 上传保存状态修改 */
  onUploadSavingChange(input: boolean) {
    this.saving = input;
  }

  /** 上传状态更改 */
  onUploadingChange(input: boolean) {
    this.uploading = input;
  }

  /** 上传的视频信息发生改变 */
  onUploadSuccessVideoInfo(video: any) {
    this.videoInfoChange.emit(video);
  }
}
