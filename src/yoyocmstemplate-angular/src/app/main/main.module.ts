import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';

import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';

import { AbpModule } from 'abp-ng2-module';
import { AboutComponent } from './about/about.component';
import { AdvertisingComponent } from './advertising/advertising.component';

import { CustomNgZorroModule } from '@shared/ng-zorro';

@NgModule({
  imports: [CommonModule, HttpClientModule, SharedModule, AbpModule, CustomNgZorroModule, MainRoutingModule],
  declarations: [DashboardComponent, AboutComponent, AdvertisingComponent],
  entryComponents: [AdvertisingComponent],
  providers: [],
})
export class MainModule {}
