import { AppConsts } from '@/abpPro/AppConsts';

export class SignalRAspNetCoreHelper {
    public static initSignalR(callback: () => void): void {
        const encryptedAuthToken = abp.utils.getCookieValue(AppConsts.authorization.encrptedAuthTokenName);

        abp.signalr = {
            autoConnect: true,
            connect: undefined,
            hubs: undefined,
            qs: AppConsts.authorization.encrptedAuthTokenName + '=' + encodeURIComponent(encryptedAuthToken),
            remoteServiceBaseUrl: AppConsts.remoteServiceBaseUrl,
            startConnection: undefined,
            url: '/signalr',
        };

        const script = document.createElement('script');
        script.onload = () => {
            callback();
        };

        script.src = AppConsts.appBaseUrl + '/assets/abp-web-resources/abp.signalr-client.js';
        document.head.appendChild(script);
    }
}
