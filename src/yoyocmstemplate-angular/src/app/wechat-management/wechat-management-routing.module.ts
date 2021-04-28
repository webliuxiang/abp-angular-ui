import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WechatAppConfigComponent } from './wechat-app-configs/wechat-app-config.component';
import { CreateOrEditWechatMenuComponent } from './create-or-edit-wechat-menu/create-or-edit-wechat-menu.component';
import { WechatMaterialManagementComponent } from './wechat-material-management/wechat-material-management.component';
import { CreateOrEditImgTextMaterialComponent } from './wechat-material-management/create-or-edit-img-text-material/create-or-edit-img-text-material.component';

const routes: Routes = [
  { path: 'wechat-app-config', component: WechatAppConfigComponent },
  { path: 'create-or-edit-wechat-menu', component: CreateOrEditWechatMenuComponent, data: { reuse: true } },
  { path: 'wechat-materials', component: WechatMaterialManagementComponent, data: { reuse: true } },
  { path: 'create-or-edit-img-text-material', component: CreateOrEditImgTextMaterialComponent, data: { reuse: true } },
  {
    path: '**',
    redirectTo: 'wechat-app-configs',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WechatManagementRoutingModule { }
