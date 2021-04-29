import { Component, OnInit } from '@angular/core';
import { TransformDataService } from '@app/data-analyze/transform-data.service';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

@Component({
  selector: 'app-table-chart',
  templateUrl: './table-chart.component.html',
  styles: [],
})
export class TableChartComponent implements OnInit {
  constructor(
    private _transformDataService: TransformDataService,
    private _aggregationDataService: DataAnalyzeService,
  ) {}

  displayData = false;
  chartData = {
    tableHeader: null,
    tablerows: null,
  };

  ngOnInit() {
    this._aggregationDataService.getChartMessage().subscribe(result => {
      this.chartData = this._transformDataService.transformData(result);
      this.displayData = true;
      // console.log(this.chartData);
    });
  }
}
