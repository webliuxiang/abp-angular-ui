<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">
                <a-icon type="medicine-box" />
                <span v-if="role.id">{{ l('EditRole') }}:{{ role.displayName }}</span>
                <span v-if="!role.id">{{ l('CreateNewRole') }}</span>
            </div>
        </div>
        <!-- 提示 -->
        <a-alert :message="l('Note_RefreshPageForPermissionChanges')" type="info" closable showIcon />
        <!-- tab切换 -->
        <a-tabs defaultActiveKey="1">
            <!-- 角色名称 -->
            <a-tab-pane key="1">
                <span slot="tab">
                    <a-icon type="medicine-box" />
                    {{ l('RoleName') }}
                </span>
                <a-form :form="form" :label-col="{ span: 5 }" :wrapper-col="{ span: 12 }" @submit="handleSubmit">
                    <a-form-item :label="l('RoleName')">
                        <a-input :placeholder="l('RoleName')"
                            v-decorator="['displayName', { rules: [
                                { required: true, message: l('ThisFieldIsRequired') },
                                { message: l('MaxLength'),max:64  }
                             ] }]" />
                    </a-form-item>
                    <!-- <a-form-item :label="l('RoleName')">
                        <a-switch checkedChildren="开" unCheckedChildren="关" defaultChecked />
                    </a-form-item> -->
                    <a-form-item>
                        <span slot="label">
                            {{ l('Default') }}&nbsp;
                            <a-tooltip :title="l('DefaultRole_Description')">
                                <a-icon type="question-circle-o" />
                            </a-tooltip>
                        </span>
                        <a-switch v-decorator="['isDefault', {valuePropName: 'checked'} ]" :checkedChildren="l('Yes')" :unCheckedChildren="l('No')" />
                    </a-form-item>
                </a-form>
            </a-tab-pane>
            <!-- 权限 -->
            <a-tab-pane key="2">
                <span slot="tab">
                    <a-icon type="safety-certificate" />
                    {{ l('Permissions') }}
                </span>
                <search-role-tree
                    :multiple="true"
                    :dropDownStyle="{ 'max-height': '500px' }"
                    :selectedPermission="selectedPermission"
                    @selectedPermissionChange="refreshGoFirstPage">
                </search-role-tree>
            </a-tab-pane>
        </a-tabs>
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
import { AppComponentBase } from "@/shared/component-base";
import { ModalComponentBase } from "@/shared/component-base";
import { RoleServiceProxy } from "@/shared/service-proxies";
import Bus from "@/shared/bus/bus";
import SearchRoleTree from "../search-role-tree/index.vue";

export default {
    name: "create-or-edit-role",
    mixins: [AppComponentBase, ModalComponentBase],
    data() {
        return {
            spinning: false,
            // 表单
            formLayout: "horizontal",
            form: this.$form.createForm(this, { name: "coordinated" }),
            // 选中的权限过滤
            selectedPermission: [],
            role: {
                // id: 1,
                // displayName: "ceshi"
            }
        };
    },
    components: {
        SearchRoleTree
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._roleServiceProxy = new RoleServiceProxy(this.$apiUrl, this.$api);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._roleServiceProxy
                .getForEdit(this.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    console.log(result);
                    this.form.setFieldsValue({
                        displayName: result.role.displayName,
                        isDefault: result.role.isDefault
                    });
                    let sameObj = result.permissions.filter(obj =>
                        result.grantedPermissionNames.some(
                            obj1 => obj1 == obj.name
                        )
                    );
                    sameObj.map(item => {
                        this.selectedPermission.push(item.name);
                    });
                });
        },
        /**
         * 提交表单
         */
        handleSubmit() {
            this.form.validateFields((err, values) => {
                if (!err) {
                    console.log("Received values of form: ", values);
                    let parmas = {
                        role: {
                            displayName: values.displayName,
                            id: this.id,
                            isDefault: values.isDefault
                                ? values.isDefault
                                : false
                        },
                        grantedPermissionNames: this.selectedPermission
                    };
                    this.saving = true;
                    this.spinning = true;
                    this._roleServiceProxy
                        .createOrUpdate(parmas)
                        .finally(() => {
                            this.saving = false;
                            this.spinning = false;
                        })
                        .then(() => {
                            this.notify.success(this.l("SavedSuccessfully"));
                            this.success(true);
                        });
                }
            });
        },
        /**
         * 选择完权限过滤
         */
        refreshGoFirstPage(data) {
            this.selectedPermission = data;
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
</style>
