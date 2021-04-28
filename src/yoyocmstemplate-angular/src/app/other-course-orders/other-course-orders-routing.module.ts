import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NeteaseOrderInfosComponent } from './netease-orders/netease-order-info.component';
import { TencentOrderInfoComponent } from './tencent-orders/tencent-order-info.component';


const routes: Routes = [


  { path: 'netease-orderinfo', component: NeteaseOrderInfosComponent },
  {
    path: 'tencent-orderinfo',
    component: TencentOrderInfoComponent,
    data: { permission: 'Pages.TencentOrderInfo' },
  },
  {
    path: '**',
    redirectTo: 'netease-orderinfo',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})


export class OtherCourseOrdersRoutingModule { }
