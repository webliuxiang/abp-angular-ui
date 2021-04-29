import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

@Component({
  selector: 'app-max-module',
  templateUrl: './max-module.component.html',
  styleUrls: ['./max-module.component.less'],
})
export class MaxModuleComponent implements OnInit {
  @Input() metricData;
  maxForm!: FormGroup;
  dateKeywordMap = [];
  numberKeywordMap = [];
  stringKeywordMap = [];
  keywordMap = [];
  constructor(private fb: FormBuilder, private _aggregationDataService: DataAnalyzeService) {}

  ngOnInit() {
    this.maxForm = this.fb.group({
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
      this.maxForm.patchValue({
        fields: this.metricData.keywordsMapTransFormLabelAndValue.find((item, key) => {
          return JSON.stringify(item) === JSON.stringify(this.metricData.metricsSetting.fields);
        }),
        metricsLabel: this.metricData.metricsSetting.metricsLabel,
      });
    }
  }

  // 构建当前 max 指标数据
  buildMetricMaxOption() {
    for (const i in this.maxForm.controls) {
      this.maxForm.controls[i].markAsDirty();
      this.maxForm.controls[i].updateValueAndValidity();
    }
    if (this.maxForm.valid) {
      let tempObj;
      this.metricData.keywordsMapTransFormLabelAndValue = [];
      if (this.metricData.isEdit) {
        this.metricData.metricsSetting = this.maxForm.value;
        tempObj = { ...this.metricData };
      } else {
        tempObj = { metricsSetting: this.maxForm.value, ...this.metricData };
      }

      return tempObj;
    }
  }
}
