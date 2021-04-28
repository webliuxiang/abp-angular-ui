<template>
    <a-spin :spinning="spinning">
        <!-- 标题 -->
        <div class="modal-header">
            <div class="modal-title">
                <span>创建文件夹</span>
            </div>
        </div>
        <a-input placeholder="创建文件夹" v-model="name" />
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

export default {
    name: "create-directory",
    mixins: [ModalComponentBase],
    data() {
        return {
            spinning: false,
            name: "",
            _sysFileServiceProxy: "'"
        };
    },
    created() {
        this._sysFileServiceProxy = new SysFileServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.fullData(); // 模态框必须,填充数据到data字段
    },
    mounted() {},
    methods: {
        /**
         * 提交数据
         */
        handleSubmit() {
            this.spinning = true;
            this._sysFileServiceProxy
                .createDirectory({
                    name: this.name,
                    parentId: this.parentId
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
