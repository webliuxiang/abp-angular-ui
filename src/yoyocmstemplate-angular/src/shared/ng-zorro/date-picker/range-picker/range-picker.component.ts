import { Component, EventEmitter, forwardRef, Input, Output, TemplateRef } from '@angular/core';
import { DatePickerBase } from '@shared/ng-zorro/date-picker/date-picker-base';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import * as moment from 'moment';

@Component({
  selector: 'app-range-picker',
  templateUrl: './range-picker.component.html',
  styles: [],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => RangePickerComponent),
    multi: true,
  }],
})
export class RangePickerComponent
  extends DatePickerBase
  implements ControlValueAccessor {

  /**
   * 内部用的值
   */
  value: Date[];

  /**
   * 日期 Moment
   */
  @Input()
  ngModel: moment.Moment[];

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
   * 预设时间范围快捷选择
   */
  @Input()
  nzRanges: { [key: string]: Date[] } | { [key: string]: () => Date[] };

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
   * 输入框提示文字
   */
  @Input()
  nzPlaceHolder: string;

  /**
   * 点击确定按钮的回调
   */
  @Output()
  nzOnOk = new EventEmitter<moment.Moment[]>();

  /**
   * 时间发生变化的回调
   */
  @Output()
  ngModelChange = new EventEmitter<moment.Moment[]>();

  /**
   * 待选日期发生变化的回调
   */
  @Output()
  nzOnCalendarChange = new EventEmitter<moment.Moment[]>();


  constructor() {
    super();
  }


  /**
   * 点击确定按钮的回调
   * @param event
   */
  onNzOk(event: Date[]) {
    if (event && moment.isDate(event[0] && moment.isDate(event[1]))) {
      this.nzOnOk.emit(event.map(o => {
        return moment(o);
      }));
    } else {
      this.nzOnOk.emit(undefined);
    }

  }

  /**
   * 时间发生变化的回调
   * @param event
   */
  onNgModelChangeChange(event: Date[]) {
    if (Array.isArray(event) && event.length === 2) {
      this.ngModelChange.emit(event.map(o => {
        return moment(o);
      }));
    } else {
      this.ngModelChange.emit(undefined);
    }
  }

  /**
   * 待选日期发生变化的回调
   * @param event
   */
  onNzOnCalendarChange(event: Date[]) {
    if (Array.isArray(event) && event.length === 2) {
      this.nzOnCalendarChange.emit(event.map(o => {
        return moment(o);
      }));
    } else {
      this.nzOnCalendarChange.emit(undefined);
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
    if (obj && Array.isArray(obj) && obj.length === 2 && moment.isMoment(obj[0]) && moment.isMoment(obj[1])) {
      this.value = [obj[0].toDate(), obj[1].toDate()];
    } else {
      this.value = obj;
    }
  }

}
