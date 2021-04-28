import {AppConsts} from '@/abpPro/AppConsts';
import {UrlHelper} from '@/shared/helpers/UrlHelper';
import {
    AuthenticateModel,
    AuthenticateResultModel,
    ExternalAuthenticateModel,
    ExternalAuthenticateResultModel,
    ExternalLoginProviderInfoModel,
    TokenAuthServiceProxy
} from '@/shared/service-proxies';

import * as _ from 'lodash';

import {environment} from '@/environments';
import {rootRouting} from '@/router';
import {abpService} from '@/shared/abp/';
import {apiHttpClient} from '@/shared/utils';
import {scriptLoaderService} from '@/shared/utils/script-loader.service';
import {UserAgentApplication} from 'msal';


declare global {
    export const QC: {
        Login: {
            showPopup: (oOpts: { appId: string; redirectURI: string }) => void;
            check(): boolean;
            getMe(callback: (openId: string, accessToken: string) => void);
        } & ((
            options: {
                btnId: string;
                showModal: boolean;
                scope: string;
                size: 'A_XL' | 'A_L' | 'A_M' | 'A_S' | 'B_M' | 'B_S' | 'C_S';
            },
            loginFun: Function,
            logoutFun: Function,
            outCallBackFun: Function
        ) => void);
    };

    export class WxLogin {
        constructor(param: {
            self_redirect?: boolean;
            id: string;
            appid: string;
            scope: string;
            redirect_uri: string;
            state?: string;
            style?: string;
            href?: string;
        });
    }
}

export class ExternalLoginProvider extends ExternalLoginProviderInfoModel {
    // static readonly FACEBOOK: string = 'Facebook';
    // static readonly GOOGLE: string = 'Google';
    // static readonly MICROSOFT: string = 'Microsoft';
    // static readonly OPENID: string = 'OpenIdConnect';
    // static readonly WSFEDERATION: string = 'WsFederation';
    public static readonly QQ: string = 'QQ';
    public static readonly Wechat: string = 'Wechat';
    public static readonly Microsoft: string = 'Microsoft';
    public static readonly Facebook: string = 'Facebook';
    public static readonly Google: string = 'Google';

    public icon: string;
    public initialized = false;

    constructor(providerInfo: ExternalLoginProviderInfoModel) {
        super();

        this.name = providerInfo.name;
        this.clientId = providerInfo.clientId;
        this.additionalParams = providerInfo.additionalParams;
        this.icon = ExternalLoginProvider.getSocialIcon(this.name);
    }

    private static getSocialIcon(providerName: string): string {
        providerName = providerName.toLowerCase();

        if (providerName === 'google') {
            providerName = 'google-plus';
        }

        if (providerName === 'microsoft') {
            providerName = 'windows';
        }

        return providerName;
    }
}


class LoginService {
    public static readonly twoFactorRememberClientTokenName =
        'TwoFactorRememberClientToken';

    public authenticateModel: AuthenticateModel = new AuthenticateModel();
    public authenticateResult: AuthenticateResultModel;
    public externalLoginProviders: ExternalLoginProvider[] = [];
    public rememberMe: boolean;

    public _tokenAuthService: TokenAuthServiceProxy;


    constructor(
        // private _tokenAuthService: TokenAuthServiceProxy,
        // private _utilsService: UtilsService,
        // private _messageService: MessageService,
        // private _tokenService: TokenService,
        // private _logService: LogService,
    ) {
        this._tokenAuthService = new TokenAuthServiceProxy(AppConsts.remoteServiceBaseUrl, apiHttpClient);
    }

    public authenticate(
        successCallback?: () => void,
        errorCallback?: () => void,
        finallyCallback?: () => void,
        redirectUrl?: string
    ): void {
        successCallback = successCallback || (() => {
        });
        errorCallback = errorCallback || (() => {
        });
        finallyCallback = finallyCallback || (() => {
        });


        this._tokenAuthService
            .authenticate(this.authenticateModel)
            .finally(() => {
                finallyCallback();
            })
            .catch((err) => {
                errorCallback();
                return err;
            })
            .then((result) => {
                successCallback();
                this.processAuthenticateResult(result, redirectUrl);
            });
    }

    public async externalAuthenticate(provider: ExternalLoginProvider): Promise<void> {
        await this.ensureExternalLoginProviderInitialized(provider, async () => {
            const queryParams = UrlHelper.getQueryParametersUsingHash();
            switch (provider.name) {
                case ExternalLoginProvider.QQ:
                    if (QC && QC.Login.check()) {
                        this.qqLoginCallback();
                    } else {
                        this.redirectToQQLogin();
                    }
                    break;
                case ExternalLoginProvider.Wechat:
                    if (queryParams.code) {
                        this.wechatLoginCallback();
                    } else {
                        this.redirectToWechatLogin();
                    }
                    break;
                case ExternalLoginProvider.Microsoft:
                    this.redirectToMicrosoftLogin();
                    // if (queryParams.code) {
                    //   this.authService.signIn(provider.clientId)
                    // } else {
                    //   this.redirectToMicrosoftLogin();
                    // }
                    break;
                // case ExternalLoginProvider.Facebook:
                //     await this.authService.signIn(FacebookLoginProvider.PROVIDER_ID)
                //         .then(user => this.standardLoginCallback(user, ExternalLoginProvider.Facebook))
                //         .catch(ex => console.log(ex))
                //     break;
                // case ExternalLoginProvider.Google:
                //     await this.authService.signIn(GoogleLoginProvider.PROVIDER_ID)
                //         .then(user => this.standardLoginCallback(user, ExternalLoginProvider.Google))
                //         .catch(ex => console.log(ex))
                //     break;
                default:
                    break;
            }
        });
    }

    // standardLoginCallback(user: SocialUser, provider: string): any {
    //     const model = new ExternalAuthenticateModel();
    //     model.authProvider = provider;
    //     model.providerAccessCode = user.authToken;
    //     model.providerKey = user.id;
    //     // model.singleSignIn = UrlHelper.getSingleSignIn();
    //
    //     this._tokenAuthService
    //         .externalAuthenticate(model)
    //         .then((result: ExternalAuthenticateResultModel) => {
    //             if (result.waitingForActivation) {
    //                 this._router.navigate(['account/activation'], {
    //                     state: result
    //                 });
    //                 return;
    //             }
    //
    //             this.login(
    //                 result.accessToken,
    //                 result.encryptedAccessToken,
    //                 result.expireInSeconds
    //             );
    //         });
    // }
    public async redirectToMicrosoftLogin() {
        const redirectUri = new URL(environment.externalLogin.redirectUri);
        redirectUri.searchParams.append('providerName', ExternalLoginProvider.Microsoft);
        const result = await new UserAgentApplication({
            auth: {
                clientId: environment.externalLogin.microsoft.consumerKey, redirectUri: redirectUri.toString(),
            },
        });
        result.acquireTokenPopup({
            scopes: ['https://graph.microsoft.com/User.Read'],

        }).then((user) => {
            const model = new ExternalAuthenticateModel();
            model.authProvider = ExternalLoginProvider.Microsoft;
            model.providerAccessCode = user.accessToken;
            model.providerKey = user.uniqueId;
            // model.singleSignIn = UrlHelper.getSingleSignIn();

            this._tokenAuthService
                .externalAuthenticate(model)
                .then((result: ExternalAuthenticateResultModel) => {
                    if (result.waitingForActivation) {
                        rootRouting.push({
                            path: 'account/activation',
                            params: {
                                state: result.toJSON()
                            }
                        });
                        // this._router.navigate(['account/activation'], {
                        //     state: result
                        // });
                        return;
                    }

                    this.login(
                        result.accessToken,
                        result.encryptedAccessToken,
                        result.expireInSeconds
                    );
                });
        });
    }

    public redirectToWechatLogin(): any {
        const curUrl = new URL(location.href);
        //   this._modalService.create({
        //       nzContent: html`
        //   <div id="wx_login_container"></div>
        // `.getHTML()
        //   });
        curUrl.searchParams.append('providerName', ExternalLoginProvider.Wechat);
        const wxConfig = environment.externalLogin.wecaht;
        const obj = new WxLogin({
            // self_redirect: true,
            id: 'wx_login_container',
            appid: wxConfig.appId,
            scope: 'snsapi_login',
            redirect_uri: encodeURIComponent(curUrl.toString()),
            // state: '',
            style: 'white'
            // href: ''
        });
    }

    private wechatLoginCallback() {
        const code = UrlHelper.getQueryParametersUsingHash().code;
        const model = new ExternalAuthenticateModel();
        model.authProvider = ExternalLoginProvider.Wechat;
        model.providerAccessCode = code;
        model.providerKey = undefined;
        // model.singleSignIn = UrlHelper.getSingleSignIn();

        this._tokenAuthService
            .externalAuthenticate(model)
            .then((result: ExternalAuthenticateResultModel) => {
                if (result.waitingForActivation) {
                    rootRouting.push({
                        path: 'account/activation',
                        params: {
                            state: result.toJSON()
                        }
                    });
                    // this._router.navigate(['account/activation'], {
                    //     state: result
                    // });
                    return;
                }

                this.login(
                    result.accessToken,
                    result.encryptedAccessToken,
                    result.expireInSeconds
                );
            });
    }

    private redirectToQQLogin() {
        const redirectURI = new URL(environment.externalLogin.redirectUri);
        redirectURI.searchParams.append('providerName', ExternalLoginProvider.QQ);
        QC.Login.showPopup({
            ...environment.externalLogin.qq,
            redirectURI: redirectURI.toString()
        });
    }

    private qqLoginCallback(): any {
        QC.Login.getMe((openId, accessToken) => {
            // angular 的 bug, 外部js中的异步回调回跑出zone, 需要手动同步, 参考issue: https://github.com/angular/angular/issues/20290
            // this.zone.run(async () => {
            //     const model = new ExternalAuthenticateModel();
            //     model.authProvider = ExternalLoginProvider.QQ;
            //     model.providerAccessCode = JSON.stringify({accessToken, openId});
            //     model.providerKey = openId;
            //     // model.singleSignIn = UrlHelper.getSingleSignIn();
            //
            //     const authResult = await this._tokenAuthService
            //         .externalAuthenticate(model)
            //         .toPromise();
            //     if (authResult.waitingForActivation) {
            //         this._router.navigate(['account/activation'], {
            //             state: authResult
            //         });
            //     } else {
            //         this.login(
            //             authResult.accessToken,
            //             authResult.encryptedAccessToken,
            //             authResult.expireInSeconds
            //         );
            //     // }
            // });
        });
    }

    public async initExternalLoginProviders(callback?: Function) {
        const providers = await this._tokenAuthService
            .getExternalAuthenticationProviders();
        this.externalLoginProviders = _.map(
            providers,
            (p) => new ExternalLoginProvider(p)
        );

        if (callback) {
            callback();
        }
    }

    public async ensureExternalLoginProviderInitialized(
        loginProvider: ExternalLoginProvider,
        callback: () => void
    ) {
        switch (loginProvider.name) {
            case ExternalLoginProvider.QQ:
                await scriptLoaderService
                    .load('//connect.qq.com/qc_jssdk.js')
                    .then(() => {
                        loginProvider.initialized = true;
                    });
                break;
            case ExternalLoginProvider.Wechat:
                await scriptLoaderService
                    .load('//res.wx.qq.com/connect/zh_CN/htmledition/js/wxLogin.js')
                    .then(() => {
                        loginProvider.initialized = true;
                    });
                break;
            case ExternalLoginProvider.Microsoft:
                loginProvider.initialized = true;
                break;
            case ExternalLoginProvider.Facebook:
                loginProvider.initialized = true;
                break;
            case ExternalLoginProvider.Google:
                loginProvider.initialized = true;
                break;
            default:
                break;
        }
        if (loginProvider && loginProvider.initialized) {
            callback();
            return;
        }
    }

    private processAuthenticateResult(
        authenticateResult: AuthenticateResultModel,
        redirectUrl?: string
    ) {
        if (authenticateResult.shouldResetPassword) {
            // 需要重置密码
            rootRouting.push({
                path: 'account/reset-password',
                params: {
                    userId: authenticateResult.userId.toString(),
                    tenantId: abp.session.tenantId.toString(),
                    resetCode: authenticateResult.passwordResetCode
                }
            });

            // this._router.navigate(['account/reset-password'], {
            //     queryParams: {
            //         userId: authenticateResult.userId,
            //         tenantId: abp.session.tenantId,
            //         resetCode: authenticateResult.passwordResetCode
            //     }
            // });
        }
        if (authenticateResult.waitingForActivation) {
            rootRouting.push({
                path: 'account/activation',
                params: {
                    state: authenticateResult.toJSON()
                }
            });
            // this._router.navigate(['account/activation'], {
            //     state: authenticateResult
            // });
        } else if (authenticateResult.accessToken) {
            // Successfully logged in
            // tslint:disable-next-line:max-line-length
            this.login(
                authenticateResult.accessToken,
                authenticateResult.encryptedAccessToken,
                authenticateResult.expireInSeconds,
                this.rememberMe
            );
        } else {
            // Unexpected result!

            abpService.abp.log.warn('Unexpected authenticateResult!');
            rootRouting.push({path: 'account/login'});
            // this._router.navigate(['account/login']);
        }
    }

    private login(
        accessToken: string,
        encryptedAccessToken: string,
        expireInSeconds: number,
        rememberMe?: boolean,
        twoFactorRememberClientToken?: string,
        redirectUrl?: string
    ): void {
        const tokenExpireDate = rememberMe
            ? new Date(new Date().getTime() + 1000 * expireInSeconds)
            : undefined;

        abpService.abp.auth.setToken(accessToken, tokenExpireDate);

        abpService.abp.utils.setCookieValue(
            AppConsts.authorization.encrptedAuthTokenName,
            encryptedAccessToken,
            tokenExpireDate,
            abp.appPath
        );

        let initialUrl = UrlHelper.initialUrl;
        if (initialUrl.indexOf('/login') > 0) {
            initialUrl = AppConsts.appBaseUrl;
        }

        location.href = initialUrl;
    }


    public getReturnUrl(): string{
       return UrlHelper.getReturnUrl();
    }
}

const loginService = new LoginService();

export {
    loginService
};
