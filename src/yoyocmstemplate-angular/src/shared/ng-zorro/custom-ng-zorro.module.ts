import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { NzTimePickerModule } from 'ng-zorro-antd/time-picker';
import { FormsModule } from '@angular/forms';
import {
  DatePickerComponent,
  MonthPickerComponent,
  RangePickerComponent,
  WeekPickerComponent,
  YearPickerComponent,
  TimePickerComponent,
} from './date-picker';
import { CSimplemdeComponent } from './c-simplemde/c-simplemde.component';
import { SampleComponentsModule } from '@shared/sample/components';
import { SimplemdeModule } from 'ngx-simplemde';

const COMPONENTS = [
  DatePickerComponent,
  RangePickerComponent,
  YearPickerComponent,
  MonthPickerComponent,
  WeekPickerComponent,
  TimePickerComponent,
  CSimplemdeComponent
];

@NgModule({
  imports: [CommonModule, NgZorroAntdModule, NzTimePickerModule, FormsModule, SimplemdeModule],
  declarations: [...COMPONENTS],
  exports: [...COMPONENTS],
})

/**自定义组件模块 */
export class CustomNgZorroModule {
  static forRoot(): ModuleWithProviders<CustomNgZorroModule> {
    return { ngModule: CustomNgZorroModule };
  }
}
