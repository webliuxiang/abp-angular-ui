
import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
CreateOrUpdateWebSiteNoticeInput,
WebSiteNoticeEditDto,
WebSiteNoticeServiceProxy,
KeyValuePairOfStringString
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-web-site-notice',
  templateUrl: './create-or-edit-web-site-notice.component.html',
  styleUrls: [
	'create-or-edit-web-site-notice.component.less'
    ],
    })

    export class CreateOrEditWebSiteNoticeComponent
    extends ModalComponentBase
    implements OnInit {
    /**
    * 编辑时DTO的id
    */
    id: any ;

    entity: WebSiteNoticeEditDto = new WebSiteNoticeEditDto();

    /**
    * 构造函数，在此处配置依赖注入
    */
    constructor(
    injector: Injector,
    private _webSiteNoticeService: WebSiteNoticeServiceProxy

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
    this._webSiteNoticeService.getForEdit(this.id).subscribe(result => {
    this.entity = result.webSiteNotice;
    

                           
                           
                           });
    }

    /**
    * 保存方法,提交form表单
    */
    submitForm(): void {
    const input = new CreateOrUpdateWebSiteNoticeInput();
    input.webSiteNotice = this.entity;

    this.saving = true;

    this._webSiteNoticeService.createOrUpdate(input)
    .finally(() => (this.saving = false))
    .pipe(finalize(() => (this.saving = false)))
    .subscribe(() => {
    this.notify.success(this.l('SavedSuccessfully'));
    this.success(true);
    });
    }
    }
