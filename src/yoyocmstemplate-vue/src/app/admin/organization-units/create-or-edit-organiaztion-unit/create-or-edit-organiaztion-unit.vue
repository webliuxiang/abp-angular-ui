<template>
    <section>
        <div class="modal-header">
            <div class="modal-title" v-if="organizationUnit.id">
                <a-icon type="share-alt" />{{ l('Edit') }}: {{ organizationUnit.displayName }}</div>
            <div class="modal-title" v-if="!organizationUnit.id && !organizationUnit.parentId">
                <a-icon type="share-alt" />{{ l('NewOrganizationUnit') }}</div>
            <div class="modal-title" v-if="organizationUnit.parentId && organizationUnit.parentDisplayName">
                <a-icon type="share-alt" />{{l('AddSubNodeForXParentNode', organizationUnit.parentDisplayName)}}</div>
        </div>
        <a-form :form="form" :label-col="{ span: 5 }" :wrapper-col="{ span: 12 }">
            <a-form-item :label="l('NameOfThings')">
                <a-input :placeholder="l('NameOfThings')"
                    v-decorator="['displayName', { rules: [
                            { required: true, message: l('ThisFieldIsRequired') },
                            { max:128, message: l('MaxLength')  }
                        ] }]" />
            </a-form-item>
        </a-form>

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
    </section>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { ModalComponentBase } from "@/shared/component-base";
import { OrganizationUnitServiceProxy } from "@/shared/service-proxies";

export default {
    name: "create-or-edit-organiaztion-unit",
    mixins: [AppComponentBase, ModalComponentBase],
    data() {
        return {
            // 表单
            formLayout: "horizontal",
            form: this.$form.createForm(this, { name: "coordinated" }),
            _organizationUnitServiceProxy: null
        };
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._organizationUnitServiceProxy = new OrganizationUnitServiceProxy(
            this.$apiUrl,
            this.$api
        );
        console.log(this.organizationUnit);
        // 修改
        if (this.organizationUnit) {
            this.$nextTick(() => {
                this.form.setFieldsValue({
                    displayName: this.organizationUnit.displayName
                });
            });
        }
    },
    methods: {
        /**
         * 提交表单
         */
        handleSubmit() {
            this.form.validateFields((err, values) => {
                if (!err) {
                    console.log("Received values of form: ", values);
                    if (this.organizationUnit.id) {
                        this.updateUnit(values);
                    } else {
                        // 创建
                        this.createUnit(values);
                    }
                }
            });
        },
        /**
         * 创建
         */
        createUnit(parmas) {
            Object.assign(parmas, {
                parentId: this.organizationUnit.parentId
            });
            this.saving = true;
            this._organizationUnitServiceProxy
                .create(parmas)
                .finally(() => {
                    this.saving = false;
                })
                .then(result => {
                    this.$notification["success"]({
                        message: this.l("SavedSuccessfully")
                    });
                    this.success(result);
                });
        },
        /**
         * 编辑
         */
        updateUnit(value) {
            this.organizationUnit.displayName = value.displayName;
            this.saving = true;
            this._organizationUnitServiceProxy
                .update(this.organizationUnit)
                .finally(() => {
                    this.saving = false;
                })
                .then(result => {
                    this.notify.success(this.l("SavedSuccessfully"));
                    this.success(result);
                });
        }
    }
};
</script>

<style scoped lang="less">
.modal-header {
    .anticon-share-alt {
        margin-right: 10px;
    }
}
</style>
