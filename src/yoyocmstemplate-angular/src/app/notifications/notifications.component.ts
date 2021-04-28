import {
  NotificationServiceProxy,
  UserNotification
} from '@shared/service-proxies/service-proxies';
import { Component, OnInit, Injector } from '@angular/core';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from '@shared/component-base';
import { finalize } from 'rxjs/operators';
import {
  UserNotificationHelper,
  IFormattedUserNotification
} from '@shared/helpers/UserNotificationHelper';
import { AppUserNotificationState } from 'abpPro/AppEnums';
import { ReuseTabService } from '@delon/abc';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styles: []
})
export class NotificationsComponent extends PagedListingComponentBase<
  IFormattedUserNotification
> {
  filter = {
    state: undefined
  };

  stateList = [
    { key: this.l('NotificationRead'), value: AppUserNotificationState.Read },
    {
      key: this.l('NotificationUnread'),
      value: AppUserNotificationState.Unread
    }
  ];

  constructor(
    injector: Injector,
    private _notificationService: NotificationServiceProxy,
    private _userNotificationHelper: UserNotificationHelper,
    private _reuseTabService: ReuseTabService
  ) {
    super(injector);

    this._reuseTabService.title = this.l('Notifications');
  }

  protected fetchDataList(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    this._notificationService
      .getPagedUserNotifications(
        this.filter.state,
        request.maxResultCount,
        request.skipCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe(result => {
        this.dataList = this.formatNotifications(result.items);
        this.showPaging(result);
      });
  }

  setAsRead(item: UserNotification) {
    this._userNotificationHelper.setAsRead(item.id, () => {
      this.refresh();
    });
  }

  delete(item: UserNotification) {
    this._notificationService.deleteNotification(item.id).subscribe(() => {
      this.notify.success(this.l('SuccessfullyDeleted'));
      this.refreshGoFirstPage();
    });
  }

  setAllUserNotificationsAsRead() {
    this._notificationService.makeAllUserNotificationsAsRead().subscribe(() => {
      this.refresh();
    });
  }

  formatNotifications(records: any[]): any[] {
    const formattedRecords = [];
    for (const record of records) {
      record.formattedNotification = this.formatRecord(record);
      formattedRecords.push(record);
    }
    return formattedRecords;
  }

  formatRecord(record: any): IFormattedUserNotification {
    return this._userNotificationHelper.format(record, false);
  }
}
