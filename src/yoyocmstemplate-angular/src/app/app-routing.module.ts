import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
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
        path: 'alert-management',
        loadChildren: () => import('./alert-management/alert-management.module').then(m => m.AlertManagementModule), // Lazy load admin module
        data: { preload: true },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
