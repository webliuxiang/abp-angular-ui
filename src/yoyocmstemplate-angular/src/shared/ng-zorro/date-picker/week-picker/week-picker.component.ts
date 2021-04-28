import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { DatePickerBase } from '@shared/ng-zorro/date-picker/date-picker-base';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import * as moment from 'moment';

@Component({
  selector: 'app-week-picker',
  templateUrl: './week-picker.component.html',
  styles: [],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => WeekPickerComponent),
    multi: true
  }]
})
export class WeekPickerComponent
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
   * 展示的日期格式，见nzFormat特别说明
   */
  @Input()
  nzFormat = 'yyyy-ww';

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
