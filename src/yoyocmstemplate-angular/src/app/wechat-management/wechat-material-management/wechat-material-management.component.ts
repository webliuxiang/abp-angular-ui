import { WechatMediaServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import { WechatMaterialImgTextViewComponent } from './wechat-material-img-text-view/wechat-material-img-text-view.component';
import { WechatMaterialImgViewComponent } from './wechat-material-img-view/wechat-material-img-view.component';
import { WechatMaterialVoiceViewComponent } from './wechat-material-voice-view/wechat-material-voice-view.component';
import { WechatMaterialVideoViewComponent } from './wechat-material-video-view/wechat-material-video-view.component';

@Component({
  selector: 'app-wechat-material-management',
  templateUrl: './wechat-material-management.component.html',
  styles: []
})
export class WechatMaterialManagementComponent extends AppComponentBase
  implements OnInit {
  @ViewChild(WechatMaterialImgTextViewComponent)
  imgTextMaterialView: WechatMaterialImgTextViewComponent;

  @ViewChild(WechatMaterialImgViewComponent)
  imgMaterialView: WechatMaterialImgViewComponent;

  @ViewChild(WechatMaterialVoiceViewComponent)
  voiceMaterialView: WechatMaterialVoiceViewComponent;

  @ViewChild(WechatMaterialVideoViewComponent)
  videoMaterialView: WechatMaterialVideoViewComponent;

  wechatMediaService: WechatMediaServiceProxy;

  appId: string;
  appName: string;
  constructor(
    injector: Injector,
    private activatedRoute: ActivatedRoute,
    private reuseTabService: ReuseTabService,
    private _wechatMediaService: WechatMediaServiceProxy
  ) {
    super(injector);
    this.wechatMediaService = _wechatMediaService;
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      // 初始化获取数据,如果未获取到,跳转到管理列表页
      this.appId = params.appId;
      this.appName = params.appName;
      if (!this.appId || !this.appName) {
        this.reuseTabService.replace('/app/wechat/wechat-app-config');
        return;
      }
      const currentTitle =
        this.l('WechatMaterialManagement') + ':' + this.appName;
      this.reuseTabService.title = currentTitle;
      this.titleSrvice.setTitle(currentTitle);
    });
  }
}
