import { Component, OnInit, forwardRef, Input, Output, EventEmitter, Injector } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { AppComponentBase } from '@shared/component-base';
import { DropdownListServiceProxy } from '@shared/service-proxies/service-proxies';
@Component({
  selector: 'sample-dropdown',
  templateUrl: './sample-dropdown.component.html',
  styleUrls: ['./sample-dropdown.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SampleDropdownComponent),
      multi: true,
    },
  ],
})
export class SampleDropdownComponent extends AppComponentBase
  implements OnInit, ControlValueAccessor {


  @Input()
  typeCd: string;

  @Input()
  returnId = false;

  @Input()
  disabled = false;
  modelValue: string;
  firstSelected = false; // 默认选中第一个

  @Input()
  onlyShowText: boolean;

  @Input()
  get selection(): string {
    return this.modelValue;
  }

  set selection(val: string) {
    this.modelValue = val;
  }
  @Output()
  selectionChange = new EventEmitter<string>();

  options = [];

  @Input()
  emptyText = this.l('请选择');

  @Input()
  returnField: string;

  @Input()
  autoLoad = true;
  inputField: string;

  @Input()
  isConvertInt = false; // 是否将value转化为int

  @Input()
  size = 'default';

  constructor(injector: Injector, private _dropdownListServiceProxy: DropdownListServiceProxy) {
    super(injector);
  }
  ngOnInit() {

    this.refreshData();

  }

  refreshData() {

    this._dropdownListServiceProxy.getByDDTypeId(this.typeCd)
      .subscribe((result) => {
        this.options = result;
        if (this.firstSelected) {
          if (result.length > 0) {
            this.selection = result[0].name_TX;
            this.selectionChange.emit(this.modelValue);
          }
        }
      });

  }

  getDisplayValue(key: string): string {
    const option = this.options.find((o) => o.id === key + '');
    if (!option) {
      return '';
    } else {
      return option.name_TX;
    }
  }

  onChange(event: any) {
    this.selectionChange.emit(this.modelValue);
  }

  writeValue(obj: any): void {
    this.modelValue = obj;
  }
  registerOnChange(fn: any): void {
    this.selectionChange.emit = fn;
  }
  registerOnTouched(fn: any): void {

  }
  setDisabledState?(isDisabled: boolean): void {

  }
}
