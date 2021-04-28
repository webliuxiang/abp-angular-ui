import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  CreateOrUpdateBlogrollInput,
  BlogrollEditDto,
  BlogrollServiceProxy,
  KeyValuePairOfStringString,
  BlogrollTypeListDto,
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { BlogrollTypeServiceProxy } from '../../../../shared/service-proxies/service-proxies';
import { PagedRequestDto } from '../../../../shared/component-base/paged-listing-component-base';

@Component({
  selector: 'create-or-edit-blogroll',
  templateUrl: './create-or-edit-blogroll.component.html',
  styleUrls: ['create-or-edit-blogroll.component.less'],
})
export class CreateOrEditBlogrollComponent extends ModalComponentBase implements OnInit {
  /**
   * 编辑时DTO的id
   */
  id: any;

  entity: BlogrollEditDto = new BlogrollEditDto();

  imgurl: any;

  imgurl2: any;

  blogrollTypeList: BlogrollTypeListDto[] = [];

  /**
   * 构造函数，在此处配置依赖注入
   */
  constructor(
    injector: Injector,
    private _blogrollService: BlogrollServiceProxy,
    private _blogrolltypeService: BlogrollTypeServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
    this.GetBlogrolltypeList();
  }

  /**
   * 初始化方法
   */
  init(): void {
    this._blogrollService.getForEdit(this.id).subscribe(result => {
      this.entity = result.blogroll;
    });
  }

  GetBlogrolltypeList(): void {
    this._blogrolltypeService
      .getPaged('', '', 1000, 0)
      .pipe()
      .subscribe(result => {
        this.blogrollTypeList = result.items;
      });
  }
  _change(event, type) {
    switch (type) {
      case 'iconName':
        this.entity.iconName = event.path;
        break;
    }
  }
  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    const input = new CreateOrUpdateBlogrollInput();
    input.blogroll = this.entity;

    this.saving = true;

    this._blogrollService
      .createOrUpdate(input)
      .pipe(finalize(() => (this.saving = false)))

      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
  }
}
