import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { SHARED_DELON_MODULES } from '../../shared-delon.module';
import { SHARED_ZORRO_MODULES } from '../..//shared-zorro.module';
import { SampleDataSourceService } from './sample-data-source.service';
import { SampleInputComponent } from './sample-input';
import { SampleSelectComponent } from './sample-select';
import { SampleDropdownComponent } from './sample-dropdown/sample-dropdown.component';
import { SampleDateComponent } from './sample-date';
import { CustomNgZorroModule } from '../../ng-zorro';



const COMPONENTS = [
  SampleInputComponent,
  SampleSelectComponent,
  SampleDropdownComponent,
  SampleDateComponent
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SHARED_DELON_MODULES,
    SHARED_ZORRO_MODULES,
    CustomNgZorroModule,
  ],
  declarations: [
    ...COMPONENTS,
  ],
  exports: [
    ...COMPONENTS,
  ],
  providers: [
    SampleDataSourceService,
  ],
})
export class SampleComponentsModule {
}
