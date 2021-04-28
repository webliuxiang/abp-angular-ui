import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SHARED_DELON_MODULES } from '../../shared-delon.module';
import { SHARED_ZORRO_MODULES } from '../..//shared-zorro.module';
import { SampleTableComponent } from './sample-table.component';
import { SampleTableDataProcessorService } from './sample-table-data-processor.service';


const COMPONENTS = [
  SampleTableComponent,
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SHARED_DELON_MODULES,
    SHARED_ZORRO_MODULES
  ],
  declarations: [
    ...COMPONENTS,
  ],
  exports: [
    ...COMPONENTS,
  ],
  providers: [
    SampleTableDataProcessorService,
  ],
})
export class SampleTableModule {
}
