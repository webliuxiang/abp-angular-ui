<template>
    <a-spin :spinning="spinning">
        <!-- 标题 -->
        <div class="modal-header">
            <div class="modal-title">
                <span>{{ l("Move") }}</span>
            </div>
        </div>
        <a-tree-select
            v-model="swelectvalue"
            show-search
            :tree-data="treeData"
            style="width:100%"
            :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
            :placeholder="l('Move')"
            allow-clear>
        </a-tree-select>
        <!-- 按钮 -->
        <div class="modal-footer">
            <a-button :disabled="saving" @click="close()" type="button">
                <a-icon type="close-circle" />
                {{ l('Cancel') }}
            </a-button>
            <a-button :loading="saving" :type="'primary'" @click="handleSubmit()">
                <a-icon type="save" />
                {{ l('Save') }}
            </a-button>
        </div>
    </a-spin>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
import { SysFileServiceProxy } from "@/shared/service-proxies";
import { arrayService } from "@/shared/utils";

export default {
    name: "move-file",
    mixins: [ModalComponentBase],
    data() {
        return {
            spinning: false,
            name: "",
            _sysFileServiceProxy: "",
            swelectvalue: undefined,
            directoryLists: [],
            treeData: [],
            moveId: ""
        };
    },
    created() {
        this._sysFileServiceProxy = new SysFileServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.fullData(); // 模态框必须,填充数据到data字段
    },
    mounted() {
        this.getData();
        this.moveId = this.item.parentId;
    },
    methods: {
        getData() {
            this.spinning = true;
            this._sysFileServiceProxy
                .getDirectories()
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    res.splice(0, 0, {
                        id: "",
                        parentId: null,
                        name: "根目录"
                    }); // 往数组中添加一个对象
                    res.forEach(element => {
                        this.directoryLists.push({
                            key: element.id,
                            value: element.id,
                            parentId: element.parentId,
                            code: element.code,
                            title: element.name
                        });
                    });
                    this.treeData = this.listConvertTree(
                        this.directoryLists,
                        null
                    );
                    this.swelectvalue = this.moveId;
                });
        },
        listConvertTree(data, parentId) {
            let convertData = [];
            let isdisabled = false;
            data.forEach((item, index) => {
                if (item.parentId === this.moveId) {
                    item.disabled = true;
                    isdisabled = true;
                } else {
                    item.disabled = false;
                    isdisabled = false;
                }
                if (item.parentId === parentId) {
                    convertData.push(item);
                    this.convertChild(data, item, item.key, isdisabled);
                }
            });
            return convertData;
        },
        convertChild(arr, parentItem, parentId, isdisabled) {
            parentItem.children = parentItem.children
                ? parentItem.children
                : [];
            arr.forEach(item => {
                item.disabled = isdisabled;
                if (item.parentId === parentId) {
                    parentItem.children.push(item);
                    this.convertChild(arr, item, item.key);
                }
            });
            return parentItem.children;
        },
        /**
         * 提交数据
         */
        handleSubmit() {
            this.spinning = true;
            this._sysFileServiceProxy
                .move({
                    id: this.item.id,
                    newParentId: this.swelectvalue
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.notify.success(this.l("SavedSuccessfully"));
                    this.success(true);
                });
        }
    }
};
</script>

<style lang="less" scoped>
</style>
