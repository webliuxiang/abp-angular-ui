import {message as Msg, Modal, notification} from 'ant-design-vue';


export class MessageExtension {
    /**
     * 覆盖abp.message替换为ng-zorro的全局message
     * @param _nzMessageService
     * @param _nzModalService
     */

    public static overrideAbpMessageByMessage() {
        abp.message.info = (message: string, title?: string) => {
            Msg.info(message);
        };

        abp.message.warn = (message: string, title?: string) => {
            Msg.warning(message);
        };
        abp.message.error = (message: string, title?: string) => {
            Msg.error(message);
        };
        abp.message.success = (message: string, title?: string) => {
            Msg.success(message);
        };
        abp.message.confirm = this.confirm;
    }

    /**
     * 覆盖abp.message替换为 antd 的模态框
     */
    public static overrideAbpMessageByModal() {

        abp.message.info = (message: string, title?: string) => {
            Modal.info({
                title,
                content: message,
                mask: true
            });
        };

        abp.message.warn = (message: string, title?: string) => {
            Modal.warning({
                title,
                content: message,
                mask: true
            });
        };
        abp.message.error = (message: string, title?: string) => {
            Modal.error({
                title,
                content: message,
                mask: true
            });
        };
        abp.message.success = (message: string, title?: string) => {
            Modal.success({
                title,
                content: message,
                mask: true
            });
        };
        abp.message.confirm = this.confirm;
    }

    /**
     * 覆盖abp.message替换为 antd 的notify
     */
    public static overrideAbpNotify() {


        abp.notify.info = (message: string, title?: string, options?: any) => {
            if (!options) {
                options = {};
            }
            notification.info({
                message: title,
                description: message,
                ...options
            });
        };
        abp.notify.warn = (message: string, title?: string, options?: any) => {
            if (!options) {
                options = {};
            }
            notification.warn({
                message: title,
                description: message,
                ...options
            });
        };
        abp.notify.error = (message: string, title?: string, options?: any) => {
            if (!options) {
                options = {};
            }
            notification.error({
                message: title,
                description: message,
                ...options
            });
        };
        abp.notify.success = (message: string, title?: string, options?: any) => {
            if (!options) {
                options = {};
            }
            notification.success({
                message: title,
                description: message,
                ...options
            });
        };
    }

    // msg confirm
    private static confirm(
        message: string,
        callback?: (result: boolean) => void
    ): any;

    private static confirm(
        message: string,
        title?: string,
        callback?: (result: boolean) => void
    ): any;

    private static confirm(
        message: string,
        titleOrCallBack?: string | ((result: boolean) => void),
        callback?: (result: boolean) => void
    ): any {
        if (typeof titleOrCallBack === 'string') {

            Modal.confirm({
                title: titleOrCallBack,
                content: message,
                onOk() {
                    if (callback) callback(true);
                },
                onCancel() {
                    if (callback) callback(false);
                }
            });

        } else {
            Modal.confirm({
                title: abp.localization.localize(
                    'MessageConfirmOperation',
                    abp.localization.defaultSourceName
                ),
                content: message,
                onOk() {
                    if (titleOrCallBack) titleOrCallBack(true);
                },
                onCancel() {
                    if (titleOrCallBack) titleOrCallBack(false);
                },
            });
        }
    }
}
