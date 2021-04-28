import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbpModule } from 'abp-ng2-module';
import { SharedModule } from '@shared/shared.module';
import { SimplemdeModule } from 'ngx-simplemde';

import { DemoManagementRoutingModule } from './demo-management-routing.module';
import { DemoUiComponent } from './demo-ui/demoui.component';
import { from } from 'rxjs';
import { XAddressLinkageComponent } from './components/x-address-linkage/x-address-linkage.component';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
const COMPONENTS = [];
const COMPONENTS_NOROUNT = [];

@NgModule({
  imports: [CommonModule, DemoManagementRoutingModule, SharedModule, AbpModule, NzDatePickerModule, SimplemdeModule],
  declarations: [...COMPONENTS, ...COMPONENTS_NOROUNT, DemoUiComponent, XAddressLinkageComponent],
  entryComponents: COMPONENTS_NOROUNT,
})
export class DemoManagementModule {}
