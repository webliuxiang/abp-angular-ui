import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-count-module',
  templateUrl: './count-module.component.html',
  styleUrls: ['./count-module.component.less'],
})
export class CountModuleComponent implements OnInit {
  @Input() metricData;
  countForm!: FormGroup;
  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.countForm = this.fb.group({
      metricsLabel: [''],
    });
    if (this.metricData.isEdit) {
      this.countForm.patchValue({ metricsLabel: this.metricData.metricsSetting.metricsLabel });
    }
  }

  // 构建当前 count 指标数据
  buildMetricAvgOption() {
    for (const i in this.countForm.controls) {
      this.countForm.controls[i].markAsDirty();
      this.countForm.controls[i].updateValueAndValidity();
    }
    if (this.countForm.valid) {
      let tempObj;
      this.metricData.keywordsMapTransFormLabelAndValue = [];
      if (this.metricData.isEdit) {
        this.metricData.metricsSetting = this.countForm.value;
        tempObj = { ...this.metricData };
      } else {
        tempObj = { metricsSetting: this.countForm.value, ...this.metricData };
      }

      return tempObj;
    }
  }
}
