import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import { _HttpClient } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd/message';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AdvertisingComponent } from '../advertising/advertising.component';
import { AuditLogServiceProxy } from '@shared/service-proxies';
import * as moment from 'moment';

import { KtdGridLayout, ktdTrackById } from '@katoid/angular-grid-layout';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less'],

  animations: [appModuleAnimation()],
})
export class DashboardComponent extends AppComponentBase implements OnInit {
  constructor(
    injector: Injector,
    private http: _HttpClient,
    public msg: NzMessageService,
  ) {
    super(injector);

  }

  notice: any[] = [
    {
      logo: 'https://docs.microsoft.com/zh-cn/dotnet/images/hub/netcore.svg',
      title: '.NET Core',
      href: 'https://dotnet.github.io/',
      description: '.NET CORE 是微软新一代的跨平台的框架， 是.NET Framework的进化版本,.NET Core的核心点： 创新、开源、跨平台，当前版本为.net core2.x'
    },
    {
      logo: 'https://aspnetboilerplate.com/images/logos/abp-logo-long.png',
      title: 'ABP',
      href: 'https://aspnetboilerplate.com/',
      description: 'ASP.NET Boilerplate是一个用最佳实践和流行技术开发现代WEB应用程序的新起点，它旨在成为一个通用的WEB应用程序框架和项目模板框架'
    },
    {
      logo: 'https://avatars2.githubusercontent.com/u/33684174?s=200&v=4',
      title: '52ABP',
      href: 'https://www.52abp.com',
      description: '52ABP是一个将目前流行的框架进行了整合，核心以ABP框架和ng-zorro为标准进行研发的应用型框架信息。'
    },
    {
      logo: 'https://gw.alipayobjects.com/zos/rmsportal/zOsKZmFRdUtvpqCImOVY.png',
      title: 'Angular',
      href: 'https://aspnetboilerplate.com/',
      description: 'Angular 是一个开发平台。它能帮你更轻松的构建 Web 应用。Angular 集声明式模板、依赖注入、端到端工具和一些最佳实践于一身，为你解决开发方面的各种...'
    },
    {
      logo: 'https://ng.ant.design/assets/img/logo.svg',
      title: 'NG-ZORRO',
      href: 'https://ng.ant.design',
      description: '这里是 Ant Design 的 Angular 实现，开发和服务于企业级后台产品。也是52ABP框架的前端核心之一'
    },
    {
      logo: 'https://ng-alain.com/assets/img/logo-color.svg',
      title: 'NG Alain',
      href: 'https://ng-alain.com/',
      description: '一个基于 Antd的设计，整合了NG-ZORRO的 中后台前端解决方案，提供更多通用性业务模块，让开发者更加专注于业务。'
    }
  ];
  loading = false;

  ngOnInit(): void {

  }

  showAdvertising() {
    this.modalHelper.open(AdvertisingComponent)
      .subscribe(() => {

      });
  }

  cols: number = 6;
  rowHeight: number = 100;
  layout: KtdGridLayout = [
    { id: '0', x: 0, y: 0, w: 1, h: 3 },
    { id: '1', x: 3, y: 0, w: 2, h: 2 },
    { id: '2', x: 0, y: 3, w: 1, h: 2 },
    { id: '3', x: 3, y: 3, w: 2, h: 1 },
  ];
  trackById = ktdTrackById;
  onLayoutUpdated(e) {

  }
}
