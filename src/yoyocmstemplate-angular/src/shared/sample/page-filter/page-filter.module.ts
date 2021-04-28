import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { PageFilterComponent } from './page-filter';
import { SHARED_DELON_MODULES } from '@shared/shared-delon.module';
import { SHARED_ZORRO_MODULES } from '@shared/shared-zorro.module';
import { SampleComponentsModule } from '../components';
import { FormsModule } from '@angular/forms';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SampleComponentsModule,
    SHARED_DELON_MODULES,
    SHARED_ZORRO_MODULES,
  ],
  declarations: [
    PageFilterComponent
  ],
  exports: [
    PageFilterComponent
  ],
})
export class PageFilterModule {
}
