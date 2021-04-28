
import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
CreateOrUpdateBlogrollTypeInput,
BlogrollTypeEditDto,
BlogrollTypeServiceProxy,
KeyValuePairOfStringString
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-blogroll-type',
  templateUrl: './create-or-edit-blogroll-type.component.html',
  styleUrls: [
	'create-or-edit-blogroll-type.component.less'
    ],
    })

    export class CreateOrEditBlogrollTypeComponent
    extends ModalComponentBase
    implements OnInit {
    /**
    * 编辑时DTO的id
    */
    id: any ;

    entity: BlogrollTypeEditDto = new BlogrollTypeEditDto();

    /**
    * 构造函数，在此处配置依赖注入
    */
    constructor(
    injector: Injector,
    private _blogrollTypeService: BlogrollTypeServiceProxy

    ) {
    super(injector);
    }

    ngOnInit(): void{
    this.init();
    }


    /**
    * 初始化方法
    */
    init(): void {
    this._blogrollTypeService.getForEdit(this.id).subscribe(result => {
    this.entity = result.blogrollType;
    

                           
                           
                           });
    }

    /**
    * 保存方法,提交form表单
    */
    submitForm(): void {
    const input = new CreateOrUpdateBlogrollTypeInput();
    input.blogrollType = this.entity;

    this.saving = true;

    this._blogrollTypeService.createOrUpdate(input)
    .finally(() => (this.saving = false))
    .pipe(finalize(() => (this.saving = false)))
    .subscribe(() => {
    this.notify.success(this.l('SavedSuccessfully'));
    this.success(true);
    });
    }
    }
