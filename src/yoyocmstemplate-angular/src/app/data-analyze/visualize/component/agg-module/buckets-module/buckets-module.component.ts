import { Component, OnInit, Input, ViewChildren, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-buckets-module',
  templateUrl: './buckets-module.component.html',
  styleUrls: ['./buckets-module.component.less'],
})
export class BucketsModuleComponent implements OnInit {
  @Input() chartSetting;
  @Input() keywordsMapTransFormLabelAndValue;
  @Input() bucketsData;
  @Input() metric;
  @Output() setMetricsList = new EventEmitter();
  @ViewChildren('bucketConfigFun') bucketConfigFun;
  constructor(private fb: FormBuilder) {}
  /**
   * 配置 aggs_buckets
   * 声明变量
   * bucketsAggregation: 配置 aggregation
   * bucketsLabel: 配置指标别名
   * bucketsSetting: 除count 外的agg_type 的fields配置
   * bucketsAdvanced： 高级配置，自定义JSON
   */
  buckets_items = [];
  // bucketsOptions 数据
  bucketsOptions = [];

  addXaXisAbled = false;
  addSeriesCount = 0;
  addSeriesAbled = false;
  addChartAbled = false;
  // bucket 类型数据
  bucketsType = [
    {
      label: 'Date Histogram',
      value: 'date_histogram',
      dataType: ['date'],
    },
    {
      label: 'Filters',
      value: 'filters',
      dataType: null,
    },
    {
      label: 'Terms',
      value: 'terms',
      dataType: ['date', 'number', 'string'],
    },
  ];
  // 默认显示，只有当图表类型为饼图和超过个数时隐藏
  showAddBucket = true;

  ngOnInit() {
    if (this.bucketsData.isEdit) {
      this.buckets_items = this.bucketsData.data.map(bucketItem => {
        bucketItem.isEdit = this.bucketsData.isEdit;
        bucketItem.keywordsMapTransFormLabelAndValue = this.keywordsMapTransFormLabelAndValue;
        bucketItem.bucketsAggregation = this.bucketsType.find((item, key) => {
          return JSON.stringify(item) === JSON.stringify(bucketItem.bucketsAggregation);
        });
        return bucketItem;
      });
      this.buckets_items.map(item => {
        if (item.seriesType === 'xAxis') {
          this.addXaXisAbled = true;
        }
        if (item.seriesType === 'chart') {
          this.addChartAbled = true;
        }
        if (item.seriesType === 'series') {
          this.addSeriesCount += 1;
          this.addSeriesAbled = this.addSeriesCount > 3 ? true : false;
        }
      });
    }
  }
  /**
   * 配置 aggs_bucket
   * bucketsTypeChange 选择 bucket_type
   * addBucketFun 添加 bucket
   * removeBucketFun 删除 bucket
   */
  bucketsTypeChange(e, item) {
    this.buckets_items.map(item => {
      item.keywordsMapTransFormLabelAndValue = this.keywordsMapTransFormLabelAndValue;
    });
    this.setMetricsList.next();
  }
  addBucketFun(e) {
    const maxValue =
      this.buckets_items.length > 0
        ? this.buckets_items.reduce((pre, cur) => (pre.id > cur.id ? pre : cur)).bucketsIndex
        : 0;

    if (e === 'series') {
      if (this.addSeriesCount > 3) { return; }
      // 按系列拆分
      this.buckets_items.push({
        isEdit: false,
        isChart: false,
        seriesType: 'series',
        bucketsIndex: maxValue + 1,
        bucketsName: maxValue + 1,
        bucketsAggregation: '',
        bucketsAdvanced: '',
        keywordsMapTransFormLabelAndValue: this.keywordsMapTransFormLabelAndValue,
      });
    } else if (e === 'chart') {
      if (this.addChartAbled) { return; }
      // 按图表拆分
      this.buckets_items.push({
        isEdit: false,
        isChart: false,
        seriesType: 'chart',
        bucketsIndex: maxValue + 1,
        bucketsName: maxValue + 1,
        bucketsAggregation: '',
        bucketsAdvanced: '',
        keywordsMapTransFormLabelAndValue: this.keywordsMapTransFormLabelAndValue,
      });
    } else {
      if (this.addXaXisAbled) { return; }
      this.buckets_items.push({
        isEdit: false,
        isChart: false,
        seriesType: 'xAxis',
        bucketsIndex: maxValue + 1,
        bucketsName: maxValue + 1,
        bucketsAggregation: '',
        bucketsAdvanced: '',
        keywordsMapTransFormLabelAndValue: this.keywordsMapTransFormLabelAndValue,
      });
    }
    this.buckets_items.map(item => {
      if (item.seriesType === 'xAxis') {
        this.addXaXisAbled = true;
      }
      if (item.seriesType === 'chart') {
        this.addChartAbled = true;
      }
      if (item.seriesType === 'series') {
        this.addSeriesCount += 1;
        this.addSeriesAbled = this.addSeriesCount > 3 ? true : false;
      }
    });
  }
  removebucketFun(e, item) {
    // 阻止默认事件
    e.stopPropagation();
    const selectIndex = this.buckets_items.findIndex(n => n.bucketsIndex === item.bucketsIndex);
    this.buckets_items.splice(selectIndex, 1);
    this.addXaXisAbled = false;
    this.addSeriesCount = 0;
    this.addSeriesAbled = false;
    this.addChartAbled = false;
    this.buckets_items.map(item => {
      if (item.seriesType === 'xAxis') {
        this.addXaXisAbled = true;
      }
      if (item.seriesType === 'chart') {
        this.addChartAbled = true;
      }
      if (item.seriesType === 'series') {
        this.addSeriesCount += 1;
        this.addSeriesAbled = this.addSeriesCount > 3 ? true : false;
      }
    });
  }
  ngAfterViewInit() {
    // console.log(this.bucketConfigFun);
  }
  // 构建 bucketsOptions
  buildBucketsOption() {
    this.bucketsOptions = [];
    this.bucketConfigFun._results.map(item => {
      this.bucketsOptions.push(item.buildBucketItemOption());
    });
    return this.bucketsOptions;
  }
}
