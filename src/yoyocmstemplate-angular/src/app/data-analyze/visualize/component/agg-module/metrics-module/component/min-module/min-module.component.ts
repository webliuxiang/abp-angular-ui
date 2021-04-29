import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-min-module',
  templateUrl: './min-module.component.html',
  styleUrls: ['./min-module.component.less'],
})
export class MinModuleComponent implements OnInit {
  @Input() metricData;
  minForm!: FormGroup;
  dateKeywordMap = [];
  numberKeywordMap = [];
  stringKeywordMap = [];
  keywordMap = [];
  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.minForm = this.fb.group({
      fields: ['', [Validators.required]],
      metricsLabel: [''],
    });
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
      this.minForm.patchValue({
        fields: this.metricData.keywordsMapTransFormLabelAndValue.find((item, key) => {
          return JSON.stringify(item) === JSON.stringify(this.metricData.metricsSetting.fields);
        }),
        metricsLabel: this.metricData.metricsSetting.metricsLabel,
      });
    }
  }

  // 构建当前 min 指标数据
  buildMetricMinOption() {
    for (const i in this.minForm.controls) {
      this.minForm.controls[i].markAsDirty();
      this.minForm.controls[i].updateValueAndValidity();
    }
    if (this.minForm.valid) {
      let tempObj;
      this.metricData.keywordsMapTransFormLabelAndValue = [];
      if (this.metricData.isEdit) {
        this.metricData.metricsSetting = this.minForm.value;
        tempObj = { ...this.metricData };
      } else {
        tempObj = { metricsSetting: this.minForm.value, ...this.metricData };
      }

      return tempObj;
    }
  }
}
