import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MarketingRoutingModule } from './marketing-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { ProductComponent } from './product-management/product.component';
import { CreateOrEditProductComponent } from './product-management/create-or-edit-product/create-or-edit-product.component';
import { OrderComponent } from './order/order.component';
import { FormsModule } from '@angular/forms';
import { ProductOrderStatisticsComponent } from './order/product-order-statistics/product-order-statistics.component';
import { ProductSecretKeyComponent } from './product-secret-key-management/product-secret-key.component';
import {
  CreateOrEditProductSecretKeyComponent,
} from './product-secret-key-management/create-or-edit-product-secret-key/create-or-edit-product-secret-key.component';
import {
  BindProductSecretKeyToUserComponent,
} from './product-secret-key-management/bind-product-secret-key-to-user/bind-product-secret-key-to-user.component';
import { EditOrderPriceComponent } from '@app/marketing/order/edit-order-price';
import { CreateOrEditOrderComponent } from './order/create-or-edit-order/create-or-edit-order.component';
import { SimplemdeModule } from 'ngx-simplemde';
import { SampleComponentsModule } from '@shared/sample/components';

@NgModule({
  imports: [CommonModule, FormsModule, MarketingRoutingModule, HttpClientModule, SharedModule,
    SimplemdeModule,
    SampleComponentsModule, ],
  declarations: [
    ProductComponent,
    ProductOrderStatisticsComponent,
    CreateOrEditProductComponent,
    ProductSecretKeyComponent,
    CreateOrEditProductSecretKeyComponent,
    OrderComponent,
    BindProductSecretKeyToUserComponent,
    EditOrderPriceComponent,
    CreateOrEditOrderComponent,
  ],
  entryComponents: [
    CreateOrEditProductComponent,
    ProductOrderStatisticsComponent,
    CreateOrEditProductSecretKeyComponent,
    BindProductSecretKeyToUserComponent,
    EditOrderPriceComponent,
    CreateOrEditOrderComponent
  ],
})
export class MarketingModule {
}
