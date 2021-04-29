import { Component, OnInit, Input, ViewChildren, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-metrics-module',
  templateUrl: './metrics-module.component.html',
  styleUrls: ['./metrics-module.component.less'],
})
export class MetricsModuleComponent implements OnInit {
  @Input() chartSetting;
  @Input() keywordsMapTransFormLabelAndValue;
  @Input() metricsData;
  @ViewChild('editor', { static: false }) editor;
  @ViewChildren('metricConfigFun') metricConfigFun;
  constructor() {}
  /**
   * 配置 aggs_metrics
   * 声明变量
   * metricsAggregation: 配置 aggregation
   * metricsLabel: 配置指标别名
   * metricsSetting: 除count 外的agg_type 的fields配置
   * metricsAdvanced： 高级配置，自定义JSON
   */
  metrics_items = [];
  // metricsOptions 数据
  metricsOptions = [];
  // metric 类型数据
  metricsType = [
    {
      label: 'Count',
      value: 'count',
      dataType: null,
    },
    {
      label: 'Average',
      value: 'avg',
      dataType: ['number'],
    },
    {
      label: 'Min',
      value: 'min',
      dataType: ['date', 'number'],
    },
    {
      label: 'Max',
      value: 'max',
      dataType: ['date', 'number'],
    },
    {
      label: 'Sum',
      value: 'sum',
      dataType: ['number'],
    },
  ];
  // 默认显示，只有当图表类型为饼图和超过3个指标配置时隐藏
  showAddMetric = true;

  ngOnInit() {
    if (this.metricsData.isEdit) {
      this.metrics_items = this.metricsData.data.map(metricItem => {
        metricItem.isEdit = this.metricsData.isEdit;
        metricItem.keywordsMapTransFormLabelAndValue = this.keywordsMapTransFormLabelAndValue;
        metricItem.metricsAggregation = this.metricsType.find((item, key) => {
          return JSON.stringify(item) === JSON.stringify(metricItem.metricsAggregation);
        });
        return metricItem;
      });
    } else {
      this.addMetricFun();
    }
  }
  /**
   * 配置 aggs_metric
   * metricsTypeChange 选择 metric_type
   * addMetricFun 添加 metric
   * removeMetricFun 删除 metric
   */
  metricsTypeChange(e, item) {
    this.metrics_items.map(item => {
      item.keywordsMapTransFormLabelAndValue = this.keywordsMapTransFormLabelAndValue;
    });
  }
  addMetricFun() {
    // this.metricForm = this.fb.group({});
    const maxValue =
      this.metrics_items.length > 0
        ? this.metrics_items.reduce((pre, cur) => (pre.id > cur.id ? pre : cur)).metricsIndex
        : 0;
    const tempObj = {
      label: 'Count',
      value: 'count',
      dataType: null,
    };
    this.metrics_items.push({
      isEdit: false,
      metricItemName: maxValue + 1,
      metricsIndex: maxValue + 1,
      metricsAggregation: this.metricsType.find((item, key) => {
        return JSON.stringify(item) === JSON.stringify(tempObj);
      }),
      keywordsMapTransFormLabelAndValue: this.keywordsMapTransFormLabelAndValue,
      metricsAdvanced: '',
    });
    console.log(this.metrics_items);
  }
  removeMetricFun(e, item) {
    // 阻止默认事件
    e.stopPropagation();
    // 至少保留一个
    if (this.metrics_items.length > 1) {
      const selectIndex = this.metrics_items.findIndex(n => n.metricsIndex === item.metricsIndex);
      this.metrics_items.splice(selectIndex, 1);
    }
  }

  ngAfterViewInit() {
    // console.log(this.metricConfigFun);
    // this.editor.mode = 'javascript';
  }

  // 构建 metricsOption
  buildMetricsOption() {
    // console.log('调用 metric module buildMetricsOption');
    // console.log(this.metricConfigFun);
    this.metricsOptions = [];
    this.metricConfigFun._results.map(item => {
      // item.buildMetricItemOption();
      // console.log(item.buildMetricItemOption());
      this.metricsOptions.push(item.buildMetricItemOption());
    });
    return this.metricsOptions;
  }
}
