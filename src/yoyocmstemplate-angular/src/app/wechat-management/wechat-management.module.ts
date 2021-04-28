import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WechatManagementRoutingModule } from './wechat-management-routing.module';
import { WechatAppConfigComponent } from './wechat-app-configs/wechat-app-config.component';
import { CreateOrEditWechatAppConfigComponent } from './wechat-app-configs/create-or-edit-wechat-app-config/create-or-edit-wechat-app-config.component';
import { AbpModule } from 'abp-ng2-module';
import { SharedModule } from '@shared/shared.module';
import { CreateOrEditWechatMenuComponent } from './create-or-edit-wechat-menu/create-or-edit-wechat-menu.component';
import { EditMenuViewComponent } from './create-or-edit-wechat-menu/edit-menu-view/edit-menu-view.component';
import { WechatMenuButtonComponent } from './components/wechat-menu-button/wechat-menu-button.component';
import { EditMenuConfigComponent } from './create-or-edit-wechat-menu/edit-menu-config/edit-menu-config.component';
import { EditMenuConditionalComponent } from './create-or-edit-wechat-menu/edit-menu-conditional/edit-menu-conditional.component';
import { WechatMaterialManagementComponent } from './wechat-material-management/wechat-material-management.component';
import { CreateOrEditImgTextMaterialComponent } from './wechat-material-management/create-or-edit-img-text-material/create-or-edit-img-text-material.component';
import { CreateImageMaterialComponent } from './wechat-material-management/create-image-material/create-image-material.component';
import { CreateVoiceMaterialComponent } from './wechat-material-management/create-voice-material/create-voice-material.component';
import { CreateVideoMaterialComponent } from './wechat-material-management/create-video-material/create-video-material.component';
import { WechatMaterialImgTextViewComponent } from './wechat-material-management/wechat-material-img-text-view/wechat-material-img-text-view.component';
import { WechatMaterialImgViewComponent } from './wechat-material-management/wechat-material-img-view/wechat-material-img-view.component';
import { WechatMaterialVoiceViewComponent } from './wechat-material-management/wechat-material-voice-view/wechat-material-voice-view.component';
import { WechatMaterialVideoViewComponent } from './wechat-material-management/wechat-material-video-view/wechat-material-video-view.component';

@NgModule({
  imports: [
    CommonModule,
    WechatManagementRoutingModule,
    SharedModule,
    AbpModule,
  ],
  declarations: [
    WechatAppConfigComponent,
    CreateOrEditWechatAppConfigComponent,
    CreateOrEditWechatMenuComponent,
    WechatMenuButtonComponent,
    EditMenuViewComponent,
    EditMenuConfigComponent,
    EditMenuConditionalComponent,
    WechatMaterialManagementComponent,
    CreateOrEditImgTextMaterialComponent,
    CreateImageMaterialComponent,
    CreateVoiceMaterialComponent,
    CreateVideoMaterialComponent,
    WechatMaterialImgTextViewComponent,
    WechatMaterialImgViewComponent,
    WechatMaterialVoiceViewComponent,
    WechatMaterialVideoViewComponent,
  ],
  entryComponents: [
    CreateOrEditWechatAppConfigComponent,
    CreateImageMaterialComponent,
    CreateVoiceMaterialComponent,
    CreateVideoMaterialComponent,
  ],
  providers: [

  ]
})
export class WechatManagementModule { }
