import {AppConsts} from '@/abpPro/AppConsts';
import {abpService} from '@/shared/abp';


class AppAuthService {
    public logout(reload?: boolean): void {
        abpService.abp.auth.clearToken();
        if (reload !== false) {
            location.href = AppConsts.appBaseUrl;
        }
    }
}

const appAuthService = new AppAuthService();
export default appAuthService;
