import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

@Component({
  selector: 'app-agg-module',
  templateUrl: './agg-module.component.html',
  styleUrls: ['./agg-module.component.less'],
})
export class AggModuleComponent implements OnInit {
  @ViewChild('metric', { static: false }) metric;
  @ViewChild('bucket', { static: false }) bucket;
  @Input() keywordsMapTransFormLabelAndValue;
  @Input() chartSetting;
  @Input() aggData;

  isCollapsed = false;

  aggregationOption: any;
  metricsList: any;
  // 编辑模式时获取到的保存数据
  metricsData = {
    isEdit: false,
    data: [],
  };
  bucketsData = {
    isEdit: false,
    data: [],
  };

  constructor(private _AggregationDataService: DataAnalyzeService) {}

  ngOnInit() {
    // console.log(this.keywordsMapTransFormLabelAndValue);
    if (this.aggData.isEdit) {
      this.metricsData = {
        isEdit: this.aggData.isEdit,
        data: this.aggData.data.metric,
      };
      this.bucketsData = {
        isEdit: this.aggData.isEdit,
        data: this.aggData.data.bucket,
      };
    }
  }

  setMetricsList(e) {
    this._AggregationDataService.setMetricsList(this.metric.buildMetricsOption());
  }
  // 构建 aggregation 配置数据
  buildAggOption() {
    const tempObj = {
      metric: this.metric.buildMetricsOption(),
      bucket: this.bucket.buildBucketsOption(),
    };
    this.aggregationOption = tempObj;
    return this.aggregationOption;
  }
}
