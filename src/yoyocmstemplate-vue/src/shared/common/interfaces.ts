import {Observable} from 'rxjs';


/**
 * 本地化服务接口定义
 */
export interface II18nService {
    /**
     * 语言发生改变
     */
    change: Observable<string>;

    /**
     * 翻译
     * @param key
     */
    fanyi(key: string): string;
}

/**
 * 权限校验服务接口定义
 */
export interface IAclService {
    /**
     * 是否可访问
     * @param acl
     */
    can(acl: string): boolean;
}

