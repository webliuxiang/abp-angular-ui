import {BehaviorSubject, Observable} from 'rxjs';
import {share} from 'rxjs/operators';
import {IAclService, II18nService} from '../interfaces';
import {IMenuService, Menu, MenuIcon} from './interfaces';


class MenuService implements IMenuService {

    private inited = false;

    private _change$: BehaviorSubject<Menu[]> = new BehaviorSubject<Menu[]>([]);

    /**
     * 树型的数据集
     */
    private data: Menu[] = [];

    /**
     * 平铺的数据集
     */
    private dataList: Menu[] = [];

    private i18nSrv: II18nService;
    private aclService: IAclService;


    get change(): Observable<Menu[]> {
        return this._change$.pipe(share());
    }

    /**
     * 初始化服务
     * @param i18nSrv 本地化服务
     * @param aclService 权限校验服务
     */
    public initService(i18nSrv: II18nService, aclService: IAclService) {
        if (this.inited) {
            return;
        }

        this.i18nSrv = i18nSrv;
        this.aclService = aclService;
        if (!this.i18nSrv || !this.aclService) {
            return;
        }

        this.inited = true;

        // 本地化发生改变,刷新菜单
        this.i18nSrv.change.subscribe(() => {
            this.resume();
        });
    }

    /**
     * 根据link查找
     * @param link
     * @param exact 强匹配
     */
    public findByLink(link: string, exact?: boolean): Menu {
        if (!link) {
            return null;
        }
        if (exact) {
            return this.dataList.find((o) => o.link !== '' && o.link !== null && o.link === link);
        }
        return this.dataList.find((o) => o.link !== '' && o.link !== null && link.startsWith(o.link));
    }

    /**
     * 添加菜单
     * @param items
     */
    public add(items: Menu[]) {
        this.data = items;
        this.resume();
    }


    /**
     * 重置菜单，可能I18N、用户权限变动时需要调用刷新
     */
    public resume(callback?: (item: Menu, parentMenum: Menu, depth?: number) => void) {
        let i = 1;
        const shortcuts: Menu[] = [];
        this.visit(this.data, (item, parent, depth) => {
            item.__id = i++;
            item.__parent = parent;
            item._depth = depth;
            // 生成唯一键值
            item.key = item.__id;

            // 平铺的数据
            this.dataList.push(item);

            if (!item.link) item.link = '';
            if (!item.externalLink) item.externalLink = '';

            // badge
            if (item.badge) {
                if (item.badgeDot !== true) {
                    item.badgeDot = false;
                }
                if (!item.badgeStatus) {
                    item.badgeStatus = 'error';
                }
            }

            item._type = item.externalLink ? 2 : 1;
            if (item.children && item.children.length > 0) {
                item._type = 3;
            }

            // icon
            if (typeof item.icon === 'string') {
                let type = 'class';
                let value = item.icon;
                // compatible `anticon anticon-user`
                if (~item.icon.indexOf('anticon-')) {
                    type = 'icon';
                    value = value
                        .split('-')
                        .slice(1)
                        .join('-');
                } else if (/^https?:\/\//.test(item.icon)) {
                    type = 'img';
                }
                item.icon = {type, value} as any;
            }
            if (item.icon != null) {
                item.icon = {theme: 'outline', spin: false, ...(item.icon as MenuIcon)};
            }

            item.text = item.i18n && this.i18nSrv ? this.i18nSrv.fanyi(item.i18n) : item.text;

            // group
            item.group = item.group !== false;

            // hidden
            item._hidden = typeof item.hide === 'undefined' ? false : item.hide;

            // disabled
            item.disabled = typeof item.disabled === 'undefined' ? false : item.disabled;

            // acl
            item._aclResult = item.acl && this.aclService ? this.aclService.can(item.acl) : true;

            // shortcut
            if (parent && item.shortcut === true && parent.shortcutRoot !== true) {
                shortcuts.push(item);
            }

            if (callback) callback(item, parent, depth);
        });

        // this.loadShortcut(shortcuts);
        this._change$.next(this.data);
    }


    /**
     * 清空菜单
     */
    public clear() {
        this.data = [];
        this._change$.next(this.data);
    }

    public getHit(data: Menu[], url: string, recursive = false, cb: (i: Menu) => void = null) {
        let item: Menu = null;

        while (!item && url) {
            this.visit(data, (i) => {
                if (cb) {
                    cb(i);
                }
                if (i.link != null && i.link === url) {
                    item = i;
                }
            });

            if (!recursive) break;

            url = url
                .split('/')
                .slice(0, -1)
                .join('/');
        }

        return item;
    }

    public visit(data: Menu[], callback: (item: Menu, parentMenum: Menu, depth?: number) => void) {
        const inFn = (list: Menu[], parentMenu: Menu, depth: number) => {
            for (const item of list) {
                callback(item, parentMenu, depth);
                if (item.children && item.children.length > 0) {
                    inFn(item.children, item, depth + 1);
                } else {
                    item.children = [];
                }
            }
        };

        inFn(data, null, 0);
    }


    /**
     * 根据URL设置菜单 `_open` 属性
     * - 若 `recursive: true` 则会自动向上递归查找
     *  - 菜单数据源包含 `/ware`，则 `/ware/1` 也视为 `/ware` 项
     */
    public openedByUrl(url: string, recursive = false) {
        if (!url) return;

        let findItem = this.getHit(this.data, url, recursive, (i) => {
            i._selected = false;
            i._open = false;
        });
        if (!findItem) return;

        do {
            findItem._selected = true;
            findItem._open = true;
            findItem = findItem.__parent;
        } while (findItem);
    }

    /**
     * 根据url获取菜单列表
     * - 若 `recursive: true` 则会自动向上递归查找
     *  - 菜单数据源包含 `/ware`，则 `/ware/1` 也视为 `/ware` 项
     */
    public getPathByUrl(url: string, recursive = false): Menu[] {
        const ret: Menu[] = [];
        let item = this.getHit(this.data, url, recursive);

        if (!item) return ret;

        do {
            ret.splice(0, 0, item);
            item = item.__parent;
        } while (item);

        return ret;
    }

}

const menuService = new MenuService();
export default menuService;
