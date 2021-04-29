import { Component, Input, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-config-module-bucket',
  templateUrl: './config-module.component.html',
  styles: [],
})
export class ConfigModuleBucketComponent implements OnInit {
  @Input() bucketData;
  @Input() metric;
  @Input() metricsList;
  @ViewChild('date_histogramFun', { static: false }) date_histogramFun;
  @ViewChild('filtersFun', { static: false }) filtersFun;
  @ViewChild('termsFun', { static: false }) termsFun;

  bucketItemOption = {};

  constructor() {}

  ngOnInit() {}
  buildBucketItemOption() {
    switch (this.bucketData.bucketsAggregation.value) {
      case 'date_histogram':
        this.bucketItemOption = this.date_histogramFun.buildBucketTermsOption();
        break;
      case 'filters':
        this.bucketItemOption = this.filtersFun.buildBucketTermsOption();
        break;
      case 'terms':
        this.bucketItemOption = this.termsFun.buildBucketTermsOption();
        break;

      default:
        break;
    }
    return this.bucketItemOption;
  }
}
