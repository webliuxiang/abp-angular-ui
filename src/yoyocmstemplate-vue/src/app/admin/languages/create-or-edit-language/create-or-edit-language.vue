<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title" v-if="!language.id">{{l('CreateNewLanguage')}}</div>
            <div class="modal-title" v-if="language.id">
                {{l('EditLanguage')}}:
                <span>{{language.name}}</span>
            </div>
        </div>
        <a-form :form="form" @submit="save()" autocomplete="off">
            <!-- 语言名称 -->
            <a-form-item
                :label="l('LanguageName')">
                <a-select v-decorator="['name', {
                        rules: [
                            { required: true, message: l('ThisFieldIsRequired') },
                        ]
                    }]">
                    <a-select-option v-for="item in languageNames" :key="item.value" :value="item.value">
                        {{ item.displayText }}
                    </a-select-option>
                </a-select>
            </a-form-item>
            <!-- 语言图标 -->
            <a-form-item
                :label="l('LanguageIcon')">
                <a-select v-decorator="['icon', {
                        rules: [
                            { required: true, message: l('ThisFieldIsRequired') },
                        ]
                    }]">
                    <a-select-option v-for="item in flags" :key="item.value" :value="item.value">
                        {{ item.displayText }}
                    </a-select-option>>
                </a-select>
            </a-form-item>
            <!-- 启用 -->
            <a-form-item
                :label="l('Enabled')">
                <a-switch v-decorator="['isEnabled', {valuePropName: 'checked'} ]" :checkedChildren="l('Yes')" :unCheckedChildren="l('No')" />
            </a-form-item>
        </a-form>

        <div class="modal-footer">
            <a-button :disabled="saving" @click="close()" type="button">
                {{l("Cancel")}}
            </a-button>
            <a-button :loading="saving" :type="'primary'" @click="save()">
                <i class="acticon acticon-save"></i>
                <span>{{l("Save")}}</span>
            </a-button>
        </div>
    </a-spin>
</template>
<script>
import { ModalComponentBase } from "@/shared/component-base";
import { LanguageServiceProxy } from "@/shared/service-proxies";

export default {
    name: "create-or-edit-language",
    mixins: [ModalComponentBase],
    components: {},
    data() {
        return {
            form: this.$form.createForm(this),
            languageServiceProxy: "",
            spinning: false,
            // 语言数据
            language: {},
            // 语言名称
            languageNames: [],
            // 图表名称
            flags: []
        };
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._languageServiceProxy = new LanguageServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.getData();
    },
    methods: {
        getData() {
            this.spinning = true;
            this._languageServiceProxy
                .getLanguageForEdit(this.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.language = res.language;
                    this.flags = res.flags;
                    this.languageNames = res.languageNames;
                    this.form.setFieldsValue({
                        isEnabled: this.id ? this.language.isEnabled : true,
                        icon: this.language.icon,
                        name: this.language.name
                    });
                    this.id = this.language.id;
                    console.log(res);
                });
        },
        /**
         * 提交
         */
        save() {
            this.form.validateFields((err, values) => {
                if (!err) {
                    console.log(values);
                    this.spinning = true;
                    this._languageServiceProxy
                        .createOrUpdateLanguage({
                            language: Object.assign(values, { id: this.id })
                        })
                        .finally(() => {
                            this.spinning = false;
                        })
                        .then(res => {
                            this.notify.success(this.l("SavedSuccessfully"));
                            this.success(true);
                            console.log(res);
                        });
                }
            });
        }
    }
};
</script>
