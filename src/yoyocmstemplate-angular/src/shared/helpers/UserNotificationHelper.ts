import { Injectable, Injector } from '@angular/core';
import {
  NotificationServiceProxy,
  EntityDtoOfGuid
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase, ModalComponentBase } from '@shared/component-base';

import * as moment from 'moment';
// import * as Push from 'push.js'; // if using ES6
import { AppConsts } from 'abpPro/AppConsts';

export interface IFormattedUserNotification {
  userNotificationId: string;
  text: string;
  time: string;
  creationTime: Date;
  icon: string;
  state: string;
  data: any;
  url: string;
  isUnread: boolean;
}

@Injectable()
export class UserNotificationHelper {
  constructor(
    // injector: Injector,
    private _notificationService: NotificationServiceProxy
  ) {
    // super(injector);
  }

  getUrl(userNotification: abp.notifications.IUserNotification): string {
    switch (userNotification.notification.notificationName) {
      case 'App.NewUserRegistered':
        return (
          '/app/admin/users?filterText=' +
          userNotification.notification.data.properties.emailAddress
        );
      case 'App.NewTenantRegistered':
        return (
          '/app/admin/tenants?filterText=' +
          userNotification.notification.data.properties.tenancyName
        );
      // Add your custom notification names to navigate to a URL when user clicks to a notification.
    }

    // No url for this notification
    return '';
  }

  /* PUBLIC functions ******************************************/

  getUiIconBySeverity(severity: abp.notifications.severity): string {
    switch (severity) {
      case abp.notifications.severity.SUCCESS:
        return 'success';
      case abp.notifications.severity.WARN:
        return 'warning';
      case abp.notifications.severity.ERROR:
        return 'error';
      case abp.notifications.severity.FATAL:
        return 'fatal';
      case abp.notifications.severity.INFO:
      default:
        return 'default';
    }
  }

  format(
    userNotification: abp.notifications.IUserNotification,
    truncateText?: boolean
  ): IFormattedUserNotification {
    const formatted: IFormattedUserNotification = {
      userNotificationId: userNotification.id,
      text: abp.notifications.getFormattedMessageFromUserNotification(
        userNotification
      ),
      time: moment(userNotification.notification.creationTime).format(
        'YYYY-MM-DD HH:mm:ss'
      ),
      creationTime: userNotification.notification.creationTime,
      icon: this.getUiIconBySeverity(userNotification.notification.severity),
      state: abp.notifications.getUserNotificationStateAsString(
        userNotification.state
      ),
      data: userNotification.notification.data,
      url: this.getUrl(userNotification),
      isUnread:
        userNotification.state ===
        abp.notifications.userNotificationState.UNREAD
    };

    if (truncateText || truncateText === undefined) {
      formatted.text = abp.utils.truncateStringWithPostfix(formatted.text, 100);
    }

    return formatted;
  }

  show(userNotification: abp.notifications.IUserNotification): void {
    // Application notification
    abp.notifications.showUiNotifyForUserNotification(userNotification, {
      onclick: () => {
        // Take action when user clicks to live toastr notification
        const url = this.getUrl(userNotification);
        if (url) {
          location.href = url;
        }
      }
    });

    // // 唤起桌面通知
    // Push.create("WebManagement", {
    //     body: this.format(userNotification).text,
    //     icon: AppConsts.appBaseUrl + '/assets/images/logos/logo-white-shield.svg',
    //     timeout: 6000,
    //     onClick: function () {
    //         window.focus();
    //         this.close();
    //     }
    // });
  }

  setAllAsRead(callback?: () => void): void {
    this._notificationService.makeAllUserNotificationsAsRead().subscribe(() => {
      abp.event.trigger('app.notifications.refresh');
      callback && callback();
    });
  }

  setAsRead(
    userNotificationId: string,
    callback?: (userNotificationId: string) => void
  ): void {
    const input = new EntityDtoOfGuid();
    input.id = userNotificationId;
    this._notificationService.makeNotificationAsRead(input).subscribe(() => {
      abp.event.trigger('app.notifications.read', userNotificationId);
      callback && callback(userNotificationId);
    });
  }
}
