import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DataAnalysisRoutingModule } from './data-analysis-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'abp-ng2-module';
import { DownloadLogComponent } from './download-logs/download-log.component';
import { UserDownloadConfigComponent } from './user-download-config/user-download-config.component';
import { CreateOrEditUserDownloadConfigComponent } from './user-download-config/create-or-edit-user-download-config/create-or-edit-user-download-config.component';

@NgModule({
  imports: [
    CommonModule,
    DataAnalysisRoutingModule,
    HttpClientModule,
    SharedModule,
  ],
  declarations: [
    DownloadLogComponent,
    UserDownloadConfigComponent,
    CreateOrEditUserDownloadConfigComponent,
  ],
  entryComponents: [
    CreateOrEditUserDownloadConfigComponent,
  ]
})
export class DataAnalysisModule { }
