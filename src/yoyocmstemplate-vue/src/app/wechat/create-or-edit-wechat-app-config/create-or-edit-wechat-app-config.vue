<template>
    <a-spin :spinning="spinning">
        <!-- 标题 -->
        <div class="modal-header">
            <div class="modal-title">
                <span v-if="entity && entity.id">{{ l('Edit') }}</span>
                <span v-if="!entity">{{ l('Create') }}</span>
            </div>
        </div>
        <a-form :form="form" :label-col="{ span: 5 }" :wrapper-col="{ span: 12 }">
            <a-form-item :label="l('WechatName')">
                <a-input :placeholder="l('WechatName')"
                    v-decorator="['name', { rules: [{ required: true, message: l('CanNotNull') }] }]" />
            </a-form-item>
            <a-form-item :label="l('WechatAppId')">
                <a-input :placeholder="l('WechatAppId')"
                    v-decorator="['appId', { rules: [{ required: true, message: l('CanNotNull') }] }]" />
            </a-form-item>
            <a-form-item :label="l('WechatAppSecret')">
                <a-input :placeholder="l('WechatAppSecret')"
                    v-decorator="['appSecret', { rules: [{ required: true, message: l('CanNotNull') }] }]" />
            </a-form-item>
            <a-form-item :label="l('WechatToken')">
                <a-input :placeholder="l('WechatToken')"
                    v-decorator="['token', { rules: [{ required: true, message: l('CanNotNull') }] }]" />
            </a-form-item>
            <a-form-item :label="l('WechatEncodingAESKey')">
                <a-input :placeholder="l('WechatEncodingAESKey')"
                    v-decorator="['encodingAESKey', { rules: [] }]" />
            </a-form-item>
            <a-form-item :label="l('WechatAppOrgId')">
                <a-input :placeholder="l('WechatAppOrgId')"
                    v-decorator="['appOrgId', { rules: [] }]" />
            </a-form-item>
            <a-form-item :label="l('WechatAppType')">
                <a-select :placeholder="l('WechatAppType')" v-decorator="['appType', { rules: [] }]">
                    <a-select-option v-for="item in wechatAppTypeList" :key="item.value" :value="item.value">
                        {{ item.key }}
                    </a-select-option>
                </a-select>
            </a-form-item>
            <a-form-item :label="l('QRCodeUrl')">
                <a-input :placeholder="l('QRCodeUrl')"
                    v-decorator="['qrCodeUrl', { rules: [] }]" />
            </a-form-item>
        </a-form>
        <!-- 按钮 -->
        <div class="modal-footer">
            <a-button :disabled="saving" @click="close()" type="button">
                <a-icon type="close-circle" />
                {{ l('Cancel') }}
            </a-button>
            <a-button :loading="saving" :type="'primary'" @click="handleSubmit">
                <a-icon type="save" />
                {{ l('Save') }}
            </a-button>
        </div>
    </a-spin>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
import { WechatAppConfigServiceProxy } from "@/shared/service-proxies";

export default {
    name: "create-or-edit-wechat-app-config",
    mixins: [ModalComponentBase],
    data() {
        return {
            spinning: false,
            name: "",
            _wechatAppConfigServiceProxy: "",
            formLayout: "horizontal",
            form: this.$form.createForm(this, { name: "coordinated" }),
            wechatAppTypeList: []
        };
    },
    created() {
        this._wechatAppConfigServiceProxy = new WechatAppConfigServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.fullData(); // 模态框必须,填充数据到data字段
    },
    mounted() {
        console.log(this.entity);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._wechatAppConfigServiceProxy
                .getForEdit(this.entity ? this.entity.id : "")
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.wechatAppTypeList = res.wechatAppTypeList;
                    this.form.setFieldsValue({
                        name: res.wechatAppConfig.name,
                        appId: res.wechatAppConfig.appId,
                        appSecret: res.wechatAppConfig.appSecret,
                        token: res.wechatAppConfig.token,
                        encodingAESKey: res.wechatAppConfig.encodingAESKey,
                        appOrgId: res.wechatAppConfig.appOrgId,
                        qrCodeUrl: res.wechatAppConfig.qrCodeUrl,
                        appType: res.wechatAppTypeList.find(
                            item => item.key === res.wechatAppConfig.appType
                        )
                            ? res.wechatAppTypeList.find(
                                  item =>
                                      item.key === res.wechatAppConfig.appType
                              ).value
                            : res.wechatAppTypeList[0].value
                    });
                    console.log(res);
                });
        },
        /**
         * 提交数据
         */
        handleSubmit(e) {
            e.preventDefault();
            this.form.validateFields((err, values) => {
                if (!err) {
                    console.log("Received values of form: ", values);
                    let parmas = Object.assign(values, {
                        id: this.entity ? this.entity.id : ""
                    });
                    this.spinning = true;
                    this._wechatAppConfigServiceProxy
                        .createOrUpdate({ wechatAppConfig: parmas })
                        .finally(() => {
                            this.spinning = false;
                        })
                        .then(res => {
                            this.notify.success(this.l("SavedSuccessfully"));
                            this.success(true);
                        });
                }
            });
        }
    }
};
</script>

<style lang="less" scoped>
</style>
