import {
  Component,
  OnInit,
  Input,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Output,
  EventEmitter,
  TemplateRef,
  Injector,
  OnChanges,
  SimpleChanges,
  SimpleChange,
} from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { UploadFile } from 'ng-zorro-antd/upload';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzFormatEmitEvent } from 'ng-zorro-antd/tree';
import { ArrayService, copy } from '@delon/util';
import { AppComponentBase, PagedListingComponentBase, PagedRequestDto } from '@shared/component-base';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from 'abpPro/AppConsts';
import { TokenService } from 'abp-ng2-module';
import {
  SysFileServiceProxy,
  SysFileListDto,
  PagedResultDtoOfSysFileListDto,
  SysFileEditDto,
  ProfileServiceProxy,
} from '../../service-proxies/service-proxies';
import { finalize, filter, catchError } from 'rxjs/operators';
import { EntityDtoOfGuid, MoveSysFilesInput } from '../../service-proxies/service-proxies';
import { element } from 'protractor';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'upload-user-portrait',
  templateUrl: './upload-user-portrait.component.html',
})
export class UploadUserPortraitComponent extends AppComponentBase implements OnInit, OnChanges {
  constructor(injector: Injector, private _profileService: ProfileServiceProxy, private _tokenService: TokenService) {
    super(injector);
    // 设置头部信息
    this.uploadHeaders = {
      Authorization: 'Bearer ' + this._tokenService.getToken(),
    };
  }

  @Input() profilePictureId: string;
  /**
   * 头像列表
   */
  profileList: any[] = [];
  @Output() onRemoveProfilePicture = new EventEmitter<void>();

  @Output() onUpLoadProfilePictureSuccess = new EventEmitter<string>();


  /**
   * 编辑时加载图像
   */
  profileLoading = false;
  /**
   * 头像预览地址
   */
  profilePreviewImage = '';
  /**
   * 预览头像Modal控制
   */
  profilePreviewVisible = false;
  /**
   * 上传控件头部信息
   */
  public uploadHeaders: any;
  /**
 * 图片最大大小 M
 */
  private maxProfilPictureBytesValue = AppConsts.maxProfilPictureMb;
  /**
 * 图片上传后台处理地址
 */
  public uploadPictureUrl: string = AppConsts.remoteServiceBaseUrl + '/Profile/UploadProfilePictureReturnFileId';
  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {

    if (changes.profilePictureId && changes.profilePictureId.currentValue && changes.profilePictureId.currentValue.trim() !== '') {
      this.getProfilePicture(this.profilePictureId);
    }

  }
  ngOnInit(): void {

  }
  /**
 * 通过头像Id获取头像
 * @param profilePictureId 头像Id
 */
  getProfilePicture(profilePictureId: string): void {
    if (profilePictureId) {
      this.profileLoading = true;
      this._profileService
        .getProfilePictureById(profilePictureId)
        .finally(() => (this.profileLoading = false))
        .subscribe(result => {
          if (result && result.profilePicture) {
            this.profilePreviewImage = 'data:image/jpeg;base64,' + result.profilePicture;

            // 把图像加到头像列表 显示
            this.profileList = [
              {
                uid: -1,
                name: profilePictureId,
                status: 'done',
                url: this.profilePreviewImage,
              },
            ];
          }
        });
    }
  }
  /**
 * 头像预览处理
 */
  handleProfilePreview = (file: UploadFile) => {
    this.profilePreviewImage = file.url || file.thumbUrl;
    this.profilePreviewVisible = true;
  }
  /**
   * 图片上传前
   */
  beforeUpload = (file: File) => {
    const isJPG = file.type === 'image/jpeg' || file.type === 'image/png' || file.type === 'image/gif';
    if (!isJPG) {
      abp.message.error(this.l('OnlySupportPictureFile'));
    }
    const isLtXM = file.size / 1024 / 1024 < this.maxProfilPictureBytesValue;
    if (!isLtXM) {
      abp.message.error(this.l('ProfilePicture_Warn_SizeLimit', this.maxProfilPictureBytesValue));
    }
    const isValid = isJPG && isLtXM;
    return isValid;
  }
  /**
 * 移除头像，同时删除数据中的图像数据
 */
  removeProfilePicture = (file: UploadFile): Observable<boolean> => {
    if (!this.profilePictureId) { return of(true); }
    if (!this.isGranted('Pages.Administration.Users.DeleteProfilePicture')) {
      abp.message.error(this.l('YouHaveNoXPermissionsWarningMessage', this.l('DeleteProfilePicture')));
      return of(false);
    }
    this._profileService
      .deleteProfilePictureById(this.profilePictureId)
      .pipe(catchError(err => of('deleteProfilePicture fail!')))
      .subscribe(res => {
        this.onRemoveProfilePicture.emit();
        abp.message.success(this.l('SuccessfullyDeleted'));
      });
    return of(true);
  }

  /**
   * 选择图片后上传事件
   * @param info 反馈信息
   */
  uploadPictureChange(info: { file: UploadFile }) {
    // 状态选择
    switch (info.file.status) {
      case 'done': // 上传完成
        // 获取服务端返回的信息
        if (info.file.response.success) {
          // 上传成功后直接把图片Id给user实体
          this.onUpLoadProfilePictureSuccess.emit(info.file.response.result.profilePictureId);
        }
        break;
      case 'error': // 上传错误
        abp.message.error(this.l('UploadFailed'));
        break;
    }
  }
}
