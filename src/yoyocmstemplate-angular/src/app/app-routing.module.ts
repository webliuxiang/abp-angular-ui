import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { NotificationsComponent } from './notifications/notifications.component';
import { AppComponent } from '@app/app.component';

const routes: Routes = [
  {
    path: '',
    component: AppComponent,
    canActivate: [AppRouteGuard],
    canActivateChild: [AppRouteGuard],
    children: [
      {
        path: 'main',
        loadChildren: () => import('./main/main.module').then(m => m.MainModule), // Lazy load main module
        data: { preload: true },
      },
      {
        path: 'admin',
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule), // Lazy load admin module
        data: { preload: true },
      },

      {
        path: 'notifications',
        component: NotificationsComponent,
        data: { titleI18n: 'Notifications', reuse: true },
      },
      {
        path: 'wechat',
        loadChildren: () => import('./wechat-management/wechat-management.module').then(m => m.WechatManagementModule), // Lazy load wechat module
        data: { preload: true },
      },
      {
        path: 'demo',
        loadChildren: () => import('./demo-management/demo-management.module').then(m => m.DemoManagementModule), // Lazy load wechat module
        data: { preload: true },
      },
      {
        path: 'other',
        loadChildren: () => import('./other/other.module').then(m => m.OtherModule), // Lazy load wechat module
        data: { preload: true },
      },
      { path: 'blogging', loadChildren: () => import('./blogging/blogging.module').then(m => m.BloggingModule) },
      { path: 'website', loadChildren: () => import('./webSite/webSite.module').then(m => m.WebSiteModule) },
      { path: 'marketing', loadChildren: () => import('./marketing/marketing.module').then(m => m.MarketingModule) },
      { path: 'mooc', loadChildren: () => import('./mooc/mooc.module').then(m => m.MoocModule) },
      { path: 'other-course-orders', loadChildren: () => import('./other-course-orders/other-course-orders.module').then(m => m.OtherCourseOrdersModule) },
      { path: 'data-analysis', loadChildren: () => import('./data-analysis/data-analysis.module').then(m => m.DataAnalysisModule) },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
