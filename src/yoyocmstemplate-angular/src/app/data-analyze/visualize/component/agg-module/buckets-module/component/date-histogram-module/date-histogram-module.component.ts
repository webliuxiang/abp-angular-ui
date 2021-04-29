import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-date-histogram-module',
  templateUrl: './date-histogram-module.component.html',
  styleUrls: ['./date-histogram-module.component.less'],
})
export class DateHistogramModuleComponent implements OnInit {
  @Input() bucketData;
  dateHistogramForm: FormGroup;

  origin_min_Interval = '';
  dateKeywordMap = [];
  numberKeywordMap = [];
  stringKeywordMap = [];
  keywordMap = [];

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.dateHistogramForm = this.fb.group({
      fields: ['', [Validators.required]],
      min_Interval: [['auto'], [Validators.required]],
      bucketsLabel: [''],
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
      this.dateHistogramForm.patchValue({
        fields: this.bucketData.keywordsMapTransFormLabelAndValue.find(item => {
          return JSON.stringify(item) === JSON.stringify(this.bucketData.bucketsSetting.fields);
        }),
        min_Interval: this.bucketData.bucketsSetting.min_Interval || ['auto'],
        bucketsLabel: this.bucketData.bucketsSetting.bucketsLabel || '',
      });
    }
  }
  // 打开下拉选项
  openOptions(e) {
    const that = JSON.parse(JSON.stringify(this.dateHistogramForm.value));
    if (e) {
      this.origin_min_Interval = that.min_Interval[0];
      this.dateHistogramForm.patchValue({
        fields: this.dateHistogramForm.value.fields,
        min_Interval: [],
        bucketsLabel: this.dateHistogramForm.value.bucketsLabel,
      });
    }
    if (!e && this.dateHistogramForm.value.min_Interval[0] === undefined) {
      this.dateHistogramForm.patchValue({
        fields: this.dateHistogramForm.value.fields,
        min_Interval: [this.origin_min_Interval],
        bucketsLabel: this.dateHistogramForm.value.bucketsLabel,
      });
    }
  }
  // 构建
  buildBucketTermsOption() {
    for (const i in this.dateHistogramForm.controls) {
      this.dateHistogramForm.controls[i].markAsDirty();
      this.dateHistogramForm.controls[i].updateValueAndValidity();
    }
    if (this.dateHistogramForm.valid) {
      let tempObj;
      this.bucketData.keywordsMapTransFormLabelAndValue = [];
      if (this.bucketData.isEdit) {
        this.bucketData.bucketsSetting = this.dateHistogramForm.value;
        tempObj = { ...this.bucketData };
      } else {
        tempObj = {
          bucketsSetting: this.dateHistogramForm.value,
          ...this.bucketData,
        };
      }
      return tempObj;
    }
  }
}
