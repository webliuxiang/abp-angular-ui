import { Component, EventEmitter, forwardRef, Input, Output, TemplateRef } from '@angular/core';
import { DatePickerBase } from '@shared/ng-zorro/date-picker/date-picker-base';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import * as moment from 'moment';

@Component({
  selector: 'app-time-picker',
  templateUrl: './time-picker.component.html',
  styles: [],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TimePickerComponent),
      multi: true,
    },
  ],
})
export class TimePickerComponent extends DatePickerBase implements ControlValueAccessor {
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
  nzDisabledTime: (current: Date) => { nzDisabledHours; nzDisabledMinutes; nzDisabledSeconds };

  /**
   * 展示的日期格式，见nzFormat特别说明
   */
  @Input()
  nzFormat = 'HH:mm:ss';

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
   * 选择框底部显示自定义的内容
   */
  @Input()
  nzAddOn: TemplateRef<void>;
  /**
   * 自动获取焦点
   */
  @Input()
  nzAutoFocus = false;
  /**
   * 禁止选择部分小时选项
   */
  @Input()
  nzDisabledHours: () => number[];
  /**
   * 禁止选择部分分钟选项
   */
  @Input()
  nzDisabledMinutes: (hour: number) => number[];
  /**
   * 禁止选择部分秒选项
   */
  @Input()
  nzDisabledSeconds: (hour: number, minute: number) => number[];
  /**
   * 隐藏禁止选择的选项
   */
  @Input()
  nzHideDisabledOptions = false;
  /**
   * 小时选项间隔
   */
  @Input()
  nzHourStep = 1;
  /**
   * 分钟选项间隔
   */
  @Input()
  nzMinuteStep = 1;
  /**
   * 	秒选项间隔
   */
  @Input()
  nzSecondStep = 1;
  /**
   * 面板是否打开，可双向绑定
   */
  @Input()
  nzOpen = false;
  /**
   * 弹出层类名
   */
  @Input()
  nzPopupClassName = '';
  /**
   * 当 [ngModel] 不存在时，可以设置面板打开时默认选中的值
   */
  @Input()
  nzDefaultOpenValue = new Date();

  /**
   * 使用12小时制，为true时format默认为h:mm:ss a
   */
  @Input()
  nzUse12Hours = false;

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
      this.nzOnOk.emit(
        event.map(o => {
          return moment(o);
        }),
      );
    } else {
      this.nzOnOk.emit(undefined);
    }
  }

  /**
   * 时间发生变化的回调
   * @param event
   */
  onNgModelChangeChange(event: Date[]) {
    if (event && moment.isDate(event[0] && moment.isDate(event[1]))) {
      this.ngModelChange.emit(
        event.map(o => {
          return moment(o);
        }),
      );
    } else {
      this.ngModelChange.emit(undefined);
    }
  }

  /**
   * 待选日期发生变化的回调
   * @param event
   */
  onNzOnCalendarChange(event: Date[]) {
    if (event && moment.isDate(event[0] && moment.isDate(event[1]))) {
      this.nzOnCalendarChange.emit(
        event.map(o => {
          return moment(o);
        }),
      );
    } else {
      this.nzOnCalendarChange.emit(undefined);
    }
  }

  registerOnChange(fn: any): void {
    this.ngModelChange.emit = fn;
  }

  registerOnTouched(fn: any): void {}

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
