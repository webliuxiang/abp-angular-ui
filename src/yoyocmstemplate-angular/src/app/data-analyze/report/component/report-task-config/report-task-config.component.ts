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
  YSLogDataAnalyzeObjectListDto,
  FileServiceProxy,
  YSLogDataSetObjectServiceProxy,
  YSLogDataAnalyzeObjectServiceProxy,
} from '@shared/service-proxies/api-service-proxies';
import { AppConsts } from 'abpPro/AppConsts';

@Component({
  selector: 'app-report-task-config',
  templateUrl: './report-task-config.component.html',
  styles: [],
})
export class ReportTaskConfigComponent extends AppComponentBase implements OnInit {
  loading = false;
  isDetail: boolean = undefined;
  reportId: number = null;
  showPage = false;
  isVisible = false;
  reportObject: YSLogDataAnalyzeObjectListDto = new YSLogDataAnalyzeObjectListDto();
  // 错误信息
  errorData = [];

  editDataForm: FormGroup;

  dynamicForm: FormGroup;
  formTemplate = [];
  // 数据类型列表
  schemaNameList = [];
  // 数据集列表
  datasetList = [];
  // 时间偏移量列表
  timeOffsetList = [];
  // 运行记录
  recordList = [];

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _router: Router,
    public msg: NzMessageService,
    private _activatedRoute: ActivatedRoute,
    private _reuseTabService: ReuseTabService,
    private _ySLogDataSetObjectServiceProxy: YSLogDataSetObjectServiceProxy,
    private _ySLogDataAnalyzeObjectServiceProxy: YSLogDataAnalyzeObjectServiceProxy,
    private _fileServiceProxy: FileServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.editDataForm = this.fb.group({
      id: [''],
      name: [null, [Validators.required]],
      description: [''],
      schema_name: [null, [Validators.required]],
      related_dataset: [null, [Validators.required]],
      time_offset: [null, [Validators.required]],
      job_type: ['BackgroundJob', [Validators.required]],
      corn_expression: [''],
    });
    this.dynamicForm = this.fb.group({});
    this._activatedRoute.params.subscribe((params: Params) => {
      this.isDetail = params.isDetail ? (params.isDetail === 'true' ? true : false) : undefined;
      if (this.isDetail) {
        this.reportId = params.reportId;
        this.editDataForm.get('related_dataset').disable();
        this.editDataForm.get('schema_name').disable();
        this._ySLogDataAnalyzeObjectServiceProxy.getById(this.reportId).subscribe(res => {
          this.reportObject = res;
          this.editDataForm.patchValue({
            id: this.reportId,
            name: res.name,
            description: res.description,
            schema_name: res.schema_name,
            related_dataset: res.related_dataset.id,
            time_offset: res.time_offset,
            job_type: res.job_type,
            corn_expression: res.corn_expression,
          });
          this.showPage = true;
          this.getDataSetList(res.schema_name);
        });
        this._reuseTabService.title = this.l('数据分析任务详情');
      } else {
        this.getDataSetList();
        this._reuseTabService.title = this.l('创建数据分析任务');
        this.showPage = true;
      }
    });
    this.getConfigData();
  }
  // 设置pagetitle
  ngAfterViewInit(): void {
    if (this.isDetail) {
      this.titleSrvice.setTitle(this.l('数据分析任务详情'));
    } else {
      this.titleSrvice.setTitle(this.l('创建数据分析任务'));
    }
  }
  // 获取数据类型和时间偏移量配置数据
  getConfigData(): void {
    this._ySLogDataAnalyzeObjectServiceProxy.getForEdit(undefined).subscribe(res => {
      this.schemaNameList = res.schema_name_enum;
      this.timeOffsetList = res.time_offset_enum;
    });
  }
  // 获取数据集
  getDataSetList(name?) {
    this._ySLogDataSetObjectServiceProxy
      .getPaged(name ? name : undefined, undefined, undefined, undefined, undefined)
      .subscribe(result => {
        this.datasetList = result.items;
      });
  }
  // 显示弹出框
  showModal(id) {
    if (!id) { return; }
    this.isVisible = true;
    this._ySLogDataAnalyzeObjectServiceProxy.getHistoryRecords(id).subscribe(res => {
      this.recordList = res;
    });
  }
  handleCancel(): void {
    this.isVisible = false;
  }
  // 下载文件
  downLoadFile(id): void {
    if (!id) { return; }
    const body: any = {
      id: id,
    };
    this._ySLogDataAnalyzeObjectServiceProxy.generateDownloadToken(body).subscribe(resToken => {
      const url = `${AppConsts.remoteServiceBaseUrl}/api/File/DownloadReport?token=${resToken}`;
      location.href = url;
    });
  }
  // 返回
  back() {
    this._router.navigate(['app/data-analyze/report']);
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
    // let formValid = this.validatorForm(this.dynamicForm, this.formTemplate);
    for (const i in this.editDataForm.controls) {
      if (this.editDataForm.get('job_type').value === 'RecurringJob') {
        this.editDataForm.get('corn_expression').setValidators([Validators.required]);
      } else {
        this.editDataForm.get('corn_expression').clearValidators();
        this.editDataForm.patchValue({ corn_expression: '' });
      }
      this.editDataForm.controls[i].markAsDirty();
      this.editDataForm.controls[i].updateValueAndValidity();
    }
    if (this.editDataForm.value.jobType === 'BackgroundJob') {
      this.editDataForm.value.corn_expression = '';
    }
    if (this.isDetail) {
      this.editDataForm.value.related_dataset = this.reportObject.related_dataset.id;
      this.editDataForm.value.schema_name = this.reportObject.schema_name;
    }
    if (this.editDataForm.valid) {
      // console.log(this.editDataForm.value);
      const body: any = {
        ysLogDataAnalyzeObject: this.editDataForm.value,
      };

      this._ySLogDataAnalyzeObjectServiceProxy.createOrUpdate(body).subscribe(() => {
        this.notify.success(this.l('保存成功'));
        this._router.navigate(['app/data-analyze/report']);
      });
    }
  }
}
