import {AppConsts} from '@/abpPro/AppConsts';
import {abpService} from '@/shared/abp';
import {
    ApplicationInfoDto, GetCurrentLoginInformationsOutput,
    SessionServiceProxy,
    TenantLoginInfoDto,
    UserLoginInfoDto
} from '@/shared/service-proxies';
import {apiHttpClient} from '@/shared/utils';

class AppSessionService {

    private _sessionService: SessionServiceProxy;


    private get loginInfo(): GetCurrentLoginInformationsOutput {
        return abpService.loginInfo;
    }

    get application(): ApplicationInfoDto {
        return this.loginInfo.application;
    }

    get user(): UserLoginInfoDto {
        return this.loginInfo.user;
    }

    get userId(): number {
        return this.user ? this.user.id : null;
    }

    get tenant(): TenantLoginInfoDto {
        return this.loginInfo.tenant;
    }

    get tenantId(): number {
        return this.tenant ? this.tenant.id : null;
    }

    public getShownLoginName(): string {
        const userName = this.loginInfo.user.userName;

        if (!abpService.abp.multiTenancy.isEnabled) {
            return userName;
        }

        return (this.loginInfo.tenant ? this.loginInfo.tenant.tenancyName + '\\' : '') + userName;
    }

    public init(): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {

            this.session
                .getCurrentLoginInformations()
                .then(
                    (result) => {

                        abpService.setLoginInfo(result);

                        resolve(true);
                    },
                    (err) => {
                        reject(err);
                    }
                );
        });
    }

    public changeTenantIfNeeded(tenantId?: number): boolean {
        if (this.isCurrentTenant(tenantId)) {
            return false;
        }

        abpService.abp.multiTenancy.setTenantIdCookie(tenantId);
        location.reload();
        return true;
    }

    get session(): SessionServiceProxy {
        if (!this._sessionService) {
            this._sessionService = new SessionServiceProxy(AppConsts.remoteServiceBaseUrl, apiHttpClient);
        }
        return this._sessionService;
    }

    private isCurrentTenant(tenantId?: number) {
        if (!tenantId && this.tenant) {
            return false;
        } else if (tenantId && (!this.tenant || this.tenant.id !== tenantId)) {
            return false;
        }

        return true;
    }
}

const appSessionService = new AppSessionService();

export default appSessionService;
