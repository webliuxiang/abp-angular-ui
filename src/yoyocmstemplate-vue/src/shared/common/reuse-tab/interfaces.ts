export interface IReuseTabStore {
    source: any[];
    to?: string;
}

export interface IReuseItem {
    /**
     * 路由名称
     */
    name: string;
    /**
     * 是否可关闭
     */
    closable: boolean;
    /**
     * url地址
     */
    path: string;
    /**
     * 显示的title
     */
    displayTitle: string;
    /**
     * 标题
     */
    title: string;
    /**
     * 本地化标题
     */
    i18n?: string;
    /**
     * 是否可复用（暂时不生效）
     */
    reuse?: boolean;
}
