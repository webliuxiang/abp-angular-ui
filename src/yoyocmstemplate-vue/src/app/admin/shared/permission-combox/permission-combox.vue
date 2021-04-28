<template>
    <a-tree-select
        :dropdownStyle="dropDownStyle"
        :multiple="multiple"
        :placeholder="l('FilterByPermission')"
        :value="selectedPermissionVal"
        :treeData="treeData"
        @change="selectedChange($event)"
        allowClear
        showSearch
        style="width: 100%"
        treeDefaultExpandAll>
    </a-tree-select>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { PermissionServiceProxy } from "@/shared/service-proxies";
import { arrayService } from "@/shared/utils";

export default {
    name: "permission-combox",
    mixins: [AppComponentBase],
    props: ["multiple", "dropDownStyle", "selectedPermission"],
    data() {
        return {
            permissionService: null,
            permissions: [],
            // loading: false,
            treeData: [],
            selectedPermissionVal: this.selectedPermission
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
            }, 500);
            console.log(this.treeData);
        },
        selectedChange(val) {
            console.log(val);
            this.selectedPermissionVal = val;
            this.$emit("selectedPermissionChange", val);
        }
    },
    watch: {}
};
</script>

<style scoped>
</style>
