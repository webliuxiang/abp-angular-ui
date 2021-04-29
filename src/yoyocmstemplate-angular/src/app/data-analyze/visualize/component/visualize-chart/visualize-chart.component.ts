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
  YSLogVisualizeObjectListDto,
  YSLogVisualizeObjectServiceProxy,
} from '@shared/service-proxies/api-service-proxies';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

import * as moment from 'moment';
import datemath from '@elastic/datemath';

@Component({
  selector: 'app-visualize-chart',
  templateUrl: './visualize-chart.component.html',
  styleUrls: ['./visualize-chart.component.less'],
})
export class VisualizeChartComponent extends AppComponentBase implements OnInit {
  @ViewChild('aggregation', { static: false }) aggregation;
  @ViewChild('dateTimeForm', { static: false }) dateTimeForm;
  @ViewChild('query', { static: false }) query;

  loading = false;
  isEdit: boolean = undefined;
  visualizeId = null;
  currentDatasources: YSLogDataSetObjectListDto = undefined;
  keywordsList = [];
  keywordsMap = [];
  keywordsMapTransFormLabelAndValue = [];
  chartSetting = {
    chartName: null,
    chartType: null,
    dataSource: {
      id: null,
      name: null,
    },
  };
  metricsObjectValid = true;
  bucketsObjectValid = true;
  isVisible = false;
  saveChartForm: FormGroup;
  // 编辑模式解析数据到UI控件
  visualizeObject: YSLogVisualizeObjectListDto = undefined;
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
  // 没有@timestamp字段和_source字段的keywordsMap

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _dataSetService: YSLogDataSetObjectServiceProxy,
    private _YSLogVisualizeObjectService: YSLogVisualizeObjectServiceProxy,
    private _noti: NzNotificationService,
    public msg: NzMessageService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _reuseTabService: ReuseTabService,
    private _aggregationDataService: DataAnalyzeService,
  ) {
    super(injector);
  }

  ngOnInit() {
    this._activatedRoute.params.subscribe((params: Params) => {
      this.isEdit = params.isEdit ? (params.isEdit === 'true' ? true : false) : undefined;
      this.visualizeId = params.visualizeId;
      if (this.isEdit) {
        this._reuseTabService.title = this.l('EditVisualize');
        this.fatchDataForVisualizeId();
      } else {
        this._reuseTabService.title = this.l('CreateVisualize');
        this.showPage = true;
        this.chartSetting.chartName = `新建${this.generateName(params.type)}`;
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
      this.titleSrvice.setTitle(this.l('EditVisualize'));
    } else {
      this.titleSrvice.setTitle(this.l('CreateVisualize'));
    }
  }
  // 返回数据可视化页面
  backVisualizeList(): void {
    this._router.navigate(['app/data-analyze/visualize']);
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

  // 新建可视化时生成的 name
  generateName(e) {
    let name: string;
    switch (e) {
      case 'lineChart':
        name = '折线图';
        break;
      case 'pieChart':
        name = '饼图';
        break;
      case 'barChart':
        name = '柱状图';
        break;
      case 'tableChart':
        name = '表格';
        break;
      case 'areaChart':
        name = '面积图';
        break;
    }
    return name;
  }

  // 设置时间分段
  setDateTimePart(time) {
    let result: string;
    if (time < 100) {
      result = '1s';
    } else if (time / 5 < 100) {
      result = '5s';
    } else if (time / 30 < 100) {
      result = '30s';
    } else if (time / 60 < 100) {
      result = '1m';
    } else if (time / 60 / 5 < 100) {
      result = '5m';
    } else if (time / 60 / 30 < 100) {
      result = '30m';
    } else if (time / 60 / 60 < 100) {
      result = '1h';
    } else if (time / 60 / 60 / 24 < 100) {
      result = '1d';
    } else if (time / 60 / 60 / 24 / 7 < 100) {
      result = '7d';
    } else if (time / 60 / 60 / 24 / 30 < 100) {
      result = '30d';
    }
    return result;
  }

  // 根据 ID 请求数据可视化配置数据
  fatchDataForVisualizeId() {
    this._YSLogVisualizeObjectService.getById(this.visualizeId).subscribe(result => {
      this.notify.success(this.l('SuccessfullyRequest'));
      // console.log(result);
      this.chartSetting.chartName = result.name;
      this.chartSetting.chartType = result.chart_type;
      this.chartSetting.dataSource.id = result.dataset_id;
      this.visualizeObject = result;
      this.getDatasourcesById(this.chartSetting.dataSource.id);
      this.getKeywordsMap(this.chartSetting);
      // 时间
      this.responseData.dateTimeData = {
        data: [
          this.visualizeObject.query.default_date_range.gte,
          this.visualizeObject.query.default_date_range.lte,
          this.visualizeObject.query.default_date_range.date_type,
        ],
        isEdit: this.isEdit,
      };
      // query
      this.responseData.queryData = {
        data: this.visualizeObject.query,
        isEdit: this.isEdit,
      };
      // agg
      this.responseData.aggData = {
        data: JSON.parse(this.visualizeObject.chart_opts.originAgg),
        isEdit: this.isEdit,
      };
      this._aggregationDataService.sendEditStatus(this.isEdit);
      this.showPage = true;
    });
  }

  // TODO: 当编辑可视化配置时，只需要把获取到的可视化配置数据分别赋值给：
  // console.log(this.chartSetting);
  // console.log(this.dateTimeForm.dateTimeForm.value);
  // console.log(queryData);
  // console.log(this.aggregation.buildAggOption());

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
   * @class metric
   * { type: 'count', },
   * { type: 'avg', field: '', },
   * { type: 'sum', field: '', },
   * { type: 'max', field: '', },
   * { type: 'min', field: '', },
   * { type: 'cardinal', field: '', },  unique count
   * { type: 'med_abs_dev', field: '', },  median
   * { type: 'ext_stats', field: '', },
   * { type: 'stats', field: '', },
   * { type: 'percentile_ranks', field: '', values: [], },
   * { type: 'percentiles', field: '', percents: [], },
   */
  /**
   * @class bucket
   * { type: 'date_histogram', field: '', calendar_interval: '', },
   * { type: 'filters', query_string_filters: [ { name: 'query_string', }, ], },
   * { type: 'terms', field: '', order_by_key: '_count<文档数量>/_key<字段内容>/metricName<自定义指标>',order_direction:'',size:'',},  // 自动排除为type为count的metricItem
   * { type: 'histogram', field: '', interval: 0, min_ext_bounds: 0, max_ext_bounds: 0, },
   * { type: 'ip_range', ranges: [ { from: '', to: '', mask: [], }, ], },
   * { type: 'numeric_range', field: '', ranges: [ { from: '', to: '', }, ], },
   */

  /**
   * 查询ES
   * @param this.chartSetting 基础配置
   * @param queryData query 模块
   * @param this.dateTimeForm.dateTimeForm.value.datePicker 时间模块
   * @param this.aggregation.buildAggOption() aggregation 模块
   */
  search(queryData) {
    // console.log(this.chartSetting);
    // console.log(this.dateTimeForm.dateTimeForm.value);
    // console.log(queryData);
    // console.log(this.aggregation.buildAggOption());
    const objTemp = this.buildVisualizeObject(queryData);

    const visualizeObjectInput: any = {
      visualize: objTemp,
    };
    if (this.bucketsObjectValid && this.metricsObjectValid) {
      // console.log(visualizeObjectInput);
      this._YSLogVisualizeObjectService.performSearchByDefinition(visualizeObjectInput).subscribe(res => {
        this.notify.success(this.l('SuccessfullyRequest'));
        // console.log(res);
        // ArrayBuffer 转为 json
        // let result = JSON.parse(JSON.stringify(res)).result;
        const chartData = {
          responseData: res,
          originData: objTemp,
        };
        this._aggregationDataService.sendChartMessage(chartData);
      });
    }
  }

  // 打开保存弹出框
  showSaveModal(edit) {
    if (!edit) { return; }
    this.isVisible = true;
    this.saveChartForm.patchValue({
      name: this.chartSetting.chartName,
    });
    this.saveChartForm.patchValue({
      desc: this.visualizeObject ? this.visualizeObject.desc : '',
    });
  }

  handleCancel(): void {
    this.isVisible = false;
  }
  // 保存
  save() {
    // console.log(this.query.save());
    // console.log(this.saveChartForm);
    const objTemp = this.buildVisualizeObject(this.query.save());
    objTemp.name = this.saveChartForm.value.name;
    objTemp.desc = this.saveChartForm.value.desc;
    objTemp.id = this.visualizeObject ? this.visualizeObject.id : null;
    const visualizeObjectInput: any = {
      visualize: objTemp,
    };
    if (this.bucketsObjectValid && this.metricsObjectValid) {
      console.log(visualizeObjectInput);
      this._YSLogVisualizeObjectService.createOrUpdate(visualizeObjectInput).subscribe(res => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.isVisible = false;
        this.chartSetting.chartName = visualizeObjectInput.visualize.name;
        this._router.navigate(['app/data-analyze/visualize']);
      });
    }
  }

  // 构建配置数据
  buildVisualizeObject(queryData) {
    // console.log(queryData);
    // console.log(this.aggregation.buildAggOption());

    const visualizeTemp = {
      id: null,
      name: this.visualizeObject ? this.visualizeObject.name : this.chartSetting.chartName,
      desc: this.visualizeObject ? this.visualizeObject.desc : '',
      dataset_id: this.visualizeObject ? this.visualizeObject.dataset_id : this.chartSetting.dataSource.id,
      chart_type: this.visualizeObject ? this.visualizeObject.chart_type : this.chartSetting.chartType,
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
      aggregation: {
        metrics: [],
        buckets: [],
      },
      chart_opts: {
        originAgg: JSON.stringify(this.aggregation.buildAggOption()),
      },
    };
    // 处理时间范围
    if (!this.dateTimeForm.dateTimeForm.value.removeDateTimeSet) {
      if (this.dateTimeForm.dateTimeForm.value.datePicker.length < 1) {
        this.dateTimeForm.dateTimeForm.value.datePicker = [
          moment(datemath.parse('now-5m')).toISOString(),
          moment(datemath.parse('now')).toISOString(),
        ];
      }
      visualizeTemp.query.default_date_range = {
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
          visualizeTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            value: item.queryContent,
          });
          break;
        case 'exists':
          visualizeTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
          });
          break;
        case 'wildcard':
          visualizeTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            value: item.queryContent,
          });
          break;
        case 'numeric_range':
          visualizeTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            gte: item.queryContent[0],
            lte: item.queryContent[1],
          });
          break;
        case 'date_range':
          visualizeTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            gte: item.queryContent[0],
            lte: item.queryContent[1],
          });
          break;
        case 'match':
          visualizeTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            query: item.queryContent,
          });
          break;
        case 'query_string':
          visualizeTemp.query[item.relation].push({
            type: item.queryMethod.value,
            field: item.queryField.value,
            query: item.queryContent,
          });
          break;
      }
    });
    // 处理 Aggregation 数据
    // metric 数据
    this.aggregation.buildAggOption().metric.map((item, key) => {
      if (!item) {
        this._noti.error('请检查 Metrics 配置！', '');
        this.metricsObjectValid = false;
        return;
      }
      if (item.metricsAggregation.value === 'count') {
        visualizeTemp.aggregation.metrics.push({
          name: item.metricItemName,
          label: item.metricsSetting.metricsLabel,
          type: item.metricsAggregation.value,
        });
      } else if (item.metricsAggregation.value === 'percentile_ranks') {
        // TODO:
      } else if (item.metricsAggregation.value === 'percentiles') {
        // TODO:
      } else {
        visualizeTemp.aggregation.metrics.push({
          name: item.metricItemName,
          label: item.metricsSetting.metricsLabel,
          type: item.metricsAggregation.value,
          field: item.metricsSetting.fields.value,
        });
      }
      const metricsAdvanced = new Function('return ' + item.metricsAdvanced)();
      for (const metricsAdvancedKey in metricsAdvanced) {
        if (Object.prototype.hasOwnProperty.call(metricsAdvanced, metricsAdvancedKey)) {
          const element = metricsAdvanced[metricsAdvancedKey];
          if (!Object.keys(visualizeTemp.aggregation.metrics[key]).includes(metricsAdvancedKey)) {
            visualizeTemp.aggregation.metrics[key][metricsAdvancedKey] = element;
          }
        }
      }
      // console.log(visualizeTemp.aggregation.metrics[key]);
    });
    // bucket 数据
    // console.log(this.aggregation.buildAggOption());
    this.aggregation.buildAggOption().bucket.map((item, key) => {
      if (!item || !item.bucketsAggregation.hasOwnProperty('value')) {
        this._noti.error('请检查 Buckets 配置！', '');
        this.bucketsObjectValid = false;
        return;
      }
      switch (item.bucketsAggregation.value) {
        case 'date_histogram':
          const strTemp =
            (new Date(moment(datemath.parse(visualizeTemp.query.default_date_range.lte)).toISOString()).getTime() -
              new Date(moment(datemath.parse(visualizeTemp.query.default_date_range.gte)).toISOString()).getTime()) /
            1000;
          // console.log(this.setDateTimePart(strTemp));
          // console.log(item.bucketsSetting.min_Interval);
          visualizeTemp.aggregation.buckets.push({
            name: item.bucketsName,
            label: item.bucketsSetting.bucketsLabel,
            type: item.bucketsAggregation.value,
            field: item.bucketsSetting.fields.value,
            calendar_interval:
              item.bucketsSetting.min_Interval[0] === 'auto'
                ? this.setDateTimePart(strTemp)
                : item.bucketsSetting.min_Interval[0],
          });
          break;
        case 'filters':
          const objTemp = {};
          item.bucketsSetting.map(item => {
            objTemp[Object.keys(item)[0]] = item[Object.keys(item)[0]];
          });
          visualizeTemp.aggregation.buckets.push({
            name: item.bucketsName,
            type: item.bucketsAggregation.value,
            query_string_filters: objTemp,
          });
          break;
        case 'terms':
          let booleanTemp = false;
          if (item.bucketsSetting.order_by === '_count' || item.bucketsSetting.order_by === '_key') {
            booleanTemp = true;
          } else {
            this.aggregation.buildAggOption().metric.map(metricsItem => {
              if (metricsItem.metricItemName === item.bucketsSetting.order_by) {
                booleanTemp = true;
              }
            });
          }
          if (booleanTemp) {
            visualizeTemp.aggregation.buckets.push({
              name: item.bucketsName,
              label: item.bucketsSetting.bucketsLabel,
              type: item.bucketsAggregation.value,
              field: item.bucketsSetting.fields.value,
              order_by_key: item.bucketsSetting.order_by,
              order_direction: item.bucketsSetting.order,
              size: item.bucketsSetting.size,
            });
          } else {
            this._aggregationDataService.sub.next(booleanTemp);
          }

          break;
        case 'histogram':
          // TODO:
          break;
        case 'ip_range':
          // TODO:
          break;
        case 'numeric_range':
          // TODO:
          break;
      }

      const bucketsAdvanced = new Function('return ' + item.bucketsAdvanced)();
      for (const bucketsAdvancedKey in bucketsAdvanced) {
        if (Object.prototype.hasOwnProperty.call(bucketsAdvanced, bucketsAdvancedKey)) {
          const element = bucketsAdvanced[bucketsAdvancedKey];
          if (!Object.keys(visualizeTemp.aggregation.buckets[key]).includes(bucketsAdvancedKey)) {
            visualizeTemp.aggregation.buckets[key][bucketsAdvancedKey] = element;
          }
        }
      }
      // console.log(visualizeTemp.aggregation.buckets[key]);
    });
    return visualizeTemp;
  }
}
