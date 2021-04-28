<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">{{l('EditText')}}</div>
        </div>
        <a-form :form="form" @submit="save()" autocomplete="off">
            <!-- 键值 -->
            <a-form-item
                :label="l('Key')">
                <p>{{editItem.key }}</p>
            </a-form-item>
            <!-- 简体中文 -->
            <a-form-item
                :label="editItem.baseLanguageNameText">
                <p>{{ editItem.baseValue }}</p>
            </a-form-item>
            <!-- 启用 -->
            <a-form-item
                :label="editItem.targetLanguageNameText">
                <a-textarea
                    v-decorator="['value', {}]"
                    :placeholder="editItem.targetLanguageNameText"
                    :auto-size="{ minRows: 3, maxRows: 5 }" />
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
    name: "edit-language-text",
    mixins: [ModalComponentBase],
    components: {},
    data() {
        return {
            form: this.$form.createForm(this),
            languageServiceProxy: "",
            spinning: false
        };
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._languageServiceProxy = new LanguageServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.$nextTick(() => {
            this.form.setFieldsValue({
                value: this.editItem.targetValue
            });
        });
        console.log(this.editItem);
    },
    methods: {
        /**
         * 提交
         */
        save() {
            this.form.validateFields((err, values) => {
                if (!err) {
                    console.log(this.editItem);
                    let parmas = {
                        key: this.editItem.key,
                        languageName: this.editItem.targetLanguageName,
                        sourceName: this.editItem.sourceName,
                        value: values.value
                    };
                    this.spinning = true;
                    this._languageServiceProxy
                        .updateLanguageText(parmas)
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
