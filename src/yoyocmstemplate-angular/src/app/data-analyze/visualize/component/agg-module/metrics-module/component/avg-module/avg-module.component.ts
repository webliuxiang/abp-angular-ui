import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-avg-module',
  templateUrl: './avg-module.component.html',
  styleUrls: ['./avg-module.component.less'],
})
export class AvgModuleComponent implements OnInit {
  @Input() metricData;
  avgForm!: FormGroup;
  dateKeywordMap = [];
  numberKeywordMap = [];
  stringKeywordMap = [];
  keywordMap = [];
  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.avgForm = this.fb.group({
      fields: ['', [Validators.required]],
      metricsLabel: [''],
    });
    // console.log(this.metricData);
    this.metricData.keywordsMapTransFormLabelAndValue.find(item => {
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
    if (this.metricData.isEdit) {
      // console.log(this.avgForm);

      this.avgForm.patchValue({
        fields: this.metricData.keywordsMapTransFormLabelAndValue.find((item, key) => {
          return JSON.stringify(item) === JSON.stringify(this.metricData.metricsSetting.fields);
        }),
        metricsLabel: this.metricData.metricsSetting.metricsLabel,
      });
    }
  }

  // 构建当前 avg 指标数据
  buildMetricAvgOption() {
    // console.log('调用 avg 指标配置');
    for (const i in this.avgForm.controls) {
      this.avgForm.controls[i].markAsDirty();
      this.avgForm.controls[i].updateValueAndValidity();
    }
    if (this.avgForm.valid) {
      let tempObj;
      this.metricData.keywordsMapTransFormLabelAndValue = [];
      if (this.metricData.isEdit) {
        this.metricData.metricsSetting = this.avgForm.value;
        tempObj = { ...this.metricData };
      } else {
        tempObj = { metricsSetting: this.avgForm.value, ...this.metricData };
      }
      return tempObj;
    }
  }
}
