import { Component, Injector, NgZone, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { IFormattedUserNotification, UserNotificationHelper } from '@shared/helpers/UserNotificationHelper';
import { NotificationServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '../../../abpPro/AppConsts';
import { finalize } from 'rxjs/operators';
import { HeaderNotificationSettingsComponent } from '../header-notification-settings/header-notification-settings.component';

@Component({
  selector: 'header-notifications',
  templateUrl: './header-notifications.component.html',
  styles: [],
})
export class HeaderNotificationsComponent extends AppComponentBase implements OnInit {

  notifications: IFormattedUserNotification[] = [];
  unreadNotificationCount = 0;
  loading: boolean;

  constructor(
    injector: Injector,
    private _notificationService: NotificationServiceProxy,
    private _userNotificationHelper: UserNotificationHelper,
    public _zone: NgZone,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loadNotifications();
    this.registerToEvents();
  }

  loadNotifications(): void {
    this.loading = true;
    // this._notificationService.getPagedUserNotifications(undefined, AppConsts.notificationCount, 0)
    //   .pipe(finalize(() => {
    //     this.loading = false;
    //   }))
    //   .subscribe(result => {
    //     this.unreadNotificationCount = result.unreadCount;
    //     this.notifications = [];
    //     result.items.forEach((item) => {
    //       this.notifications.push(this._userNotificationHelper.format(item as any));
    //     });
    //   });
  }

  registerToEvents() {

    abp.event.on('abp.notifications.received', userNotification => {
      this._zone.run(() => {
        this._userNotificationHelper.show(userNotification);
        this.loadNotifications();
      });
    });

    abp.event.on('app.notifications.refresh', () => {
      this._zone.run(() => {
        this.loadNotifications();
      });
    });


    abp.event.on('app.notifications.read', userNotificationId => {
      this._zone.run(() => {
        for (let i = 0; i < this.notifications.length; i++) {
          if (this.notifications[i].userNotificationId === userNotificationId) {
            this.notifications[i].state = 'READ';
          }
        }
        this.unreadNotificationCount -= 1;
      });
    });

  }

  setAllNotificationsAsRead(): void {
    this._userNotificationHelper.setAllAsRead();
  }

  chantNotificationSettings(): void {
    this.modalHelper.create(HeaderNotificationSettingsComponent)
      .subscribe((res) => {

      });
  }

  setNotificationAsRead(userNotification: IFormattedUserNotification): void {
    this._userNotificationHelper.setAsRead(userNotification.userNotificationId);
  }

  gotoUrl(url): void {
    if (url) {
      location.href = url;
    }
  }
}
