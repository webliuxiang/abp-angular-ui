import { Component, OnInit, Input, Injector, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import {
  YSLogDataSetObjectListDto,
  YSLogDataSetObjectServiceProxy,
  YSLogSearchObjectListDto,
  YSLogSearchObjectServiceProxy,
} from '@shared/service-proxies/api-service-proxies';

// import moment from 'moment';
// import datemath from '@elastic/datemath';
import stringify from 'json-stable-stringify';

@Component({
  selector: 'app-discover-data',
  templateUrl: './discover-data.component.html',
  styleUrls: ['./discover-data.component.less'],
})
export class DiscoverDataComponent extends AppComponentBase implements OnInit {
  @ViewChild('dateTimeForm', { static: false }) dateTimeForm;
  @ViewChild('query', { static: false }) query;
  loading = false;
  isEdit: boolean = undefined;
  currentDatasources: YSLogDataSetObjectListDto = undefined;
  keywordsList = [];
  keywordsMap = [];
  keywordsMapTransFormLabelAndValue = [];
  chartSetting = {
    chartType: null,
    dataSource: {
      id: null,
      name: null,
    },
  };
  isVisible = false;
  saveChartForm: FormGroup;
  // 编辑模式解析数据到UI控件
  discoverObject: YSLogSearchObjectListDto = undefined;
  responseData = {
    dateTimeData: {
      data: [],
      isEdit: false,
    },
    queryData: {
      data: {},
      isEdit: false,
    },
    aggData: {
      data: [],
      isEdit: false,
    },
  };
  showPage = false;
  showSourceData = false;

  keywordsTimeDate = false;
  resData = [];
  // 没有@timestamp字段和_source字段的keywordsMap

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _dataSetService: YSLogDataSetObjectServiceProxy,
    private _YSLogSearchObjectServiceProxy: YSLogSearchObjectServiceProxy,
    private _noti: NzNotificationService,
    public msg: NzMessageService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _reuseTabService: ReuseTabService,
  ) {
    super(injector);
  }

  ngOnInit() {
    this._activatedRoute.params.subscribe((params: Params) => {
      this.isEdit = params.isEdit ? (params.isEdit === 'true' ? true : false) : undefined;
      if (this.isEdit) {
        this._reuseTabService.title = this.l('编辑数据检索');
        this.getDiscoverConfigById(params);
      } else {
        this._reuseTabService.title = this.l('创建数据检索');
        this.showPage = true;
        this.chartSetting.chartType = params.type;
        this.chartSetting.dataSource.id = params.id;
        this.getDatasourcesById(this.chartSetting.dataSource.id);
        this.getKeywordsMap(this.chartSetting);
      }
    });
    this.saveChartForm = this.fb.group({
      name: [null, [Validators.required]],
      desc: [null],
    });
  }
  // 设置pagetitle
  ngAfterViewInit(): void {
    if (this.isEdit) {
      this.titleSrvice.setTitle(this.l('编辑数据检索'));
    } else {
      this.titleSrvice.setTitle(this.l('创建数据检索'));
    }
    // 初始化请求数据
    // this.query.query();
  }
  // 返回数据可视化页面
  backDiscoverList(): void {
    this._router.navigate(['app/data-analyze/discover']);
  }

  // 获取数据源
  getDatasourcesById(datasourceId) {
    this._dataSetService.getById(datasourceId).subscribe(
      (result: any) => {
        this.currentDatasources = result;
        this.chartSetting.dataSource.name = result.name;
      },
      err => {
        this.currentDatasources = undefined;
        console.log(err);
        this._noti.warning('获取数据源失败', '');
      },
    );
  }

  // 获取查询字段
  getKeywordsMap(datasourceId) {
    this.loading = true;
    this._dataSetService.getMapping(datasourceId.dataSource.id).subscribe(
      (res: any) => {
        const arrTemp = [];
        for (const key in res) {
          if (Object.prototype.hasOwnProperty.call(res, key)) {
            const element = res[key];
            arrTemp.push({
              name: key,
              type: element,
            });
          }
        }
        this.keywordsMap = arrTemp;
        this.keywordsMapTransFormLabelAndValue = this.keywordsMap.map(item => {
          item.label = item.name;
          item.value = item.name;
          return item;
        });
        this.keywordsMapTransFormLabelAndValue.map(item => {
          if (item.type !== 'keyword') {
            this.keywordsList.push(item);
          }
        });
        this.loading = false;
      },
      err => {
        this.loading = false;
        this._noti.warning('未找到可查询字段', '请检查该数据源的配置');
        this.keywordsMap = [];
        this.keywordsMapTransFormLabelAndValue = [];
      },
    );
  }

  /**
   * @class query
   * { type: 'term', field: '', value: '', }
   * { type: 'exists', field: '', },
   * { type: 'wildcard', field: '', value: '', },
   * { type: 'numeric_range', field: '', gte: '', lte: '', },
   * { type: 'date_range', field: '', gte: '', lte: '', },
   * { type: 'match', field: '', query: '', },
   * { type: 'query_string', field: '', query: '', },
   */

  /**
   * 查询ES
   * @param this.chartSetting 基础配置
   * @param queryData query 模块
   * @param this.dateTimeForm.dateTimeForm.value.datePicker 时间模块

   */
  search(queryData) {
    const objTemp = this.buildDiscoverObject(queryData);

    const discoverObjectInput: any = {
      ysLogSearchObject: objTemp,
    };
    this._YSLogSearchObjectServiceProxy.performSearchByDefinition(discoverObjectInput).subscribe(res => {
      this.notify.success(this.l('SuccessfullyRequest'));
      console.log(res);
      this.showSourceData = true;
      this.resData = this.formatResData(res);

      // let chartData = {
      //   responseData: result,
      //   originData: objTemp,
      // };
      // this._aggregationDataService.sendChartMessage(chartData);
    });
  }

  // 编辑状态根据id获取数据检索配置
  getDiscoverConfigById(params) {
    this._YSLogSearchObjectServiceProxy.getById(params.discoverId).subscribe(result => {
      console.log(result);
      this.chartSetting.dataSource.id = result.dataset_id;
      this.discoverObject = result;
      this.getDatasourcesById(this.chartSetting.dataSource.id);
      this.getKeywordsMap(this.chartSetting);
      // 时间
      this.responseData.dateTimeData = {
        data: [
          this.discoverObject.query.default_date_range.gte,
          this.discoverObject.query.default_date_range.lte,
          this.discoverObject.query.default_date_range.date_type,
        ],
        isEdit: this.isEdit,
      };
      // query
      // console.log(this.discoverObject.query.must);

      this.responseData.queryData = {
        data: this.discoverObject.query,
        isEdit: this.isEdit,
      };
      this.showPage = true;
    });
  }

  // 格式化响应数据
  formatResData(resData) {
    const data = [];
    resData.hits.map(hit => {
      const h = hit;
      if (h['@timestamp'] !== undefined) {
        this.keywordsTimeDate = true;
      } else {
        this.keywordsTimeDate = false;
      }
      data.push({
        // TODO: 如果没有时间字段
        timestamp: h['@timestamp'],
        originData: h,
        _source: this.flat(h, false).sort(function(a, b) {
          const nameA = a.key;
          const nameB = b.key;
          if (nameA < nameB) {
            return -1;
          }
          if (nameA > nameB) {
            return 1;
          }
          return 0;
        }),
        _raw: this.flat(h, true)[0],
        _JSONValue: stringify(h, { space: '    ' }),
      });
    });
    return data;
  }

  // 递归 平铺字段
  private flat = (dataOrigin, raw: boolean) => {
    const result = [];
    const rawResult = [{}];
    /**
     *
     * @param data 递归时的 value
     * @param arr 递归时的 key
     */
    const recursion = function(data, arr) {
      if (typeof data === 'object' && !Array.isArray(data)) {
        for (const key in data) {
          if (data.hasOwnProperty(key)) {
            const element = data[key];
            arr.push(key);
            if (typeof element === 'object') {
              recursion(element, arr);
            } else {
              result.push({ key: arr.join('.'), value: element, type: 'string' });
              rawResult[0][arr.join('.')] = element;
            }
            arr.pop();
          }
        }
      } else {
        if (typeof data[0] === 'string') {
          result.push({
            key: arr[0],
            value: data.join(';'),
            type: 'array',
          });
          rawResult[0][arr] = data.join(';');
        } else {
          if (typeof data[0] === 'object' && !Array.isArray(data[0])) {
            const tempArr = [];
            data.map(item => {
              for (const key in item) {
                if (item.hasOwnProperty(key)) {
                  if (tempArr.indexOf(key) === -1) {
                    tempArr.push(key);
                  }
                }
              }
            });
            tempArr.map(itemKey => {
              const tempObj = {
                key: itemKey,
                value: [],
              };
              data.map(item => {
                if (typeof item[itemKey] === 'string') {
                  tempObj.value.push(item[itemKey]);
                } else {
                  tempObj.value.push(JSON.stringify(item[itemKey]));
                }
              });
              result.push({
                parentName: arr + '.',
                key: arr + '.' + tempObj.key,
                value: tempObj.value.join(';'),
                type: 'object',
              });
              rawResult[0][arr + '.' + tempObj.key] = tempObj.value.join(';');
            });
          }
        }
      }
    };
    recursion(dataOrigin, []);
    return raw ? rawResult : result;
  }

  // 打开保存弹出框
  showSaveModal(edit) {
    if (!edit) { return; }
    this.isVisible = true;
    this.saveChartForm.patchValue({
      name: this.discoverObject ? this.discoverObject.name : `新建数据检索`,
    });
    this.saveChartForm.patchValue({
      desc: this.discoverObject ? this.discoverObject.desc : '',
    });
  }

  handleCancel(): void {
    this.isVisible = false;
  }
  // 保存
  save() {
    const objTemp = this.buildDiscoverObject(this.query.save());
    objTemp.name = this.saveChartForm.value.name;
    objTemp.desc = this.saveChartForm.value.desc;
    objTemp.id = this.discoverObject ? this.discoverObject.id : null;

    const discoverObjectInput: any = {
      ysLogSearchObject: objTemp,
    };
    this._YSLogSearchObjectServiceProxy.createOrUpdate(discoverObjectInput).subscribe(res => {
      this.notify.success(this.l('SavedSuccessfully'));
      this.isVisible = false;
      this._router.navigate(['app/data-analyze/discover']);
    });
  }

  // 构建配置数据
  buildDiscoverObject(queryData) {
    // console.log(queryData);
    // console.log(this.aggregation.buildAggOption());
    const discoverTemp = {
      id: null,
      name: `新建数据检索`,
      desc: this.discoverObject ? this.discoverObject.desc : '',
      dataset_id: this.discoverObject ? this.discoverObject.dataset_id : this.chartSetting.dataSource.id,
      query: {
        default_query: queryData.queryString === '' ? '*' : queryData.queryString,
        default_date_range: {
          type: null,
          field: null,
          date_type: null,
          gte: null,
          lte: null,
        },
        must: [],
        must_not: [],
        should: [],
        filter: [],
        min_should_match: 0,
      },
      sort_by: 'null',
      sort_by_order: 'Ascending',
      from: 0,
      size: 0,
    };
    // 处理时间范围
    if (!this.dateTimeForm.dateTimeForm.value.removeDateTimeSet) {
      if (this.dateTimeForm.dateTimeForm.value.datePicker.length < 1) {
        this.dateTimeForm.dateTimeForm.value.datePicker = ['now-5m', 'now'];
      }
      discoverTemp.query.default_date_range = {
        type: 'date_range',
        field: '@timestamp',
        date_type: this.dateTimeForm.dateTimeForm.value.datePicker[2],
        gte: this.dateTimeForm.dateTimeForm.value.datePicker[0],
        lte: this.dateTimeForm.dateTimeForm.value.datePicker[1],
      };
    }
    // 处理筛选条件
    queryData.filterTags.map(item => {
      switch (item.queryMethod.value) {
        case 'term':
          discoverTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            value: item.queryContent,
          });
          break;
        case 'exists':
          discoverTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
          });
          break;
        case 'wildcard':
          discoverTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            value: item.queryContent,
          });
          break;
        case 'numeric_range':
          discoverTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            gte: item.queryContent[0],
            lte: item.queryContent[1],
          });
          break;
        case 'date_range':
          discoverTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            gte: item.queryContent[0],
            lte: item.queryContent[1],
          });
          break;
        case 'match':
          discoverTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            query: item.queryContent,
          });
          break;
        case 'query_string':
          discoverTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            query: item.queryContent,
          });
          break;
      }
    });

    return discoverTemp;
  }
}
