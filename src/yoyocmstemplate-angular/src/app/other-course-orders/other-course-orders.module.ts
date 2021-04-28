import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { SampleComponentsModule } from '@shared/sample/components';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'abp-ng2-module';
import { SimplemdeModule } from 'ngx-simplemde';
import { OtherCourseOrdersRoutingModule } from './other-course-orders-routing.module';


import { TencentOrderInfoComponent } from './tencent-orders/tencent-order-info.component';
import { CreateOrEditTencentOrderInfoComponent } from './tencent-orders/create-or-edit-tencent-order-info';
import { NeteaseOrderInfosComponent } from './netease-orders/netease-order-info.component';
import { NeteaseOrderInfoDetailsComponent } from './netease-orders/netease-order-info-details/netease-order-info-details.component';
import { NeteaseOrderInfoStatisticsComponent } from './netease-orders/netease-order-info-statistics/netease-order-info-statistics.component';



const ENTRY_COMPONENTS = [
  NeteaseOrderInfoDetailsComponent,
  NeteaseOrderInfoStatisticsComponent,
  CreateOrEditTencentOrderInfoComponent,


];

const COMPONENTS = [
  NeteaseOrderInfosComponent,

  TencentOrderInfoComponent,

  ...ENTRY_COMPONENTS,
];



@NgModule({
  imports: [
    CommonModule,
    OtherCourseOrdersRoutingModule,
    HttpClientModule,
    SharedModule,
    AbpModule,
    SimplemdeModule,
    SampleComponentsModule,
  ],
  declarations: [
    ...COMPONENTS,
  ],
  entryComponents: [
    ...ENTRY_COMPONENTS,
  ],
  providers: [],
})


export class OtherCourseOrdersModule { }
