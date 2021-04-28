import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  CreateOrUpdateCourseCategoryInput,
  CourseCategoryEditDto,
  CourseCategoryServiceProxy,
  KeyValuePairOfStringString,
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { CourseCategoryListDto, SysFileListDto } from '../../../../shared/service-proxies/service-proxies';
import { AppConsts } from 'abpPro/AppConsts';
import { UploadFile } from 'ng-zorro-antd/upload';
import { TokenService } from 'abp-ng2-module';
import { UrlHelper } from '@shared/helpers/UrlHelper';

@Component({
  selector: 'create-or-edit-course-category',
  templateUrl: './create-or-edit-course-category.component.html',
  styleUrls: ['create-or-edit-course-category.component.less'],
})
export class CreateOrEditCourseCategoryComponent extends ModalComponentBase implements OnInit {
  /**
   * 列表传递过来的DTO
   */
  parentId: any;
  id: any;
  entity: CourseCategoryEditDto = new CourseCategoryEditDto();

  imageUrl = '';
  // split
  /**
   * 图片上传后台处理地址
   */
  public uploadPictureUrl: string =
    AppConsts.remoteServiceBaseUrl + '/api/services/app/FileManagement/UploadFileToSysFiles';
  /**
   * 构造函数，在此处配置依赖注入
   */
  constructor(
    injector: Injector,
    private _courseCategoryService: CourseCategoryServiceProxy,
    private _tokenService: TokenService,
  ) {
    super(injector);
  }

  get authHeader() {
    return {
      Authorization: 'Bearer ' + this._tokenService.getToken(),
    };
  }

  ngOnInit(): void {
    // console.log(this.parentId);
    // console.log(this.id);
    if (this.id != undefined) {
      this.init();
    }

    //
  }

  /**
   * 初始化方法
   */
  init(): void {
    this._courseCategoryService.getForEdit(this.id).subscribe(result => {
      this.entity = result.courseCategory;
      // debugger;
      this.imageUrl = UrlHelper.processImgUrl(this.entity.imgUrl);
    });
  }

  beforeUpload = (file: File) => {
    const isJPG =
      file.type === 'image/jpeg' || file.type === 'image/jpg' || file.type === 'image/png' || file.type === 'image/gif';
    if (!isJPG) {
      this.message.error('只能上传图片类型!');
    }
    const isLt2M = file.size / 1024 / 1024 < 2;
    if (!isLt2M) {
      this.message.error('图片最大不能超过2MB!');
    }
    return isJPG && isLt2M;
  }
  handleChange(info: { file: UploadFile }): void {
    console.log(info);
    if (info.file.status === 'uploading') {
      this.loading = true;
      return;
    }
    if (info.file.status === 'done') {
      this.loading = false;
      if (info.file.response.success) {
        const result = info.file.response.result as SysFileListDto[];
        this.entity.imgUrl = result[0].path;

        this.imageUrl = AppConsts.remoteServiceBaseUrl + '/sysfiles/' + result[0].path;
        //   console.log(result);
      }
      if (info.file.response.error) {
        this.message.error('上传失败：' + info.file.response.error.message);
        return;
      }
      // this.imageUrl = info.file.response.result;
      // this.category.imgUrl = this.imageUrl;
    }
  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    const input = new CreateOrUpdateCourseCategoryInput();
    input.courseCategory = this.entity;
    if (this.parentId !== undefined) {
      input.courseCategory.parentId = this.parentId;
    }
    this.saving = true;

    this._courseCategoryService
      .createOrUpdate(input)
      .finally(() => (this.saving = false))
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
  }
}
