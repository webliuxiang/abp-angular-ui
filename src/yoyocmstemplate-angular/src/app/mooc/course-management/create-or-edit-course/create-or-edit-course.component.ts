import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit, NgZone, Renderer2, ElementRef } from '@angular/core';
import {
  CourseCategoryServiceProxy,
  CourseDto,
  CourseServiceProxy,
  CreateOrUpdateCourseDto,
  SysFileListDto,
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { AppComponentBase } from '@shared/component-base';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ReuseTabService, STColumn, STChange } from '@delon/abc';
import { finalize, delay, filter } from 'rxjs/operators';
import { Route } from '@angular/compiler/src/core';
import { of } from 'rxjs';
import { TokenService } from 'abp-ng2-module';
import { AppConsts } from 'abpPro/AppConsts';
import { NzTreeSelectComponent } from 'ng-zorro-antd/tree-select';
import { UploadFile } from 'ng-zorro-antd/upload';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { ArrayService } from '@delon/util';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { RequestHelper } from '@shared/helpers/RequestHelper';
import { WechatMaterialType } from 'abpPro/AppEnums';

@Component({
  selector: 'create-or-edit-course',
  templateUrl: './create-or-edit-course.component.html',
  styleUrls: ['create-or-edit-course.component.less'],
})
export class CreateOrEditCourseComponent extends AppComponentBase implements OnInit {
  /**
   * 图片上传后台处理地址
   */
  public uploadPictureUrl: string =
    AppConsts.remoteServiceBaseUrl + '/api/services/app/FileManagement/UploadFileToSysFiles';


  /** 实体id */
  id: any;
  /** 实体 */
  entity = new CourseDto();

  loading = true;


  /** 课程连载类型 */
  courseTypeEnum = {
    type: 'default',
    dataSource: 'enum',
    enumType: 'CourseTypeEnum',
    allowClear: false,
  };
  /** 课程视频类型 */
  courseVideoTypeEnum = {
    type: 'default',
    dataSource: 'enum',
    enumType: 'CourseVideoTypeEnum',
    allowClear: false,
  };
  /** 课程发布状态 */
  courseStateEnum = {
    type: 'default',
    dataSource: 'enum',
    enumType: 'CourseStateEnum',
    allowClear: false,
  };

  imageUrl: string;
  nodes: any[];
  liveDate: Date;

  /** 分类id数组 */
  categoryIds: number[];

  /**
   * 初始化的构造函数
   */
  constructor(
    injector: Injector,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _courseService: CourseServiceProxy,
    private _reuseTabService: ReuseTabService,
    public _zone: NgZone,
    private _tokenService: TokenService,
    private _courseCategoryService: CourseCategoryServiceProxy,
    private _arrService: ArrayService,
  ) {
    super(injector);
    this.entity.description = '';
  }

  get authHeader() {
    return {
      Authorization: 'Bearer ' + this._tokenService.getToken(),
    };
  }

  ngOnInit(): void {
    this.onLoadCategory();

    this.onLoadData();
    if (this.id) {
      this.titleSrvice.setTitle(this.l('编辑课时'));
    } else {
      this.titleSrvice.setTitle(this.l('新增课时'));
    }

  }

  /**
   * 初始化方法
   */
  onLoadData(): void {
    this.id = this._activatedRoute.snapshot.params.id;
    this._reuseTabService.title = this.l('加载中...');
    this._courseService.getForEdit(this.id).subscribe(result => {
      this.categoryIds = result.categoryIds;
      this.entity = result.entity;

      if (this.entity.title === null) {
        this._reuseTabService.title = this.l('新增') + this.l('课程');
        this.entity.description = '';
      } else {
        this._reuseTabService.title = this.l('编辑') + this.l('课程') + ' - ' + this.entity.title;
      }

      setTimeout(() => {
        this._reuseTabService.refresh();
      }, 300);

      this.imageUrl = UrlHelper.processImgUrl(this.entity.imgUrl);
      this.loading = false;
    });
  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    const input = new CreateOrUpdateCourseDto();
    input.entity = this.entity;
    input.categoryIds = this.categoryIds;
    this.saving = true;
    this._courseService
      .createOrUpdate(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.close();
      });
  }

  /** 分类发生改变 */
  onCategoryChange(ctrl: any, value: any[]): void {
    const nodes = ctrl.valueAccessor.getCheckedNodeList();
    this.categoryIds = this._arrService.getKeysByTreeNode(nodes);
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
   * 加载分类数据
   */
  onLoadCategory() {
    this._courseCategoryService.getAllCourseCategoriesList().subscribe(result => {
      this.nodes = [];

      this.nodes = this._arrService.arrToTreeNode(result.items, {
        titleMapName: 'name',
        parentIdMapName: 'parentId',
        cb: (item, parent, deep) => {
        },
      });
    });
  }

  close(e?: any) {
    abp.event.trigger('abp.course.refresh');

    this._router.navigate(['app/mooc/course']);
  }

}
