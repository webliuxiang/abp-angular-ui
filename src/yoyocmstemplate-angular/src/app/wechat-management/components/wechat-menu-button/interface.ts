export interface IWechatMenuInfo {
    /**
     * 父级节点id
     */
    parentId: string;
    /**
     * 父级节点
     */
    parent: IWechatMenuInfo;
    /**
     * 菜单id
     */
    id: string;
    /**
     * 文本
     */
    text: string;
    /**
     * 子菜单集合
     */
    subMenu: IWechatMenuInfo[];
    /**
     * 子菜单是否水平方向
     */
    subMenuHorizontal: boolean;
    /**
     * 菜单所在组件
     */
    component?: any;

    /**
     * 源数据
     */
    origin?: any;
}
