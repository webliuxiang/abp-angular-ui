import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-date-time',
  templateUrl: './date-time.component.html',
  styleUrls: ['./date-time.component.less'],
})
export class DateTimeComponent implements OnInit {
  @Input() beforeCreateChart;
  @Input() dateTimeData;
  // 时间组件
  dateTimeForm: FormGroup;
  removeDateTime = false;
  dateTimeTitle = '关闭';
  disabledStatus = false;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.dateTimeForm = this.fb.group({
      removeDateTimeSet: [false],
      datePicker: [[, , 'absolute']],
    });
    // console.log(this.dateTimeData);
    if (this.dateTimeData.isEdit) {
      if (this.dateTimeData.data.length < 1) {
        this.dateTimeForm.patchValue({ removeDateTimeSet: true, datePicker: [] });
      } else {
        this.dateTimeForm.patchValue({
          removeDateTimeSet: false,
          datePicker: this.dateTimeData.data,
        });
      }
    }
  }

  // 关闭时间设置
  dateTimeSetChange(e) {
    // console.log(e);
    // 关闭时间设置时保存当前已经设置的值，方便打开时继续使用
    this.dateTimeForm
      .get('datePicker')
      .patchValue(e ? this.dateTimeForm.value.datePicker : this.dateTimeForm.value.datePicker);

    if (e) {
      this.removeDateTime = true;
      this.disabledStatus = true;
      this.dateTimeTitle = '打开';
    } else {
      this.removeDateTime = false;
      this.disabledStatus = false;
      this.dateTimeTitle = '关闭';
    }
  }
}
