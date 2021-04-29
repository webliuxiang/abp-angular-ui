import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

@Component({
  selector: 'app-terms-module',
  templateUrl: './terms-module.component.html',
  styleUrls: ['./terms-module.component.less'],
})
export class TermsModuleComponent implements OnInit {
  @Input() bucketData;
  @Input() metric;
  termsForm: FormGroup;
  metricsList = [];
  booleanTemp: any;
  dateKeywordMap = [];
  numberKeywordMap = [];
  stringKeywordMap = [];
  keywordMap = [];
  constructor(private fb: FormBuilder, private _aggregationDataService: DataAnalyzeService) {}

  ngOnInit() {
    this.termsForm = this.fb.group({
      fields: ['', [Validators.required]],
      order_by: ['_count', [Validators.required]],
      order: ['desc', [Validators.required]],
      size: ['5', [Validators.required]],
      bucketsLabel: [''],
    });
    setTimeout(() => {
      this.metricsList = [];
      this.metric.buildMetricsOption().map(item => {
        if (item.metricsAggregation.label !== 'Count') {
          const objTemp = {
            label: `Metric: ${item.metricsAggregation.label} ${item.metricsSetting.fields.label}`,
            value: item.metricItemName,
          };
          this.metricsList.push(objTemp);
        }
      });
      this.termsForm.patchValue({ order_by: this.metricsList.length > 0 ? this.metricsList[0].value : '_count' });
    });
    this._aggregationDataService.sub.subscribe(res => {
      this.booleanTemp = res;
      if (!res) {
        this.termsForm.patchValue({ order_by: '_count' });
      }
    });
    // console.log(this.bucketData);
    this.bucketData.keywordsMapTransFormLabelAndValue.find(item => {
      if (item.type === 'date') {
        this.dateKeywordMap.push(item);
      }
      if (
        item.type === 'long' ||
        item.type === 'short' ||
        item.type === 'byte' ||
        item.type === 'integer' ||
        item.type === 'double' ||
        item.type === 'float' ||
        item.type === 'half_float' ||
        item.type === 'scaled_float'
      ) {
        this.numberKeywordMap.push(item);
      }
      if (item.type === 'string' || item.type === 'text' || item.type === 'keyword' || item.type === 'keyword') {
        this.stringKeywordMap.push(item);
      }
      if (item.type === 'keyword') {
        this.keywordMap.push(item);
      }
    });
    if (this.bucketData.isEdit) {
      setTimeout(() => {
        this.metricsList = [];
        this.metric.buildMetricsOption().map(item => {
          if (item.metricsAggregation.label !== 'Count') {
            const objTemp = {
              label: `Metric: ${item.metricsAggregation.label} ${item.metricsSetting.fields.label}`,
              value: item.metricItemName,
            };
            this.metricsList.push(objTemp);
          }
        });

        this.termsForm.patchValue({
          fields: this.bucketData.keywordsMapTransFormLabelAndValue.find(item => {
            return JSON.stringify(item) === JSON.stringify(this.bucketData.bucketsSetting.fields);
          }),
          order_by:
            this.bucketData.bucketsSetting.order_by ||
            (this.metricsList.length > 0 ? this.metricsList[0].value : '_count'),
          order: this.bucketData.bucketsSetting.order || 'desc',
          size: this.bucketData.bucketsSetting.size || '5',
          bucketsLabel: this.bucketData.bucketsSetting.bucketsLabel || '',
        });
      });
    }
  }

  openSelect(e) {
    if (e) {
      this.metricsList = [];
      this.metric.buildMetricsOption().map(item => {
        if (item.metricsAggregation.label !== 'Count') {
          const objTemp = {
            label: `Metric: ${item.metricsAggregation.label} ${item.metricsSetting.fields.label}`,
            value: item.metricItemName,
          };
          this.metricsList.push(objTemp);
        }
      });
    }
  }
  // 构建
  buildBucketTermsOption() {
    for (const i in this.termsForm.controls) {
      this.termsForm.controls[i].markAsDirty();
      this.termsForm.controls[i].updateValueAndValidity();
    }
    if (this.termsForm.valid) {
      let tempObj;
      this.bucketData.keywordsMapTransFormLabelAndValue = [];
      if (this.bucketData.isEdit) {
        this.bucketData.bucketsSetting = this.termsForm.value;
        tempObj = { ...this.bucketData };
      } else {
        tempObj = { bucketsSetting: this.termsForm.value, ...this.bucketData };
      }

      return tempObj;
    }
  }
  // 销毁订阅
  ngOnDestroy() {
    // this._aggregationDataService.sub.unsubscribe();
  }
}
