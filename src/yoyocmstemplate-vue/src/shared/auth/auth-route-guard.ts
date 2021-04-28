import { abpService, appSessionService } from '@/shared/abp';
import NProgress from 'nprogress'; // progress bar
import { Vue } from 'vue-property-decorator';
import { RawLocation, Route, VueRouter } from 'vue-router/types/router';


// 默认的登录页和主页
const defaultLoginUrl = '/account/login';
const defaultMainUrl = '/app';

/**
 * 路由守卫
 */
class AppRouteGuard {

    private _inited = false;

    private _loginUrl = defaultLoginUrl;
    private _mainUrl = defaultMainUrl;
    private _whiteList = [
        defaultLoginUrl,
    ];

    /**
     * 不需要校验权限的路由
     */
    get whiteList(): string[] {
        return this._whiteList;
    }

    set whiteList(val: string[]) {
        if (Array.isArray(val)) {
            this._whiteList = val;
        } else {
            this._whiteList = [];
        }
    }

    /**
     * 登陆页面地址
     */
    get loginUrl(): string {
        return this._loginUrl;
    }

    set loginUrl(val: string) {
        this._loginUrl = val ? val : defaultLoginUrl;
    }

    /**
     * 首页地址
     */
    get mainUrl(): string {
        return this._mainUrl;
    }

    set mainUrl(val: string) {
        this._mainUrl = val ? val : defaultMainUrl;
    }


    /**
     * 初始化路由守卫
     */
    public init(rootRooting: VueRouter) {
        if (this._inited) {
            return;
        }

        this._inited = true;
        NProgress.configure({ showSpinner: false });

        /** 绑定路由守卫 */
        rootRooting.beforeResolve((to: Route, from: Route, next: any) => {
            this.beforeResolve(to, from, next);
        });
        rootRooting.beforeEach((to: Route, from: Route, next: any) => {
            this.beforeEach(to, from, next);
        });
        rootRooting.afterEach((to: Route, from: Route) => {
            this.afterEach(to, from);
        });
    }

    /**
     * 路由进入跳转之前
     * @param to
     * @param from
     * @param next
     */
    public beforeResolve(to: Route, from: Route, next: any) {
        if (to.path === '/') {
            next({ path: appRouteGuard.mainUrl });
            return;
        }

        NProgress.start();

        if (appRouteGuard.whiteList.findIndex((o) => o.startsWith(to.path))) {

            if (appRouteGuard.canActivate(to, from, next)) {

                const redirect = decodeURIComponent((from.query.redirect as string) || to.path);
                if (to.path !== redirect) {
                    // 跳转到重定向的路由
                    next({ path: redirect });
                    return;
                }
            }
        }


        next();
    }

    /**
     * 路由跳转之前
     * @param to
     * @param from
     * @param next
     */
    public beforeEach(to: Route, from: Route, next: any) {
        next();
    }

    /**
     * 路由跳转后
     * @param to
     * @param from
     */
    public afterEach(to: Route, from: Route) {
        NProgress.done();
    }


    /**
     * 判断是否可访问这个地址
     * @param to 目标地址
     * @param from 现地址
     * @param next
     */
    private canActivate(to: Route, from: Route, next: (to?: RawLocation | false | ((vm: Vue) => any) | void) => void): boolean {

        if (!appSessionService.user) {
            next({ path: this.loginUrl, query: { redirect: to.fullPath } });
            return false;
        }

        if (!to.meta || !to.meta.permission) {
            return true;
        }

        if (abpService.abp.auth.isGranted(to.meta.permission)) {
            return true;
        }




        next({ path: this.selectBestRoute(), query: { redirect: to.fullPath } });

        return false;

    }


    /**
     * 选择最佳的路由地址
     */
    private selectBestRoute(): string {
        if (!appSessionService.user) {
            return this.loginUrl;
        }


        return this.mainUrl;
    }
}

const appRouteGuard = new AppRouteGuard();
export default appRouteGuard;
