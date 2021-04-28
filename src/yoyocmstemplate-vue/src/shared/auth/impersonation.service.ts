import {AppConsts} from '@/abpPro/AppConsts';
import {AccountServiceProxy, ImpersonateInput} from '@/shared';
import {appAuthService} from '@/shared/abp';
import {appUrlService} from '@/shared/nav';
import apiHttpClient from '@/shared/utils/api-http-client';

class ImpersonationService {

    private accountService: AccountServiceProxy;

    get _accountService(): AccountServiceProxy {
        if (!this.accountService) {
            this.accountService = new AccountServiceProxy(AppConsts.remoteServiceBaseUrl, apiHttpClient);
        }

        return this.accountService;
    }

    get _appUrlService() {
        return appUrlService;
    }

    get _authService() {
        return appAuthService;
    }


    public impersonate(userId: number, tenantId?: number): void {
        const input = new ImpersonateInput();
        input.userId = userId;
        input.tenantId = tenantId;

        this._accountService
            .impersonate(input)
            .then((result) => {
                this._authService.logout(false);

                let targetUrl =
                    this._appUrlService.getAppRootUrlOfTenant(result.tenancyName) +
                    '?impersonationToken=' +
                    result.impersonationToken;
                if (input.tenantId) {
                    targetUrl = targetUrl + '&tenantId=' + input.tenantId;
                }

                location.href = targetUrl;
            });
    }

    public backToImpersonator(): void {
        this._accountService
            .backToImpersonator()
            .then((result) => {
                this._authService.logout(false);

                let targetUrl =
                    this._appUrlService.getAppRootUrlOfTenant(result.tenancyName) +
                    '?impersonationToken=' +
                    result.impersonationToken;
                if (abp.session.impersonatorTenantId) {
                    targetUrl =
                        targetUrl + '&tenantId=' + abp.session.impersonatorTenantId;
                }

                location.href = targetUrl;
            });
    }

}

const impersonationService = new ImpersonationService();
export default impersonationService;
