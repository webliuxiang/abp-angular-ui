import {BehaviorSubject, Observable} from 'rxjs';
import {share} from 'rxjs/operators';
import {Route, VueRouter} from 'vue-router/types/router';

import {II18nService, IMenuService} from '@/shared';
import {IReuseItem} from './interfaces';


/**
 * 复用tab类定义
 */
class ReuseTabService {

    private inited = false;

    private change$ = new BehaviorSubject<IReuseItem[]>([]);

    private changePos$ = new BehaviorSubject<string>(undefined);

    private dataSource: IReuseItem[] = [];

    private i18nSer: II18nService;
    private menuSer: IMenuService;

    private rootRouting: VueRouter;

    private currentPath: string;

    private maxCount = 20;

    get change(): Observable<IReuseItem[]> {
        return this.change$.asObservable().pipe(share());
    }

    get changePos(): Observable<string> {
        return this.changePos$.asObservable().pipe(share());
    }

    get data(): IReuseItem[] {
        return this.dataSource;
    }

    /**
     * 最大复用数量,默认值为10
     * @param val
     */
    set max(val: number) {
        if (val < 1) {
            val = 1;
        }
        this.maxCount = val;
    }

    get max(): number {
        return this.maxCount;
    }

    /**
     * 初始化服务
     * @param rootRooting
     * @param i18nSer
     * @param menuSer
     */
    public init(rootRooting: VueRouter, i18nSer: II18nService, menuSer: IMenuService) {
        if (this.inited) {
            return;
        }

        this.inited = true;

        this.i18nSer = i18nSer;
        this.menuSer = menuSer;


        this.rootRouting = rootRooting;
        rootRooting.afterEach((to: Route, from: Route) => {
            this.reuseTabOnRoutingAfterEach(to, from);
        });
    }

    /**
     * 设置当前reuse-tab激活的tab页
     * @param val
     */
    public setPos(val: string) {
        this.changePos$.next(val);
    }

    /**
     * 跳转到此页面
     * @param item
     */
    public to(item: IReuseItem) {
        if (item.path === this.currentPath) {
            return;
        }

        setTimeout(() => {
            this.rootRouting.push({path: item.path});
        }, 1);

    }

    /**
     * 添加一项
     * @param item
     * @param currentPath 当前的路由地址
     */
    public add(item: IReuseItem, currentPath: string) {

        // 匹配下一个路由地址是否存在,不存在则重定向到上一个路由地址
        // @ts-ignore
        const nextPath = this.rootRouting.matcher.match({path: item.path});
        if (!nextPath || !nextPath.matched || nextPath.matched.length === 0) {
            setTimeout(() => {
                this.rootRouting.push({path: currentPath});
            }, 10);
            return;
        }

        const isAccount = item.path.startsWith('/account');
        if (isAccount === true) {
            return;
        }

        if (currentPath === item.path) {
            return;
        }

        const existIndex = this.dataSource.findIndex((o) => {
            return o.path === item.path;
        });

        if (existIndex === -1) {
            // 超出最大复用数量,自动从开始截取
            if ((this.dataSource.length + 1) > this.max) {
                this.dataSource = this.dataSource.splice(1, this.dataSource.length);
            }


            this.dataSource.push(item);

            // 处理是否允许关闭
            if (this.dataSource.length === 1) {
                this.dataSource[0].closable = false;
            } else if (this.dataSource.length > 1) {
                this.dataSource.forEach((o) => {
                    o.closable = true;
                });
            }

            this.change$.next(this.dataSource);
        }

        this.currentPath = item.path;
        this.changePos$.next(this.currentPath);
    }

    /**
     * 删除一项
     * @param item
     */
    public remove(item: IReuseItem) {

        if (!this.dataSource || this.dataSource.length === 1) {
            return;
        }

        // 删除项的索引
        const itemIndex = this.dataSource.findIndex((o) => o.path === item.path);


        let needJump = false;
        // 关闭的是当前的tab页
        if (this.currentPath === item.path) {
            // 下一个选中的tab
            const nextTab = this.dataSource[itemIndex + 1] || this.dataSource[itemIndex - 1];
            this.currentPath = nextTab.path;
            needJump = true;
        }

        // 过滤掉删除项
        this.dataSource = this.dataSource.filter((o) => o.path !== item.path);

        // 处理允许关闭tab
        if (this.dataSource.length === 1) {
            this.dataSource[0].closable = false;
        }


        // 跳转
        if (needJump) {
            this.rootRouting.push({path: this.currentPath});
        } else {
            this.changePos$.next(this.currentPath);
        }

        // 修改tab数量
        this.change$.next(this.dataSource);
    }


    /**
     * 路由跳转完成之后的拦截器
     * @param to
     * @param from
     */
    private reuseTabOnRoutingAfterEach(to: Route, from: Route) {
        if (!this.inited) {
            return;
        }

        const reuseItem: IReuseItem = {
            name: to.name || to.path,
            closable: true,
            path: to.path,
            displayTitle: undefined,
            title: undefined,
            i18n: null,
            reuse: false
        };

        // 首先从菜单中匹配
        const menuItem = this.menuSer.findByLink(reuseItem.path);
        if (menuItem) {
            reuseItem.title = menuItem.text;
            reuseItem.i18n = menuItem.i18n;
        }

        // 其次从路由meta信息中获取
        if (to.meta) {
            if (to.meta.title) {
                reuseItem.title = to.meta.title;
            }
            if (to.meta.i18n) {
                reuseItem.title = to.meta.i18n;
            }
        } else if (!reuseItem.title) {
            reuseItem.title = reuseItem.path;
        }


        // 生成显示的名称,如果有i18n,那么显示本地化处理之后的i18n,如果没有i18n,使用title
        reuseItem.displayTitle = reuseItem.i18n ? this.i18nSer.fanyi(reuseItem.i18n) : reuseItem.title;


        this.add(reuseItem, from.path);
    }
}


const reuseTabService = new ReuseTabService();
export default reuseTabService;
