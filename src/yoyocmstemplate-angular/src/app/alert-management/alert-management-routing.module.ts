import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AlertRuleComponent } from './alert-rule/alert-rule.component';
import { AlertSubscribeComponent } from './alert-subscribe/alert-subscribe.component';
import { AlertChannelComponent } from './alert-channel/alert-channel.component';
import { AlertRuleConfigComponent } from './alert-rule/component/alert-rule-config/alert-rule-config.component';

const routes: Routes = [
  {
    path: 'alert-rule',
    component: AlertRuleComponent,
  },
  {
    path: 'alert-rule-config',
    component: AlertRuleConfigComponent,
  },
  {
    path: 'alert-subscribe',
    component: AlertSubscribeComponent,
  },
  {
    path: 'alert-channel',
    component: AlertChannelComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AlertManagementRoutingModule {}
