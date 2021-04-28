import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { AppConsts } from '../../../../abpPro/AppConsts';

@Component({
  selector: 'app-overlay',
  templateUrl: './overlay.component.html',
  styles: [
    `
      nz-spin {
        display: inline-block;
        margin-right: 16px;
      }
    `
  ]
})
export class OverlayComponent implements OnInit {
  loadingThreeDot: any;

  constructor() {}

  ngOnInit() {
    this.loadingThreeDot = abp.localization.localize(
      'LoadingThreeDot',
      AppConsts.localization.defaultLocalizationSourceName
    );

    // this.loadingThreeDot = '数据加载。。。。。。';
  }
}
