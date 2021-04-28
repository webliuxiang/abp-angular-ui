import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DownloadLogComponent } from './download-logs/download-log.component';
import { UserDownloadConfigComponent } from './user-download-config/user-download-config.component';

const routes: Routes = [
  { path: 'download-log', component: DownloadLogComponent },
  { path: 'user-download-config', component: UserDownloadConfigComponent, data: { permission: 'Pages.UserDownloadConfig' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DataAnalysisRoutingModule { }
