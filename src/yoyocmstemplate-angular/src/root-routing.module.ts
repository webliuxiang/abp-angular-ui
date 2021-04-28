import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'app',
    loadChildren: () => import('./app/app.module').then(m => m.AppModule), // Lazy load app module
    // loadChildren: () => import('./app/app.module').then(o => o.AppModule), // Lazy load app module
    data: { preload: true },
  },
  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then(o => o.AccountModule), // Lazy load account module
    data: { preload: true },
  },
  {
    path: '**',
    redirectTo: 'app/main/',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [],
})
export class RootRoutingModule {
}
