import { AxiosRequestConfig } from 'axios';
import * as _ from 'lodash';
import moment from 'moment';

import { AppConsts } from '@/abpPro/AppConsts';
import { environment } from '@/environments/environment';
import { MessageExtension } from '@/shared';
import { abpService } from '@/shared/abp';
import { UrlHelper } from '@/shared/helpers/UrlHelper';
import { apiHttpClient, httpClient } from '@/shared/utils';


export class AppPreBootstrap {

    /**
     * 启动
     * @param callback 回调函数
     */
    public static run(callback: () => void) {

        // 覆盖默认的租户id名称
        abp.multiTenancy.tenantIdCookieName = AppConsts.tenantIdCookieName;

        // 覆盖默认的租户id名称
        abp.multiTenancy.tenantIdCookieName = AppConsts.tenantIdCookieName;

        // 获取客户端基础配置
        AppPreBootstrap.getApplicationConfig(() => {

            const queryStringObj = UrlHelper.getQueryParameters();
            if (queryStringObj.impersonationToken) {
                // 模拟登陆
                abp.multiTenancy.setTenantIdCookie(queryStringObj.tenantId);
                AppPreBootstrap.impersonatedAuthenticate(
                    queryStringObj.impersonationToken,
                    () => {
                        AppPreBootstrap.getUserConfiguration(
                            callback
                        );
                    }
                );
            } else if (queryStringObj.switchAccountToken) {
                // 切换关联账号
                abp.multiTenancy.setTenantIdCookie(queryStringObj.tenantId);
                AppPreBootstrap.linkedAccountAuthenticate(
                    queryStringObj.switchAccountToken,
                    () => {
                        AppPreBootstrap.getUserConfiguration(callback);
                    }
                );
            } else {
                // 普通登陆直接获取信息
                AppPreBootstrap.getUserConfiguration(callback);
            }
        });

    }

    /**
     * 初始化前端基本配置
     * @param callback
     */
    public static getApplicationConfig(callback: () => void) {

        let envName = '';
        if (environment.production) {
            envName = 'prod';
        } else {
            envName = 'dev';
        }

        const url = '/assets/appconfig.' + envName + '.json';

        httpClient.get(url).then((response: any) => {

            const result = response.data;

            AppConsts.appBaseUrl =
                window.location.protocol + '//' + window.location.host;
            AppConsts.remoteServiceBaseUrl = result.remoteServiceBaseUrl;
            AppConsts.portalBaseUrl = result.portalBaseUrl;
            AppConsts.localeMappings = result.localeMappings;
            AppConsts.ngZorroLocaleMappings = result.ngZorroLocaleMappings;
            AppConsts.ngAlainLocaleMappings = result.ngAlainLocaleMappings;
            AppConsts.momentLocaleMappings = result.momentLocaleMappings;


            callback();
        }).catch((err) => {
            alert(`初始化配置信息出错,错误信息:\n\n${err.message}`);
        });

    }

    /**
     * 获取后端配置
     * @param callback 回调函数
     */
    public static getUserConfiguration(
        callback: () => void
    ) {
        const config: AxiosRequestConfig = {
            headers: {
                common: {
                    'Authorization': 'Bearer ' + abp.auth.getToken() || '',
                    '.AspNetCore.Culture': abp.utils.getCookieValue('Abp.Localization.CultureName'),
                    'Abp.TenantId': abp.multiTenancy.getTenantIdCookie() || ''
                }
            }
        };
        httpClient
            .get(
                `${AppConsts.remoteServiceBaseUrl}/api/services/app/Session/GetUserConfigurations`,
                config
            )
            .then(
                (response: any) => {

                    const result = response.data.result;

                    MessageExtension.overrideAbpMessageByModal();
                    MessageExtension.overrideAbpNotify();

                    // 填充数据
                    _.merge(abp, result);

                    // 时区
                    abp.clock.provider = AppPreBootstrap.getCurrentClockProvider(
                        result.clock.provider
                    );

                    const locale = AppPreBootstrap.convertAbpLocaleToMomentLocale(
                        abp.localization.currentLanguage.name
                    );

                    moment.locale(locale);

                    if (abp.clock.provider.supportsMultipleTimezone) {
                        moment.tz.setDefault(abp.timing.timeZoneInfo.iana.timeZoneId);
                        (window as any).moment.tz.setDefault(
                            abp.timing.timeZoneInfo.iana.timeZoneId
                        );
                    }

                    // // 权限
                    // const permissionService = injector.get(PermissionService);
                    // permissionService.extend(abp.auth);
                    //
                    // // 本地化
                    // const localization = injector.get<LocalizationService>(
                    //   ALAIN_I18N_TOKEN
                    // );
                    // localization.extend(abp.localization);
                    //
                    // // 写入菜单
                    // const menuService = injector.get(MenuService);
                    // menuService.add(AppMenus.Menus);

                    abpService.set(abp);

                    callback();
                },
                (error) => {
                    alert(`初始化用户信息出错,错误信息:\n\n${error.message}`);
                }
            );
    }

    /**
     * 模拟登陆用户
     * @param impersonationToken
     * @param callback
     */
    public static impersonatedAuthenticate(
        impersonationToken: string,
        callback: () => void
    ) {
        apiHttpClient
            .post(
                `${AppConsts.remoteServiceBaseUrl}/api/TokenAuth/ImpersonatedAuthenticate?impersonationToken=${impersonationToken}`,
                null
            )
            .then(
                (response: any) => {
                    const result = response.data;
                    abp.auth.setToken(result.accessToken);
                    AppPreBootstrap.setEncryptedTokenCookie(result.encryptedAccessToken);
                    location.search = '';
                    callback();
                }
            ).catch((err) => {
                alert(`模拟登陆出错,错误信息:\n\n${err.message}`);
            });
    }

    /**
     * 切换关联用户
     * @param switchAccountToken
     * @param callback
     */
    public static linkedAccountAuthenticate(
        switchAccountToken: string,
        callback: () => void
    ): void {
        apiHttpClient
            .post(
                `${AppConsts.remoteServiceBaseUrl}/api/TokenAuth/LinkedAccountAuthenticate?switchAccountToken=${switchAccountToken}`,
                null
            )
            .then((response: any) => {
                const result = response.data;
                abp.auth.setToken(result.accessToken);
                AppPreBootstrap.setEncryptedTokenCookie(result.encryptedAccessToken);
                location.search = '';
                callback();
            }
            ).catch((err) => {
                alert(`切换关联用户出错,错误信息:\n\n${err.message}`);
            });
    }


    /**
     * 时区修改
     * @param currentProviderName
     */
    private static getCurrentClockProvider(
        currentProviderName: string
    ): abp.timing.IClockProvider {
        if (currentProviderName === 'unspecifiedClockProvider') {
            return abp.timing.unspecifiedClockProvider;
        }

        if (currentProviderName === 'utcClockProvider') {
            return abp.timing.utcClockProvider;
        }

        return abp.timing.localClockProvider;
    }


    private static setEncryptedTokenCookie(encryptedToken: string) {
        abp.utils.setCookieValue(
            AppConsts.authorization.encrptedAuthTokenName,
            encryptedToken,
            new Date(new Date().getTime() + 365 * 86400000), // 1 year
            abp.appPath
        );
    }

    /**
     * 将ABP多语言转换为moment多语言
     * @param locale
     */
    private static convertAbpLocaleToMomentLocale(locale: string): string {
        const defaultLocale = 'zh-CN';
        if (!AppConsts.momentLocaleMappings) {
            return defaultLocale;
        }

        const localeMapings = _.filter(AppConsts.momentLocaleMappings, {
            from: locale
        });
        if (localeMapings && localeMapings.length) {
            return localeMapings[0].to;
        }

        return defaultLocale;
    }

}
