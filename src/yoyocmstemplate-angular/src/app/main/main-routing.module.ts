import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from '@app/main/dashboard/dashboard.component';
import { AboutComponent } from './about/about.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'about', component: AboutComponent },

      {
        path: '**',
        redirectTo: 'dashboard',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MainRoutingModule {}
