<template>
    <div>
        <a-input :placeholder="l('InXSearchPlaceHolder', l('PermissionDisplayName'))" @change="onSearch" size="default">
            <a-icon slot="addonAfter" type="reload" @click.prevent="reload($event)" />
        </a-input>
        <a-tree
            v-if="treeData.length"
            checkable
            defaultExpandAll
            v-model="selectedPermissionVal"
            :selectedKeys="selectedKeys"
            @check="onCheck"
            :treeData="treeData" />
    </div>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { PermissionServiceProxy } from "@/shared/service-proxies";
import { arrayService } from "@/shared/utils";

export default {
    name: "search-role-tree",
    mixins: [AppComponentBase],
    props: ["multiple", "dropDownStyle", "selectedPermission"],
    data() {
        return {
            permissionService: null,
            permissions: [],
            // loading: false,
            treeData: [],
            treeDataOrgin: [],
            selectedPermissionVal: this.selectedPermission,
            checkedKeys: [],
            selectedKeys: [],
            filterText: "",
            // 模糊查询
            dataList: []
        };
    },
    computed: {},
    created() {
        this.permissionService = new PermissionServiceProxy(
            this.$apiUrl,
            this.$api
        );
    },
    mounted() {
        this.permissionService.getAllPermissions().then(result => {
            this.permissions = result.items;
            this.permissions.map(item => {
                item.value = item.name;
            });
            this.arrToTreeNode();
        });
    },
    watch: {
        checkedKeys(val) {
            console.log("onCheck", val);
        }
    },
    methods: {
        arrToTreeNode() {
            this.loading = true;
            this.treeData = arrayService.arrToTreeNode(this.permissions, {
                idMapName: "name",
                parentIdMapName: "parentName",
                titleMapName: "displayName"
            });

            // 延时设置子父节点checkbox关联状态，否则有父节点选中则全部选中了
            setTimeout(() => {
                this.loading = false;
            }, 3000);
            this.permissions.map(item => {
                this.dataList.push({
                    key: item.name,
                    title: item.displayName
                });
            });
            this.treeDataOrgin = [...this.treeData];
        },
        /**
         * 选中
         */
        onCheck(val) {
            this.selectedPermissionVal = val;
            this.$emit("selectedPermissionChange", val);
        },
        /**
         * 格式化数据 模糊匹配
         */
        getParentKey(key, tree) {
            let parentKey;
            for (let i = 0; i < tree.length; i++) {
                const node = tree[i];
                if (node.children) {
                    if (node.children.some(item => item.title === key)) {
                        parentKey = node.title;
                    } else if (this.getParentKey(key, node.children)) {
                        parentKey = this.getParentKey(key, node.children);
                    }
                }
            }
            return parentKey;
        },
        /**
         * 搜索
         */
        onSearch(e) {
            const value = e.target.value;
            this.treeData = this.rebuildData(value, this.treeDataOrgin);
        },
        rebuildData(value, arr) {
            let newarr = [];
            arr.forEach(element => {
                if (element.title.indexOf(value) > -1) {
                    newarr.push(element);
                } else {
                    if (element.children && element.children.length > 0) {
                        const ab = this.rebuildData(value, element.children);
                        const obj = {
                            ...element,
                            children: ab
                        };
                        if (ab && ab.length > 0) {
                            newarr.push(obj);
                        }
                    }
                }
            });
            return newarr;
        },
        /**
         * 刷新
         */
        reload() {
            this.treeData = this.treeDataOrgin;
        }
    },
    watch: {}
};
</script>

<style scoped>
</style>
