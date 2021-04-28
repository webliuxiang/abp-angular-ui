import { NgModule, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '@app/app-routing.module';

import { SharedModule } from '@shared/shared.module';
import { HttpClientModule } from '@angular/common/http';

import { AbpModule } from 'abp-ng2-module';
import { LayoutModule } from '@layout/layout.module';
import { ImpersonationService } from '@shared/auth';
import { UserNotificationHelper } from '@shared/helpers/UserNotificationHelper';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { registerLocaleData } from '@angular/common';
import { AppSessionService } from '@shared/session/app-session.service';
import zh from '@angular/common/locales/zh';
import { AppComponent } from '@app/app.component';
import { SimplemdeModule } from 'ngx-simplemde';

registerLocaleData(zh);

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    AppRoutingModule,
    LayoutModule,
    SharedModule,
    AbpModule,
    NzModalModule,
    SimplemdeModule.forRoot({
      style: 'antd',
      delay: 1,
      options: { toolbar: ['bold', 'italic', 'heading', '|', 'quote'] } as any,
    }),
  ],
  declarations: [
    AppComponent,
  ],
  entryComponents: [],
  providers: [
    ImpersonationService,
    UserNotificationHelper,
  ],
})
export class AppModule {
}
