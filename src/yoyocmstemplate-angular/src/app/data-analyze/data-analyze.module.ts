import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '@shared/shared.module';

import { DataAnalyzeRoutingModule } from './data-analyze-routing.module';
import { VisualizeComponent } from './visualize/visualize.component';
import { DiscoverComponent } from './discover/discover.component';
import { CreateChartComponent } from './visualize/component/create-chart-modal/create-chart-modal.component';
import { VisualizeChartComponent } from './visualize/component/visualize-chart/visualize-chart.component';
import { DateTimeComponent } from './component/date-time/date-time.component';
import { CreateQueryTagModalComponent } from './component/query-module/create-query-tag-modal/create-query-tag-modal.component';
import { QueryModuleComponent } from './component/query-module/query-module.component';
import { AggModuleComponent } from './visualize/component/agg-module/agg-module.component';
import { MetricsModuleComponent } from './visualize/component/agg-module/metrics-module/metrics-module.component';
import { BucketsModuleComponent } from './visualize/component/agg-module/buckets-module/buckets-module.component';
import { ChartModuleComponent } from './visualize/component/chart-module/chart-module.component';
import { ChartConfigModuleComponent } from './visualize/component/chart-config-module/chart-config-module.component';
import { SumModuleComponent } from './visualize/component/agg-module/metrics-module/component/sum-module/sum-module.component';
import { MinModuleComponent } from './visualize/component/agg-module/metrics-module/component/min-module/min-module.component';
import { MaxModuleComponent } from './visualize/component/agg-module/metrics-module/component/max-module/max-module.component';
import { AvgModuleComponent } from './visualize/component/agg-module/metrics-module/component/avg-module/avg-module.component';
import { DateHistogramModuleComponent } from './visualize/component/agg-module/buckets-module/component/date-histogram-module/date-histogram-module.component';
import { FilterModuleComponent } from './visualize/component/agg-module/buckets-module/component/filter-module/filter-module.component';
import { TermsModuleComponent } from './visualize/component/agg-module/buckets-module/component/terms-module/terms-module.component';
import { ConfigModuleMetricComponent } from './visualize/component/agg-module/metrics-module/config-module/config-module.component';
import { ConfigModuleBucketComponent } from './visualize/component/agg-module/buckets-module/config-module/config-module.component';

import { CountModuleComponent } from './visualize/component/agg-module/metrics-module/component/count-module/count-module.component';
import { TableChartComponent } from './visualize/component/chart-module/component/table-chart/table-chart.component';
import { BarChartComponent } from './visualize/component/chart-module/component/bar-chart/bar-chart.component';
import { LineChartComponent } from './visualize/component/chart-module/component/line-chart/line-chart.component';
import { AreaChartComponent } from './visualize/component/chart-module/component/area-chart/area-chart.component';
import { CreateDiscoverModalComponent } from './discover/component/create-discover-modal/create-discover-modal.component';
import { DiscoverDataComponent } from './discover/component/discover-data/discover-data.component';
import { FieldsModuleComponent } from './discover/component/fields-module/fields-module.component';
import { SourceDataModuleComponent } from './discover/component/source-data-module/source-data-module.component';
import { PieChartComponent } from './visualize/component/chart-module/component/pie-chart/pie-chart.component';
import { ReportComponent } from './report/report.component';
import { ReportTaskConfigComponent } from './report/component/report-task-config/report-task-config.component';

const COMPONENTS = [
  CreateChartComponent,
  CreateQueryTagModalComponent,
  DateTimeComponent,
  VisualizeChartComponent,
  CreateDiscoverModalComponent,
  DiscoverDataComponent,
];
@NgModule({
  declarations: [
    VisualizeComponent,
    DiscoverComponent,
    ...COMPONENTS,
    QueryModuleComponent,
    AggModuleComponent,
    MetricsModuleComponent,
    BucketsModuleComponent,
    ChartModuleComponent,
    ChartConfigModuleComponent,
    SumModuleComponent,
    MinModuleComponent,
    MaxModuleComponent,
    AvgModuleComponent,
    DateHistogramModuleComponent,
    FilterModuleComponent,
    TermsModuleComponent,
    ConfigModuleMetricComponent,
    ConfigModuleBucketComponent,
    CountModuleComponent,
    TableChartComponent,
    BarChartComponent,
    LineChartComponent,
    AreaChartComponent,
    FieldsModuleComponent,
    SourceDataModuleComponent,
    PieChartComponent,
    ReportComponent,
    ReportTaskConfigComponent,
  ],
  imports: [CommonModule, SharedModule, DataAnalyzeRoutingModule],
  entryComponents: [...COMPONENTS],
  providers: [],
})
export class DataAnalyzeModule {}
