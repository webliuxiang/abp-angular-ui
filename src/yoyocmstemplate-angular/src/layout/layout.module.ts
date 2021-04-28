import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { HeaderComponent } from './default/header/header.component';
import { SidebarComponent } from './default/sidebar/sidebar.component';
import { ChangePasswordModalComponent } from './default/profile/change-password-modal.component';
import { LoginAttemptsModalComponent } from './default/profile/login-attempts-modal.component';
import { MySettingsModalComponent } from './default/profile/my-settings-modal.component';
import { LayoutDefaultComponent } from './default';

import { UserNotificationHelper } from '@shared/helpers/UserNotificationHelper';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { CommonModule } from '@angular/common';

import {
  HeaderFullScreenComponent,
  HeaderI18nComponent,
  HeaderNotificationsComponent,
  HeaderNotificationSettingsComponent,
  HeaderStorageComponent,
  HeaderUserComponent,
  YoYoSidebarNavComponent,
} from './components';
import { LayoutThemeOneComponent, ThemeOneHeaderComponent, ThemeOneSidebarComponent } from './theme-one';

// 导出组件
const EXPORT_COMPONENTS = [
  LayoutDefaultComponent,
  LayoutThemeOneComponent,
  HeaderNotificationSettingsComponent,
];

// 内部组件
const SELF_COMPONENTS = [
  HeaderFullScreenComponent,
  HeaderI18nComponent,
  HeaderStorageComponent,
  HeaderUserComponent,
  HeaderNotificationsComponent,
  YoYoSidebarNavComponent,
  HeaderComponent,
  SidebarComponent,
  ThemeOneHeaderComponent,
  ThemeOneSidebarComponent,
  ChangePasswordModalComponent,
  LoginAttemptsModalComponent,
  MySettingsModalComponent,
];

// 弹出组件
const ENTRY_COMPONENTS = [
  ChangePasswordModalComponent,
  LoginAttemptsModalComponent,
  MySettingsModalComponent,
  HeaderNotificationSettingsComponent
];


@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    NgZorroAntdModule,
  ],
  declarations: [
    ...SELF_COMPONENTS,
    ...EXPORT_COMPONENTS,
  ],
  entryComponents: [
    ...ENTRY_COMPONENTS,
  ],
  exports: [
    ...EXPORT_COMPONENTS,
  ],
  providers: [
    UserNotificationHelper,
  ],
})
export class LayoutModule {
}
