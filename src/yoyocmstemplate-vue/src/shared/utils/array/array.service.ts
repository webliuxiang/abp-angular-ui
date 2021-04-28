import {
    ArrayConfig,
    ArrayServiceArrToTreeNodeOptions,
    ArrayServiceArrToTreeOptions,
    ArrayServiceGetKeysByTreeNodeOptions,
    ArrayServiceTreeToArrOptions
} from './interface';


class ArrayService {

    public c: ArrayConfig;

    constructor() {
        this.configService(null);
    }

    /**
     * 配置服务
     * @param config
     */
    public configService(config: ArrayConfig) {
        this.c = {
            deepMapName: 'deep',
            parentMapName: 'parent',
            idMapName: 'id',
            parentIdMapName: 'parent_id',
            childrenMapName: 'children',
            titleMapName: 'title',
            checkedMapname: 'checked',
            selectedMapname: 'selected',
            expandedMapname: 'expanded',
            disabledMapname: 'disabled',
            ...(config && config),
        };
    }


    /**
     * 将树结构转换成数组结构
     */
    public treeToArr(tree: any[], options?: ArrayServiceTreeToArrOptions): any[] {
        const opt = {
            deepMapName: this.c.deepMapName,
            parentMapName: this.c.parentMapName,
            childrenMapName: this.c.childrenMapName,
            clearChildren: true,
            cb: null,
            ...options,
        } as ArrayServiceTreeToArrOptions;
        const result: any[] = [];
        const inFn = (list: any[], parent: any, deep: number = 0) => {
            for (const i of list) {
                i[opt.deepMapName!] = deep;
                i[opt.parentMapName!] = parent;
                if (opt.cb) {
                    opt.cb(i, parent, deep);
                }
                result.push(i);
                const children = i[opt.childrenMapName!];
                if (children != null && Array.isArray(children) && children.length > 0) {
                    inFn(children, i, deep + 1);
                }
                if (opt.clearChildren) {
                    delete i[opt.childrenMapName!];
                }
            }
        };
        inFn(tree, 1);
        return result;
    }

    /**
     * 数组转换成树数据
     */
    public arrToTree(arr: any[], options?: ArrayServiceArrToTreeOptions): any[] {
        const opt = {
            idMapName: this.c.idMapName,
            parentIdMapName: this.c.parentIdMapName,
            childrenMapName: this.c.childrenMapName,
            cb: null,
            ...options,
        } as ArrayServiceArrToTreeOptions;
        const tree: any[] = [];
        const childrenOf = {};
        for (const item of arr) {
            const id = item[opt.idMapName!];
            const pid = item[opt.parentIdMapName!];
            childrenOf[id] = childrenOf[id] || [];
            item[opt.childrenMapName!] = childrenOf[id];
            if (opt.cb) {
                opt.cb(item);
            }
            if (pid) {
                childrenOf[pid] = childrenOf[pid] || [];
                childrenOf[pid].push(item);
            } else {
                tree.push(item);
            }
        }
        return tree;
    }

    /**
     * 数组转换成 `nz-tree` 数据源，通过 `options` 转化项名，也可以使用 `options.cb` 更高级决定数据项
     */
    public arrToTreeNode(arr: any[], options?: ArrayServiceArrToTreeNodeOptions): any[] {
        const opt = {
            idMapName: this.c.idMapName,
            parentIdMapName: this.c.parentIdMapName,
            titleMapName: this.c.titleMapName,
            isLeafMapName: 'isLeaf',
            checkedMapname: this.c.checkedMapname,
            selectedMapname: this.c.selectedMapname,
            expandedMapname: this.c.expandedMapname,
            disabledMapname: this.c.disabledMapname,
            cb: null,
            ...options,
        } as ArrayServiceArrToTreeNodeOptions;
        const tree = this.arrToTree(arr, {
            idMapName: opt.idMapName,
            parentIdMapName: opt.parentIdMapName,
            childrenMapName: 'children',
        });
        this.visitTree(tree, (item: any, parent: any, deep: number) => {
            item.key = item[opt.idMapName!];
            item.title = item[opt.titleMapName!];
            item.checked = item[opt.checkedMapname!];
            item.selected = item[opt.selectedMapname!];
            item.expanded = item[opt.expandedMapname!];
            item.disabled = item[opt.disabledMapname!];
            if (item[opt.isLeafMapName!] == null) {
                item.isLeaf = item.children.length === 0;
            } else {
                item.isLeaf = item[opt.isLeafMapName!];
            }
            if (opt.cb) {
                opt.cb(item, parent, deep);
            }

            item.origin = item;
        });


        return tree;

        // return tree.map(node => new NzTreeNode(node));
    }

    /**
     * 递归访问整个树
     */
    public visitTree(
        tree: any[],
        cb: (item: any, parent: any, deep: number) => void,
        options?: {
            /** 子项名，默认：`'children'` */
            childrenMapName?: string;
        }
    ): void {
        options = {
            childrenMapName: this.c.childrenMapName,
            ...options,
        };
        const inFn = (data: any[], parent: any, deep: number) => {
            for (const item of data) {
                cb(item, parent, deep);
                const childrenVal = item[options!.childrenMapName!];
                if (childrenVal && childrenVal.length > 0) {
                    inFn(childrenVal, item, deep + 1);
                }
            }
        };
        inFn(tree, null, 1);
    }

    /**
     * 获取所有已经选中的 `key` 值
     */
    public getKeysByTreeNode(tree: any[], options?: ArrayServiceGetKeysByTreeNodeOptions): any[] {
        const opt = {
            includeHalfChecked: true,
            ...options,
        } as ArrayServiceGetKeysByTreeNodeOptions;
        const keys: any[] = [];
        this.visitTree(tree, (item: any, parent: any, deep: number) => {
            if (item.isChecked || (opt.includeHalfChecked && item.isHalfChecked)) {
                keys.push(opt.cb ? opt.cb(item, parent, deep) : opt.keyMapName ? item.origin[opt.keyMapName] : item.key);
            }
        });
        return keys;
    }
}

const arrayService = new ArrayService();
export default arrayService;
