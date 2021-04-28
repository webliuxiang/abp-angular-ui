import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';

export class MessageExtension {
  /**
   * 覆盖abp.message替换为ng-zorro的全局message
   * @param _nzMessageService
   * @param _nzModalService
   */

  static overrideAbpMessageByMini(_nzMessageService: NzMessageService, _nzModalService?: NzModalService) {
    if (_nzModalService) {
      if ((abp as any).nzModal) {
        return;
      }

      (abp as any).nzModal = _nzModalService;
    }
    if ((abp as any).nzMessage) {
      return;
    }
    (abp as any).nzMessage = _nzMessageService;

    abp.message.info = (message: string, title?: string) => {
      //   const timingCounts = parseInt(title, 0);
      // (<any>abp).nzMessage.info(message, { nzDuration: timingCounts });
      (abp as any).nzMessage.info(message);
    };

    abp.message.warn = (message: string, title?: string) => {
      (abp as any).nzMessage.warning(message);
    };
    abp.message.error = (message: string, title?: string) => {
      (abp as any).nzMessage.error(message);
    };
    abp.message.success = (message: string, title?: string) => {
      (abp as any).nzMessage.success(message);
    };
    abp.message.confirm = this.confirm;
  }

  /**
   * 覆盖abp.message替换为ng-zorro的模态框
   * @param _nzModalService
   */
  static overrideAbpMessageByNgModal(_nzModalService: NzModalService) {
    if ((abp as any).nzModal) {
      return;
    }

    (abp as any).nzModal = _nzModalService;

    abp.message.info = (message: string, title?: string) => {
      (abp as any).nzModal.info({
        nzTitle: title,
        nzContent: message,
        nzMask: true,
      });
    };

    abp.message.warn = (message: string, title?: string) => {
      (abp as any).nzModal.warning({
        nzTitle: title,
        nzContent: message,
        nzMask: true,
      });
    };
    abp.message.error = (message: string, title?: string) => {
      (abp as any).nzModal.error({
        nzTitle: title,
        nzContent: message,
        nzMask: true,
      });
    };
    abp.message.success = (message: string, title?: string) => {
      (abp as any).nzModal.success({
        nzTitle: title,
        nzContent: message,
        nzMask: true,
      });
    };
    abp.message.confirm = this.confirm;
  }

  // 覆盖abp.message替换为ng-zorro的notify
  static overrideAbpNotify(_nzNotificationService: NzNotificationService) {
    if ((abp as any).nzNotify) {
      return;
    }

    (abp as any).nzNotify = _nzNotificationService;

    abp.notify.info = (message: string, title?: string, options?: any) => {
      (abp as any).nzNotify.info(message, title, options);
    };
    abp.notify.warn = (message: string, title?: string, options?: any) => {
      (abp as any).nzNotify.warning(message, title, options);
    };
    abp.notify.error = (message: string, title?: string, options?: any) => {
      (abp as any).nzNotify.error(message, title, options);
    };
    abp.notify.success = (message: string, title?: string, options?: any) => {
      (abp as any).nzNotify.success(message, title, options);
    };
  }

  // msg confirm
  private static confirm(message: string, callback?: (result: boolean) => void): any;

  private static confirm(message: string, title?: string, callback?: (result: boolean) => void): any;

  private static confirm(
    message: string,
    titleOrCallBack?: string | ((result: boolean) => void),
    callback?: (result: boolean) => void,
  ): any {
    if (typeof titleOrCallBack === 'string' || typeof titleOrCallBack === 'undefined') {
      (abp as any).nzModal.confirm({
        nzTitle: titleOrCallBack,
        nzContent: message,
        nzOnOk() {
          if (callback) {
            callback(true);
          }
        },
        nzOnCancel() {
          if (callback) {
            callback(false);
          }
        },
      });
    } else {
      (abp as any).nzModal.confirm({
        nzTitle: abp.localization.localize('MessageConfirmOperation', abp.localization.defaultSourceName),
        nzContent: message,
        nzOnOk() {
          if (titleOrCallBack) {
            titleOrCallBack(true);
          }
        },
        nzOnCancel() {
          if (titleOrCallBack) {
            titleOrCallBack(false);
          }
        },
      });
    }
  }
}
