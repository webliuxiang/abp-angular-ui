import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SHARED_DELON_MODULES } from '../../shared-delon.module';
import { SHARED_ZORRO_MODULES } from '../../shared-zorro.module';
import { ValidationMessagesComponent } from './validation-messages.component';


const COMPONENTS = [
  ValidationMessagesComponent,
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SHARED_DELON_MODULES,
    SHARED_ZORRO_MODULES,
  ],
  declarations: [
    ...COMPONENTS,
  ],
  exports: [
    ...COMPONENTS,
  ]
})
export class ValidationMessagesModule {
}
