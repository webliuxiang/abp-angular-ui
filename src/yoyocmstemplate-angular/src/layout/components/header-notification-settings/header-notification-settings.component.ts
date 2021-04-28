import { Component, Injector, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base';
import {
  GetNotificationSettingsOutput,
  NotificationServiceProxy,
  NotificationSubscriptionDto,
  UpdateNotificationSettingsInput,
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import * as _ from 'lodash';

@Component({
  selector: 'header-notification-settings',
  templateUrl: './header-notification-settings.component.html',
  styles: [],
})
export class HeaderNotificationSettingsComponent extends ModalComponentBase implements OnInit {
  settings: GetNotificationSettingsOutput = new GetNotificationSettingsOutput();
  loding: boolean;

  constructor(injector: Injector, private _notificationService: NotificationServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    this.loding = true;
    this._notificationService
      .getNotificationSettings()
      .pipe(
        finalize(() => {
          this.loding = false;
        }),
      )
      .subscribe((result: GetNotificationSettingsOutput) => {
        this.settings = result;
      });
  }

  save() {
    this.saving = true;

    const input = new UpdateNotificationSettingsInput();
    input.receiveNotifications = this.settings.receiveNotifications;
    input.notifications = _.map(this.settings.notifications, n => {
      const subscription = new NotificationSubscriptionDto();
      subscription.name = n.name;
      subscription.isSubscribed = n.isSubscribed;
      return subscription;
    });

    this._notificationService
      .updateNotificationSettings(input)
      .pipe(
        finalize(() => {
          this.saving = false;
        }),
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.close();
      });
  }
}
