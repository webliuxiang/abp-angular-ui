import { Component, EventEmitter, forwardRef, Input, Output, TemplateRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import * as moment from 'moment';
import { DatePickerBase } from '@shared/ng-zorro/date-picker/date-picker-base';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.less'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DatePickerComponent),
    multi: true,
  }],
})
export class DatePickerComponent
  extends DatePickerBase
  implements ControlValueAccessor {

  /**
   * 内部用的值
   */
  value: Date;

  /**
   * 日期 Moment
   */
  @Input()
  ngModel: moment.Moment;

  /**
   * 不可选择的时间
   */
  @Input()
  nzDisabledTime: (current: Date) => { nzDisabledHours, nzDisabledMinutes, nzDisabledSeconds };

  /**
   * 展示的日期格式，见nzFormat特别说明
   */
  @Input()
  nzFormat = 'yyyy-MM-dd';

  /**
   * 在面板中添加额外的页脚
   */
  @Input()
  nzRenderExtraFooter: TemplateRef<any> | string | (() => TemplateRef<any> | string);


  /**
   * 增加时间选择功能
   */
  @Input()
  nzShowTime: object | boolean;

  /**
   * nzShowToday
   */
  @Input()
  nzShowToday = true;

  /**
   * 输入框提示文字
   */
  @Input()
  nzPlaceHolder: string;

  /**
   * 点击确定按钮的回调
   */
  @Output()
  nzOnOk = new EventEmitter<moment.Moment>();

  /**
   * 时间发生变化的回调
   */
  @Output()
  ngModelChange = new EventEmitter<moment.Moment>();


  constructor() {
    super();
  }


  onNzOk(event: Date) {
    if (event && moment.isDate(event)) {
      this.nzOnOk.emit(moment(event));
    } else {
      this.nzOnOk.emit(undefined);
    }

  }

  onNgModelChangeChange(event: Date) {
    if (event && moment.isDate(event)) {
      this.ngModelChange.emit(moment(event));
    } else {
      this.ngModelChange.emit(undefined);
    }
  }


  registerOnChange(fn: any): void {
    this.ngModelChange.emit = fn;
  }

  registerOnTouched(fn: any): void {
  }

  setDisabledState(isDisabled: boolean): void {
    this.nzDisabled = isDisabled;
  }

  writeValue(obj: any): void {
    if (obj && moment.isMoment(obj)) {
      this.value = obj.toDate();
    } else {
      this.value = obj;
    }
  }

}
