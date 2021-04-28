import { Component, EventEmitter, forwardRef, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { DatePickerBase } from '@shared/ng-zorro/date-picker/date-picker-base';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import * as moment from 'moment';

@Component({
  selector: 'app-month-picker',
  templateUrl: './month-picker.component.html',
  styles: [],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => MonthPickerComponent),
    multi: true
  }]
})
export class MonthPickerComponent
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
  nzFormat = 'yyyy-MM';

  /**
   * 在面板中添加额外的页脚
   */
  @Input()
  nzRenderExtraFooter: TemplateRef<any> | string | (() => TemplateRef<any> | string);

  /**
   * 输入框提示文字
   */
  @Input()
  nzPlaceHolder: string;

  /**
   * 时间发生变化的回调
   */
  @Output()
  ngModelChange = new EventEmitter<moment.Moment>();


  constructor() {
    super();
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
