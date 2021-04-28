<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">
                <a-icon type="safety-certificate" />
                <span>{{ l('Permissions') }}: <span v-if="userName"> - {{ userName }}</span></span>
            </div>
        </div>
        <!-- 提示 -->
        <a-alert :message="l('Note_RefreshPageForPermissionChanges')" type="warning" closable showIcon />
        <search-role-tree
            v-if="isshowPermissionTree"
            :multiple="true"
            :dropDownStyle="{ 'max-height': '500px' }"
            :selectedPermission="selectedPermission"
            @selectedPermissionChange="refreshGoFirstPage">
        </search-role-tree>
        <!-- 按钮 -->
        <div class="modal-footer">
            <a-button :disabled="saving" @click="resetPermissions()" :loading="resettingPermissions" type="button">
                <a-icon type="reload" />
                {{ l('ResetSpecialPermissions') }}
            </a-button>
            <a-button :disabled="saving || resettingPermissions" @click="close()" type="button">
                <a-icon type="close-circle" />
                {{ l('Cancel') }}
            </a-button>
            <a-button :loading="saving || resettingPermissions" :type="'primary'" @click="handleSubmit()">
                <a-icon type="save" />
                {{ l('Save') }}
            </a-button>
        </div>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { ModalComponentBase } from "@/shared/component-base";
import { UserServiceProxy } from "@/shared/service-proxies";
import Bus from "@/shared/bus/bus";
import SearchRoleTree from "../../roles/search-role-tree/index.vue";

export default {
    name: "create-or-edit-role",
    mixins: [AppComponentBase, ModalComponentBase],
    data() {
        return {
            spinning: false,
            // 选中的权限过滤
            selectedPermission: [],
            resettingPermissions: false,
            _userServiceProxy: "",
            isshowPermissionTree: false
        };
    },
    components: {
        SearchRoleTree
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._userServiceProxy = new UserServiceProxy(this.$apiUrl, this.$api);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._userServiceProxy
                .getPermissionsTreeForEdit(this.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    console.log(result.grantedPermissionNames);
                    this.selectedPermission = result.grantedPermissionNames;
                    this.isshowPermissionTree = true;
                });
        },
        /**
         * 提交表单
         */
        handleSubmit() {
            this.saving = true;
            this.spinning = true;
            let input = {};
            input.id = this.id;
            input.grantedPermissionNames = this.selectedPermission;
            this._userServiceProxy
                .updatePermissions(input)
                .finally(() => {
                    this.saving = false;
                    this.spinning = false;
                })
                .then(() => {
                    this.notify.success(this.l("SavedSuccessfully"));
                    this.success(true);
                });
        },
        /**
         * 选择完权限过滤
         */
        refreshGoFirstPage(data) {
            this.selectedPermission = data;
        },
        /**
         * 重置权限
         */
        resetPermissions() {
            let input = {};
            input.id = this.id;
            this.resettingPermissions = true;
            this._userServiceProxy
                .resetSpecificPermissions(input)
                .finally(() => {
                    this.resettingPermissions = false;
                })
                .then(() => {
                    this.notify.success(this.l("ResetSuccessfully"));
                    this._userServiceProxy
                        .getPermissionsTreeForEdit(this.id)
                        .then(result => {
                            this.selectedPermission = result;
                        });
                });
        }
    }
};
</script>

<style scoped lang="less">
.modal-header {
    .anticon {
        margin-right: 10px;
    }
}
.pagination {
    margin: 10px auto;
    text-align: right;
}
.ant-alert {
    margin-bottom: 10px;
}
</style>
