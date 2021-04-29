import { Component, OnInit, Input, ViewChild } from '@angular/core';

@Component({
  selector: 'app-chart-module',
  templateUrl: './chart-module.component.html',
  styleUrls: ['./chart-module.component.less'],
})
export class ChartModuleComponent implements OnInit {
  @Input() chartSetting;
  @ViewChild('barChart', { static: false }) barChart;
  isGroupType = true;
  constructor() {}

  ngOnInit() {}
  // 柱状图切换显示
  switchChartType() {
    this.isGroupType = !this.isGroupType;
    this.barChart.switchChartType();
  }
}
