import { EventEmitter, Input, Output, TemplateRef, Directive } from '@angular/core';

@Directive()
export abstract class DatePickerBase {

  /**
   * 名称
   */
  @Input()
  name: string;


  /**
   * 是否显示清除按钮
   */
  @Input()
  nzAllowClear = true;

  /**
   * 自动获取焦点
   */
  @Input()
  nzAutoFocus = false;

  /**
   * 选择器
   */
  @Input()
  nzClassName = '';

  /**
   * 自定义日期单元格的内容（month-picker/year-picker不支持）
   */
  @Input()
  nzDateRender: TemplateRef<Date> | string | ((d: Date) => TemplateRef<Date> | string);

  /**
   * 禁用
   */
  @Input()
  nzDisabled = false;

  /**
   * 不可选择的日期
   */
  @Input()
  nzDisabledDate: (current: Date) => boolean;

  /**
   * 国际化配置  object
   */
  @Input()
  nzLocale: any;

  /**
   * 控制弹层是否展开,暂时无用
   */
  @Input()
  nzOpen: boolean;

  /**
   * 额外的弹出日历样式
   */
  @Input()
  nzPopupStyle: any = {};

  /**
   * 额外的弹出日历 className
   */
  @Input()
  nzDropdownClassName: string;

  /**
   * 输入框大小
   */
  @Input()
  nzSize: 'large' | 'small' | 'default' = 'default';

  /**
   * 自定义输入框样式
   */
  @Input()
  nzStyle: any = {};

  /**
   * 弹出日历和关闭日历的回调
   */
  @Output()
  nzOnOpenChange = new EventEmitter<boolean>();

  constructor() {

  }

  /**
   * 控制弹层展开状态发生改变
   * @param event
   */
  onNzOnOpenChange(event: boolean) {
    this.nzOpen = event;
    this.nzOnOpenChange.emit(event);
  }
}
