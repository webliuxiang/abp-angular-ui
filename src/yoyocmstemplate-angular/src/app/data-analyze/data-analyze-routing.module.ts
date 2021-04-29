import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VisualizeComponent } from './visualize/visualize.component';
import { DiscoverComponent } from './discover/discover.component';
import { ReportComponent } from './report/report.component';

import { VisualizeChartComponent } from './visualize/component/visualize-chart/visualize-chart.component';
import { DiscoverDataComponent } from './discover/component/discover-data/discover-data.component';
import { ReportTaskConfigComponent } from './report/component/report-task-config/report-task-config.component';

const routes: Routes = [
  { path: 'discover', component: DiscoverComponent },
  { path: 'visualize', component: VisualizeComponent },
  { path: 'report', component: ReportComponent },

  { path: 'create-visualize', component: VisualizeChartComponent },
  { path: 'create-discover', component: DiscoverDataComponent },
  { path: 'create-report', component: ReportTaskConfigComponent },
  {
    path: '**',
    redirectTo: 'discover',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DataAnalyzeRoutingModule {}
