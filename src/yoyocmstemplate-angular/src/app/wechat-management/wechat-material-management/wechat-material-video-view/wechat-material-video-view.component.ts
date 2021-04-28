import { AppComponentBase } from '@shared/component-base/app-component-base';
import { Component, OnInit, Injector, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import { WechatMediaServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-wechat-material-video-view',
  templateUrl: './wechat-material-video-view.component.html',
  styles: []
})
export class WechatMaterialVideoViewComponent extends AppComponentBase implements OnInit {

  @Input()
  wechatMediaService: WechatMediaServiceProxy;

  constructor(
    injector: Injector,
    private activatedRoute: ActivatedRoute,
    private reuseTabService: ReuseTabService,
  ) {
    super(injector);
  }

  ngOnInit() {
  }

}
