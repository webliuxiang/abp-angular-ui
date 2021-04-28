/**
 * 布局的参数
 */
export interface ILayout {
    /** 主题 */
    theme: any;
    /** 左侧菜单栏是否关闭 */
    collapsed: boolean;
    /** 是否为小尺寸设备 */
    isPad: boolean;
    /** 多tab页 */
    reuseTab: boolean;
}

/**
 * 布局的 state
 */
export interface ILayoutState {
    value: ILayout
}
