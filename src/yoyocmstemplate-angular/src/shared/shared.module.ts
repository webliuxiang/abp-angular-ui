import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppSessionService } from '@shared/session/app-session.service';
import { AppUrlService } from '@shared/nav/app-url.service';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';

// region: third libs
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { CountdownModule } from 'ngx-countdown';

import { DelonABCModule } from '@delon/abc';
import { DelonFormModule } from '@delon/form';
import { AlainThemeModule } from '@delon/theme';

// endregion
import { DelonChartModule } from '@delon/chart';
import { DelonACLModule } from '@delon/acl';
import { PermissionService } from './auth';
import { DirectivesModule } from './directives/directives.module';
import { FileDownloadService, TreeDataHelperService, ArrayToTreeConverterService } from './utils';
import { MomentFromNowPipe } from './utils/moment-from-now.pipe';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoadingInterceptor } from './utils/global-loading/loading.interceptor';
import { OverlayModule } from '@angular/cdk/overlay';
import { OverlayComponent } from './utils/global-loading/overlay/overlay.component';
import { CustomNgZorroModule } from './ng-zorro';
import { NoDataComponent } from './components/no-data/no-data.component';
import { ImgShowComponent } from './components/img-show/img-show.component';
import { FileManagerComponent } from './components/file-manager/file-manager.component';
import { ImgDirective } from './components/img/img.directive';
import { ProLangsComponent } from './components/pro-langs/pro-langs.component';
import { ScrollbarDirective } from './components/scrollbar/scrollbar.directive';
import { PageGridComponent } from './components/page-grid/page-grid.component';
import { LookupComponent } from './components/lookup/lookup.component';
import { FriendProfilePictureComponent } from './components/friend-profile-picture/friend-profile-picture.component';
import { ImgComponent } from './components/img/img.component';
import { TableCheckboxPanelComponent } from './components/table-checkbox-panel/table-checkbox-panel.component';
import { SimplemdeConfig, SimplemdeModule } from 'ngx-simplemde';
import { PageFilterModule } from './sample/page-filter';
import { SampleTableModule } from './sample/table';
import { ValidationMessagesModule } from './sample/validation-messages';
import { AbpModule } from 'abp-ng2-module';
import { MomentFormatPipe } from '@shared/utils/moment-format.pipe';
import { UploadUserPortraitComponent } from './components/upload-user-portrait/upload-user-portrait.component';

const THIRDMODULES = [
  NgZorroAntdModule,
  CountdownModule,
  CustomNgZorroModule,
  DirectivesModule,
  NzSpinModule,
  OverlayModule,
  PageFilterModule,
  SampleTableModule,
  SimplemdeModule,
  ValidationMessagesModule,
  //  DragDropModule,
  //  NgxImageGalleryModule,
];

/** 需要注入到 entryComponents 的组件 */
const COMPONENTS_ENTRY = [ImgComponent, ImgShowComponent, LookupComponent, FriendProfilePictureComponent];

/**
 * 组件
 */
const COMPONENTS = [
  NoDataComponent,
  UploadUserPortraitComponent,
  FileManagerComponent,
  ProLangsComponent,
  PageGridComponent,
  TableCheckboxPanelComponent,
  ...COMPONENTS_ENTRY,
];

/**
 * 指令
 */
const DIRECTIVES = [ImgDirective, ScrollbarDirective];

/**
 * markdown 编辑器全局配置
 */
const SAMPLE_MD_CONFIG: SimplemdeConfig = {
  style: 'antd',
  delay: 1,
  options: { toolbar: ['bold', 'italic', 'heading', '|', 'quote'] },
} as any;

@NgModule({
  imports: [
    CommonModule,
    AbpModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    AlainThemeModule.forChild(),
    DelonABCModule,
    DelonACLModule,
    DelonChartModule,
    DelonFormModule,
    // third libs
    ...THIRDMODULES,
  ],
  declarations: [MomentFormatPipe, MomentFromNowPipe, OverlayComponent, ...COMPONENTS, ...DIRECTIVES],
  entryComponents: [OverlayComponent, ...COMPONENTS_ENTRY],

  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    AlainThemeModule,
    DelonABCModule,
    DelonACLModule,
    DelonChartModule,
    DelonFormModule,
    // third libs
    ...THIRDMODULES,
    ...COMPONENTS,
    ...DIRECTIVES,
    MomentFormatPipe,
    MomentFromNowPipe,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
  ],
})
export class SharedModule {
  static forRoot(): ModuleWithProviders<SharedModule> {
    return {
      ngModule: SharedModule,
      providers: [
        AppSessionService,
        AppUrlService,
        AppAuthService,
        AppRouteGuard,
        FileDownloadService,
        TreeDataHelperService,
        ArrayToTreeConverterService,
        PermissionService,
        { provide: SimplemdeConfig, useValue: SAMPLE_MD_CONFIG },
      ],
    };
  }
}
