import {
  Component,
  OnInit,
  Injector,
} from '@angular/core';
import { ModalComponentBase } from '@shared/component-base';


@Component({
  selector: 'app-upload-video-resource',
  templateUrl: './upload-video-resource.component.html',
  styleUrls: ['./upload-video-resource.component.less']
})
export class UploadVideoResourceComponent extends ModalComponentBase implements OnInit {

  callSave = false;
  dataListLength = 0;
  saving = false;
  uploading = false;

  constructor(
    injector: Injector
  ) {
    super(injector);
  }
  ngOnInit(): void {

  }




  /** 开始上传 */
  handleUpload(): void {
    this.callSave = true;
    setTimeout(() => {
      this.callSave = false;
    }, 10);
  }

  /** 数据长度发生改变 */
  onDataListLengthChange(input: number) {
    this.dataListLength = input;
  }

  /*** 保存状态修改 */
  onSavingChange(input: boolean) {
    this.saving = input;
  }

  /** 上传状态更改 */
  onUploadingChange(input: boolean) {
    this.uploading = input;
  }

  /** 成功 */
  onSuccess() {
    this.message.success(this.l('SavedSuccessfully'));
    this.success();
  }

}
