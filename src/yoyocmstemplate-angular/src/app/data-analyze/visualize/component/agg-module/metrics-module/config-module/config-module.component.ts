import { Component, Input, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-config-module-metric',
  templateUrl: './config-module.component.html',
  styleUrls: ['./config-module.component.less'],
})
export class ConfigModuleMetricComponent implements OnInit {
  @Input() metricData;
  @ViewChild('countFun', { static: false }) countFun;
  @ViewChild('avgFun', { static: false }) avgFun;
  @ViewChild('minFun', { static: false }) minFun;
  @ViewChild('maxFun', { static: false }) maxFun;
  @ViewChild('sumFun', { static: false }) sumFun;

  metricItemOption = {};
  constructor() {}
  ngOnInit() {
    // console.log(this.metricData);
  }
  buildMetricItemOption() {
    // console.log('调用 config module buildMetricItemOption');
    switch (this.metricData.metricsAggregation.value) {
      case 'count':
        this.metricItemOption = this.countFun.buildMetricAvgOption();
        break;
      case 'avg':
        this.metricItemOption = this.avgFun.buildMetricAvgOption();
        break;
      case 'min':
        this.metricItemOption = this.minFun.buildMetricMinOption();
        break;
      case 'max':
        this.metricItemOption = this.maxFun.buildMetricMaxOption();
        break;
      case 'sum':
        this.metricItemOption = this.sumFun.buildMetricSumOption();
        break;

      default:
        break;
    }
    // console.log(this.metricItemOption);
    return this.metricItemOption;
  }
}
