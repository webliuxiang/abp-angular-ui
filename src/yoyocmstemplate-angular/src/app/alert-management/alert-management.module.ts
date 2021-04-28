import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'abp-ng2-module';

import { AlertManagementRoutingModule } from './alert-management-routing.module';
import { AlertRuleComponent } from './alert-rule/alert-rule.component';
import { AlertSubscribeComponent } from './alert-subscribe/alert-subscribe.component';
import { AlertChannelComponent } from './alert-channel/alert-channel.component';
import { AlertRuleConfigComponent } from './alert-rule/component/alert-rule-config/alert-rule-config.component';

@NgModule({
  declarations: [AlertRuleComponent, AlertSubscribeComponent, AlertChannelComponent, AlertRuleConfigComponent],
  imports: [CommonModule, SharedModule, AbpModule, AlertManagementRoutingModule],
  entryComponents: [],
})
export class AlertManagementModule { }
