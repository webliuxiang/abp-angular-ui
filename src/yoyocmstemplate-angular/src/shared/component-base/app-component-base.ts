import { Injector, ElementRef, Directive } from '@angular/core';
import { AppSessionService } from '@shared/session/app-session.service';
import {
  FeatureCheckerService,
  NotifyService,
  SettingService,
  MessageService,
  AbpMultiTenancyService,
  AbpSessionService,
} from 'abp-ng2-module';
import { ModalHelper, TitleService } from '@delon/theme';
import { LoadingService } from '@shared/utils/global-loading/loading.service';
import { SFSchemaEnumType } from '@delon/form';
import { SampleComponentBase } from './sample-component-base';

export abstract class AppComponentBase extends SampleComponentBase {


  feature: FeatureCheckerService;
  notify: NotifyService;
  setting: SettingService;
  message: MessageService;
  multiTenancy: AbpMultiTenancyService;
  appSession: AppSessionService;
  elementRef: ElementRef;
  modalHelper: ModalHelper;
  titleSrvice: TitleService;
  abpSession: AbpSessionService;

  /**
   * 保存状态
   */
  saving = false;

  loadingService: LoadingService;

  constructor(injector: Injector) {
    super(injector);
    this.feature = injector.get(FeatureCheckerService);
    this.notify = injector.get(NotifyService);
    this.setting = injector.get(SettingService);
    this.message = injector.get(MessageService);
    this.multiTenancy = injector.get(AbpMultiTenancyService);
    this.appSession = injector.get(AppSessionService);
    this.elementRef = injector.get(ElementRef);
    this.modalHelper = injector.get(ModalHelper);
    this.titleSrvice = injector.get(TitleService);
    this.abpSession = injector.get(AbpSessionService);
    this.loadingService = injector.get(LoadingService);
  }


  /** 将枚举转换SFS的枚举数组 */
  ConvertToSFSchemaEnumType(listEnumType: any) {
    const temp: SFSchemaEnumType[] = [];

    listEnumType.map(item => {
      temp.push({
        label: item.key,
        value: item.value,
      });
    });

    return temp;
  }

}
