import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-sum-module',
  templateUrl: './sum-module.component.html',
  styleUrls: ['./sum-module.component.less'],
})
export class SumModuleComponent implements OnInit {
  @Input() metricData;
  sumForm!: FormGroup;
  dateKeywordMap = [];
  numberKeywordMap = [];
  stringKeywordMap = [];
  keywordMap = [];
  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.sumForm = this.fb.group({
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
      this.sumForm.patchValue({
        fields: this.metricData.keywordsMapTransFormLabelAndValue.find((item, key) => {
          return JSON.stringify(item) === JSON.stringify(this.metricData.metricsSetting.fields);
        }),
        metricsLabel: this.metricData.metricsSetting.metricsLabel,
      });
    }
  }

  // 构建当前 sum 指标数据
  buildMetricSumOption() {
    for (const i in this.sumForm.controls) {
      this.sumForm.controls[i].markAsDirty();
      this.sumForm.controls[i].updateValueAndValidity();
    }
    if (this.sumForm.valid) {
      let tempObj;
      this.metricData.keywordsMapTransFormLabelAndValue = [];
      if (this.metricData.isEdit) {
        this.metricData.metricsSetting = this.sumForm.value;
        tempObj = { ...this.metricData };
      } else {
        tempObj = { metricsSetting: this.sumForm.value, ...this.metricData };
      }

      return tempObj;
    }
  }
}
