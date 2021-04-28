import {AppConsts} from '@/abpPro/AppConsts';
import {appSessionService} from '@/shared/abp';

class AppUrlService {

    public static tenancyNamePlaceHolder = '{TENANCY_NAME}';



    get appRootUrl(): string {
        if (appSessionService.tenant) {
            return this.getAppRootUrlOfTenant(
                appSessionService.tenant.tenancyName
            );
        } else {
            return this.getAppRootUrlOfTenant(null);
        }
    }

    /**
     * Returning url ends with '/'.
     */
    public getAppRootUrlOfTenant(tenancyName?: string): string {
        let baseUrl = this.ensureEndsWith(AppConsts.appBaseUrl, '/');

        if (baseUrl.indexOf(AppUrlService.tenancyNamePlaceHolder) < 0) {
            return baseUrl;
        }

        if (baseUrl.indexOf(AppUrlService.tenancyNamePlaceHolder + '.') >= 0) {
            baseUrl = baseUrl.replace(
                AppUrlService.tenancyNamePlaceHolder + '.',
                AppUrlService.tenancyNamePlaceHolder
            );
            if (tenancyName) {
                tenancyName = tenancyName + '.';
            }
        }

        if (!tenancyName) {
            return baseUrl.replace(AppUrlService.tenancyNamePlaceHolder, '');
        }

        return baseUrl.replace(AppUrlService.tenancyNamePlaceHolder, tenancyName);
    }

    private ensureEndsWith(str: string, c: string) {
        if (str.charAt(str.length - 1) !== c) {
            str = str + c;
        }

        return str;
    }

    private removeFromEnd(str: string, c: string) {
        if (str.charAt(str.length - 1) === c) {
            str = str.substr(0, str.length - 1);
        }

        return str;
    }
}

const appUrlService = new AppUrlService();

export default appUrlService;
