import { Component, OnInit, Input, Injector, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ReuseTabService } from '@delon/abc';
import {
  YSLogDataAlertObjectListDto,
  YSLogDataSetObjectServiceProxy,
  YSLogDataAlertObjectServiceProxy,
} from '@shared/service-proxies/api-service-proxies';

@Component({
  selector: 'app-alert-rule-config',
  templateUrl: './alert-rule-config.component.html',
  styles: [],
})
export class AlertRuleConfigComponent extends AppComponentBase implements OnInit {
  loading: boolean = false;
  isDetail: boolean = undefined;
  alertId: number = null;
  showPage: boolean = false;
  errorIsVisible: boolean = false;
  alertObject: YSLogDataAlertObjectListDto = new YSLogDataAlertObjectListDto();
  // 错误信息
  errorData = [];

  editDataForm: FormGroup;

  dynamicForm: FormGroup;
  formTemplate = [];

  alertChannelList = [];
  recordList = [];

  datasetList = [];
  datasetSchemaNameList = [];

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _router: Router,
    public msg: NzMessageService,
    private _activatedRoute: ActivatedRoute,
    private _reuseTabService: ReuseTabService,
    private _ySLogDataSetObjectServiceProxy: YSLogDataSetObjectServiceProxy,
    private _ySLogDataAlertObjectServiceProxy: YSLogDataAlertObjectServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.editDataForm = this.fb.group({
      id: [null],
      name: [null, [Validators.required]],
      description: [null],
      alert_event_type: [null, [Validators.required]],
      related_dataset_id: [null, [Validators.required]],
      schema_name: [null, [Validators.required]],
      detection_frequency: [null, [Validators.required]],
      threshold: [null, [Validators.required]],
      alert_reciever_email: [null, [Validators.required]],
    });
    this.dynamicForm = this.fb.group({});
    this._activatedRoute.params.subscribe((params: Params) => {
      this.isDetail = params['isDetail'] ? (params['isDetail'] === 'true' ? true : false) : undefined;
      if (this.isDetail) {
        this.editDataForm.get('alert_event_type').disable();
        this.editDataForm.get('schema_name').disable();
        this.editDataForm.get('related_dataset_id').disable();
        this.alertId = params['alertId'];
        // TODO: 根据ID获取告警信息
        this._ySLogDataAlertObjectServiceProxy.getById(this.alertId).subscribe(res => {
          this.alertObject = res;
          this.editDataForm.patchValue({
            id: this.alertId,
            name: res.name,
            description: res.description,
            alert_event_type: res.alert_event_type,
            related_dataset_id: res.related_dataset.id,
            schema_name: res.schema_name,
            detection_frequency: res.detection_frequency,
            threshold: res.threshold,
            alert_reciever_email: res.alert_reciever_email,
          });
          this.getDataSetList(res.schema_name);
          this.showPage = true;
        });
        this._reuseTabService.title = this.l('告警任务详情');
      } else {
        this._reuseTabService.title = this.l('创建告警任务');
        this.getDataSetList();
        this.showPage = true;
      }
    });

    this.getDataSetSchemaNameList();
  }
  // 设置pagetitle
  ngAfterViewInit(): void {
    if (this.isDetail) {
      this.titleSrvice.setTitle(this.l('告警任务详情'));
    } else {
      this.titleSrvice.setTitle(this.l('创建告警任务'));
    }
  }
  // 获取数据集
  getDataSetList(name?) {
    this._ySLogDataSetObjectServiceProxy
      .getPaged(name ? name : undefined, undefined, undefined, undefined, undefined)
      .subscribe(result => {
        this.datasetList = result.items;
      });
  }
  // 获取 SchemaName
  getDataSetSchemaNameList() {
    this._ySLogDataAlertObjectServiceProxy.getForEdit(undefined).subscribe(result => {
      this.datasetSchemaNameList = result.schema_name_enum;
    });
  }
  // 显示弹出框
  showModal(id) {
    this.errorIsVisible = true;
    this._ySLogDataAlertObjectServiceProxy.getErrorList(id).subscribe(res => {
      res.map((item, index) => {
        let tempString = '时间: ' + item.detectionTime + '\r\n' + '日志: ' + '\r\n' + item.log;
        this.errorData.push(`=====================No.${index + 1}=====================\r\n` + tempString);
      });
    });
  }
  handleCancel(): void {
    this.errorIsVisible = false;
  }
  // 返回
  back() {
    this._router.navigate(['app/alert-management/alert-rule']);
  }
  // 校验动态表单
  validatorForm(form, formData) {
    for (const i in form.controls) {
      form.controls[i].markAsDirty();
      form.controls[i].updateValueAndValidity();
    }
    if (formData.length > 0) {
      return form.valid;
    } else {
      return false;
    }
  }
  // 保存
  save() {
    //let formValid = this.validatorForm(this.dynamicForm, this.formTemplate);
    for (const i in this.editDataForm.controls) {
      this.editDataForm.controls[i].markAsDirty();
      this.editDataForm.controls[i].updateValueAndValidity();
    }
    if (this.isDetail) {
      this.editDataForm.value.related_dataset_id = this.alertObject.related_dataset.id;
      this.editDataForm.value.schema_name = this.alertObject.schema_name;
    }
    // if (this.editDataForm.valid && formValid && formValid) {
    if (this.editDataForm.valid) {
      console.log(this.editDataForm.value);
      console.log(this.dynamicForm.value);
      let body: any = {
        alert: this.editDataForm.value,
      };
      this._ySLogDataAlertObjectServiceProxy.createOrUpdate(body).subscribe(() => {
        this.notify.success(this.l('保存成功'));
        this._router.navigate(['app/alert-management/alert-rule']);
      });
    }
  }
}
