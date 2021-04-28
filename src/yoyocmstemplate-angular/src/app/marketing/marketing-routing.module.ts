import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductComponent } from './product-management/product.component';
import { OrderComponent } from './order/order.component';
import { ProductSecretKeyComponent } from './product-secret-key-management/product-secret-key.component';

const routes: Routes = [
  { path: 'product', component: ProductComponent, data: { permission: 'Pages.Product' } },
  { path: 'product-secret-key', component: ProductSecretKeyComponent, data: { permission: 'Pages.ProductSecretKey' } },

  { path: 'order', component: OrderComponent, data: { permission: 'Pages.Order' } },
  {
    path: '**',
    redirectTo: 'product',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MarketingRoutingModule {}
