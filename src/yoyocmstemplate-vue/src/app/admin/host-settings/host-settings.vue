<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('Settings')"></page-header>
        <a-card :bordered="false">
            <a-tabs v-if="hostSettings" @change="tabChange">
                <!-- 基本信息 -->
                <a-tab-pane :tab="l('General')" key="1" v-if="showTimezoneSelection">
                    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 12 }">
                        <a-form-item :label="l('Timezone')">
                            <timezone-combo :selectedTimeZone="hostSettings.general.timezone" :defaultTimezoneScope="defaultTimezoneScope" @selectedTimeZoneChange="selectedTimeZoneChange"></timezone-combo>
                        </a-form-item>
                    </a-form>
                </a-tab-pane>
                <!-- 租户管理设置 -->
                <a-tab-pane :tab="l('TenantManagement')" key="2" v-if="hostSettings.tenantManagement">
                    <section>
                        <h3>{{ l('FormBasedRegistration') }}</h3>
                        <!-- 允许租户注册到系统 -->
                        <a-checkbox v-model="hostSettings.tenantManagement.allowSelfRegistration">{{ l('AllowTenantsToRegisterThemselves') }}</a-checkbox>
                        <p>{{ l('AllowTenantsToRegisterThemselves_Hint') }}</p>
                        <br>
                        <!-- 新注册租户默认激活 -->
                        <a-checkbox v-model="hostSettings.tenantManagement.isNewRegisteredTenantActiveByDefault" v-if="hostSettings.tenantManagement.allowSelfRegistration">{{ l('NewRegisteredTenantsIsActiveByDefault') }}</a-checkbox>
                        <p v-if="hostSettings.tenantManagement.allowSelfRegistration">{{ l('NewRegisteredTenantsIsActiveByDefault_Hint') }}</p>
                        <br>
                        <!-- 租户注册时使用图片验证码(captcha) -->
                        <a-checkbox v-model="hostSettings.tenantManagement.useCaptchaOnTenantRegistration" v-if="hostSettings.tenantManagement.allowSelfRegistration">{{ l('UseCaptchaOnTenantRegistration') }}</a-checkbox>
                        <p>
                            <br>
                        </p>
                        <!-- 租户注册图片验证码类型 -->
                        <div v-if="hostSettings.tenantManagement.useCaptchaOnTenantRegistration">
                            <p>{{ l('CaptchaOnTenantRegistrationType') }}:</p>
                            <a-select style="width: 100%" :defaultValue="hostSettings.tenantManagement.captchaOnTenantRegistrationType" @change="CaptchaOnTenantRegistrationTypeChange" :placeholder="l('CaptchaOnHostUserLoginType')">
                                <a-select-option v-for="item in imgCodeTypeList" :value="item.value" :key="item.value">{{ item.displayText }}</a-select-option>
                            </a-select>
                            <p>
                                <br>
                            </p>
                        </div>
                        <!-- 租户注册图片验证码长度  -->
                        <div v-if="hostSettings.tenantManagement.useCaptchaOnTenantRegistration">
                            <p>{{ l('CaptchaOnTenantRegistrationLength') }}:</p>
                            <a-input-number id="inputNumber" :min="4" :max="10" :step="1" v-model="hostSettings.tenantManagement.captchaOnTenantRegistrationLength" />
                            <p>
                                <br>
                            </p>
                        </div>
                        <!-- 版本  -->
                        <p>{{ l('Edition') }}:</p>
                        <a-select style="width: 100%" :defaultValue="hostSettings.tenantManagement.defaultEditionId" @change="versionHandleChange" :placeholder="l('Edition')">
                            <a-select-option v-for="item in versionList" :value="item.value" :key="item.value">{{ item.displayText }}</a-select-option>
                        </a-select>
                    </section>
                </a-tab-pane>
                <!-- 用户管理 -->
                <a-tab-pane :tab="l('UserManagement')" key="3" v-if="hostSettings.userManagement">
                    <!-- 必须验证邮箱地址后才能登录 -->
                    <a-checkbox v-model="hostSettings.userManagement.isEmailConfirmationRequiredForLogin">{{ l('EmailConfirmationRequiredForLogin') }}</a-checkbox>
                    <p></p>
                    <!-- 宿主用户登陆时使用图片验证码(captcha) -->
                    <a-checkbox v-model="hostSettings.userManagement.useCaptchaOnUserLogin">{{ l('UseCaptchaOnHostUserLogin') }}</a-checkbox>
                    <p></p>
                    <!-- 宿主用户登陆图片验证码类型   -->
                    <div v-if="hostSettings.userManagement.useCaptchaOnUserLogin">
                        <p>{{ l('CaptchaOnHostUserLoginType') }}:</p>
                        <a-select style="width: 100%" :defaultValue="hostSettings.userManagement.captchaOnUserLoginType" @change="useCaptchaOnUserLoginHandleChange" :placeholder="l('CaptchaOnHostUserLoginType')">
                            <a-select-option v-for="item in imgCodeTypeList" :value="item.value" :key="item.value">{{ item.displayText }}</a-select-option>
                        </a-select>
                        <p>
                            <br>
                        </p>
                    </div>
                    <!-- 宿主用户登陆图片验证码长度 -->
                    <div v-if="hostSettings.userManagement.useCaptchaOnUserLogin">
                        <p>{{ l('CaptchaOnHostUserLoginLength') }}:</p>
                        <a-input-number id="inputNumber" :min="4" :max="10" :step="1" v-model="hostSettings.userManagement.captchaOnUserLoginLength" />
                        <p>
                            <br>
                        </p>
                    </div>
                </a-tab-pane>
                <!-- 安全 -->
                <a-tab-pane :tab="l('Security')" key="4" v-if="hostSettings.security">
                    <!-- 密码复杂性 -->
                    <h3>{{ l('PasswordComplexity') }}</h3>
                    <a-checkbox v-model="hostSettings.security.useDefaultPasswordComplexitySettings">{{ l('UseDefaultSettings') }}</a-checkbox>
                    <br>
                    <a-checkbox v-model="hostSettings.security.passwordComplexity.requireDigit" :disabled="hostSettings.security.useDefaultPasswordComplexitySettings">{{ l('PasswordComplexity_RequireDigit') }}</a-checkbox>
                    <br>
                    <a-checkbox v-model="hostSettings.security.passwordComplexity.requireLowercase" :disabled="hostSettings.security.useDefaultPasswordComplexitySettings">{{ l('PasswordComplexity_RequireLowercase') }}</a-checkbox>
                    <br>
                    <a-checkbox v-model="hostSettings.security.passwordComplexity.requireNonAlphanumeric" :disabled="hostSettings.security.useDefaultPasswordComplexitySettings">{{ l('PasswordComplexity_RequireNonAlphanumeric') }}</a-checkbox>
                    <br>
                    <a-checkbox v-model="hostSettings.security.passwordComplexity.requireUppercase" :disabled="hostSettings.security.useDefaultPasswordComplexitySettings">{{ l('PasswordComplexity_RequireUppercase') }}</a-checkbox>
                    <p>{{ l('PasswordComplexity_RequiredLength') }}:</p>
                    <a-input-number id="inputNumber" :step="1" v-if="!hostSettings.security.useDefaultPasswordComplexitySettings" v-model="hostSettings.security.passwordComplexity.requiredLength" />
                    <a-input-number id="inputNumber" :step="1" disabled v-if="hostSettings.security.useDefaultPasswordComplexitySettings" v-model="hostSettings.security.defaultPasswordComplexity.requiredLength" />
                    <p>
                        <br>
                    </p>
                    <!-- 锁定用户 -->
                    <h3>{{ l('UserLockOut') }}</h3>
                    <a-checkbox v-model="hostSettings.security.userLockOut.isEnabled">{{ l('EnableUserAccountLockingOnFailedLoginAttemts') }}</a-checkbox>
                    <p></p>
                    <p v-if="hostSettings.security.userLockOut.isEnabled">{{ l('MaxFailedAccessAttemptsBeforeLockout') }}:</p>
                    <a-input-number v-if="hostSettings.security.userLockOut.isEnabled" id="inputNumber" :step="1" v-model="hostSettings.security.userLockOut.maxFailedAccessAttemptsBeforeLockout" />
                    <p v-if="hostSettings.security.userLockOut.isEnabled"></p>
                    <p v-if="hostSettings.security.userLockOut.isEnabled">{{ l('DefaultAccountLockoutDurationAsSeconds') }}:</p>
                    <a-input-number v-if="hostSettings.security.userLockOut.isEnabled" id="inputNumber" :step="1" v-model="hostSettings.security.userLockOut.defaultAccountLockoutSeconds" />
                    <!-- 两步认证登录 -->
                    <p></p>
                    <h3>{{ l('TwoFactorLogin') }}</h3>
                    <a-checkbox v-model="hostSettings.security.twoFactorLogin.isEnabled">{{ l('EnableTwoFactorLogin') }}</a-checkbox>
                    <p></p>
                    <a-checkbox v-model="hostSettings.security.twoFactorLogin.isEmailProviderEnabled" v-if="hostSettings.security.twoFactorLogin.isEnabled">{{ l('IsEmailVerificationEnabled') }}</a-checkbox>
                    <p></p>
                    <a-checkbox v-model="hostSettings.security.twoFactorLogin.isSmsProviderEnabled" v-if="hostSettings.security.twoFactorLogin.isEnabled">{{ l('IsSmsVerificationEnabled') }}</a-checkbox>
                    <p></p>
                    <a-checkbox v-model="hostSettings.security.twoFactorLogin.isGoogleAuthenticatorEnabled" v-if="hostSettings.security.twoFactorLogin.isEnabled">{{ l('IsGoogleAuthenticatorEnabled') }}</a-checkbox>
                    <p></p>
                    <a-checkbox v-model="hostSettings.security.twoFactorLogin.isRememberBrowserEnabled" v-if="hostSettings.security.twoFactorLogin.isEnabled">{{ l('AllowToRememberBrowserForTwoFactorLogin') }}</a-checkbox>
                    <p></p>
                </a-tab-pane>
                <!-- 邮箱 -->
                <a-tab-pane :tab="l('EmailSmtp')" key="5" v-if="hostSettings.userManagement">
                    <a-form :form="form">
                        <a-form-item v-bind="formItemLayout" :label="l('DefaultFromAddress')">
                            <a-input
                                v-decorator="[
                                'defaultFromAddress',
                                {
                                    rules: [
                                    {
                                        type: 'email',
                                        message: '电子邮箱格式不正确',
                                    },
                                    {
                                        required: true,
                                        message: l('ThisFieldIsRequired'),
                                    },
                                    {
                                        max:128,
                                        message:l('MaxLength')
                                    }
                                    ],
                                },
                                ]" />
                        </a-form-item>
                        <a-form-item v-bind="formItemLayout" :label="l('DefaultFromDisplayName')">
                            <a-input
                                v-decorator="[
                                'defaultFromDisplayName',
                                {
                                    rules: [
                                    {
                                        required: true,
                                        message: l('ThisFieldIsRequired'),
                                    },
                                    {
                                        max:128,
                                        message:l('MaxLength')
                                    }
                                    ],
                                },
                                ]" />
                        </a-form-item>
                        <a-form-item v-bind="formItemLayout" :label="l('SmtpHost')">
                            <a-input
                                v-decorator="[
                                'smtpHost',
                                {
                                    rules: [
                                    {
                                        required: true,
                                        message: l('ThisFieldIsRequired'),
                                    },
                                    {
                                        max:64,
                                        message:l('MaxLength')
                                    }
                                    ],
                                },
                                ]" />
                        </a-form-item>
                        <a-form-item v-bind="formItemLayout" :label="l('SmtpPort')">
                            <a-input
                                v-decorator="[
                                'smtpPort',
                                {
                                    rules: [
                                    {
                                        required: true,
                                        message: l('ThisFieldIsRequired'),
                                    },
                                    {
                                        max:64,
                                        message:l('MaxLength')
                                    }
                                    ],
                                },
                                ]" />
                        </a-form-item>
                        <a-form-item v-bind="formItemLayout">
                            <a-checkbox v-model="hostSettings.email.smtpEnableSsl">{{ l('UseSsl') }}</a-checkbox>
                        </a-form-item>
                        <a-form-item v-bind="formItemLayout">
                            <a-checkbox v-model="hostSettings.email.smtpUseDefaultCredentials">{{ l('UseDefaultCredentials') }}</a-checkbox>
                        </a-form-item>
                        <a-form-item v-bind="formItemLayout" :label="l('DomainName')" v-if="!hostSettings.email.smtpUseDefaultCredentials">
                            <a-input
                                v-decorator="[
                                'smtpDomain',
                                {
                                    rules: [
                                    {
                                        required: true,
                                        message: l('ThisFieldIsRequired'),
                                    },
                                    {
                                        max:128,
                                        message:l('MaxLength')
                                    }
                                    ],
                                },
                                ]" />
                        </a-form-item>
                        <a-form-item v-bind="formItemLayout" :label="l('UserName')" v-if="!hostSettings.email.smtpUseDefaultCredentials">
                            <a-input
                                v-decorator="[
                                'smtpUserName',
                                {
                                    rules: [
                                    {
                                        required: true,
                                        message: l('ThisFieldIsRequired'),
                                    },
                                    {
                                        max:128,
                                        message:l('MaxLength')
                                    }
                                    ],
                                },
                                ]" />
                        </a-form-item>
                        <a-form-item v-bind="formItemLayout" :label="l('Password')" v-if="!hostSettings.email.smtpUseDefaultCredentials">
                            <a-input
                                type="password"
                                v-decorator="[
                                'smtpPassword',
                                {
                                    rules: [
                                    {
                                        required: true,
                                        message: l('ThisFieldIsRequired'),
                                    },
                                    {
                                        max:128,
                                        message:l('MaxLength')
                                    }
                                    ],
                                },
                                ]" />
                        </a-form-item>
                        <a-form-item v-bind="formItemLayout" :label="l('TestEmailSettingsHeader')">
                            <a-input :placeholder="l('TestEmailSettingsHeader')"
                                v-decorator="[
                                    'testEmailSettingsHeader',
                                    {
                                        rules: [
                                        {
                                            required: true,
                                            message: l('ThisFieldIsRequired'),
                                        },
                                        {
                                            max:128,
                                            message:l('MaxLength')
                                        }
                                        ],
                                    },
                                ]">
                                <a-button type="primary" @click="sendTestEmail" slot="addonAfter">{{ l('SendTestEmail') }}</a-button>
                            </a-input>
                        </a-form-item>
                    </a-form>
                </a-tab-pane>
                <a-button :type="'primary'" @click="handleSubmit" slot="tabBarExtraContent">
                    <a-icon type="save" />
                    <span>{{l("SaveAll")}}</span>
                </a-button>
            </a-tabs>
        </a-card>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { ModalHelper } from "@/shared/helpers";
import {
    CommonLookupServiceProxy,
    HostSettingsServiceProxy
} from "@/shared/service-proxies";
import TimezoneCombo from "../shared/timezone-combo/timezone-combo";
import moment from "moment";

export default {
    mixins: [AppComponentBase],
    name: "host-settings",
    components: {
        TimezoneCombo
    },
    data() {
        return {
            hostSettings: {
                // 时区
                general: {
                    timezone: ""
                },
                // 租户管理
                tenantManagement: {
                    allowSelfRegistration: true,
                    isNewRegisteredTenantActiveByDefault: false,
                    useCaptchaOnTenantRegistration: false,
                    captchaOnTenantRegistrationLength: 4,
                    captchaOnTenantRegistrationType: ""
                },
                // 用户管理
                userManagement: {
                    isEmailConfirmationRequiredForLogin: false,
                    useCaptchaOnUserLogin: true,
                    captchaOnUserLoginType: "",
                    captchaOnUserLoginLength: 4
                },
                // 安全
                security: {
                    useDefaultPasswordComplexitySettings: true,
                    passwordComplexity: {
                        requireDigit: false,
                        requireLowercase: false,
                        requireNonAlphanumeric: false,
                        requireUppercase: false,
                        requiredLength: 3
                    },
                    defaultPasswordComplexity: {
                        requiredLength: 3
                    },
                    userLockOut: {
                        isEnabled: true,
                        maxFailedAccessAttemptsBeforeLockout: 5,
                        defaultAccountLockoutSeconds: 300
                    },
                    twoFactorLogin: {
                        isEnabled: false,
                        isEmailProviderEnabled: true,
                        isSmsProviderEnabled: true,
                        isGoogleAuthenticatorEnabled: false,
                        isRememberBrowserEnabled: true
                    }
                },
                // 邮箱
                email: {
                    smtpEnableSsl: false,
                    smtpUseDefaultCredentials: true
                }
            },
            // loading
            spinning: false,
            // 选择时区
            showTimezoneSelection: abp.clock.provider.supportsMultipleTimezone,
            defaultTimezoneScope: "Application",
            // 邮箱
            formItemLayout: {
                labelCol: {
                    xs: { span: 24 },
                    sm: { span: 24 }
                },
                wrapperCol: {
                    xs: { span: 24 },
                    sm: { span: 24 }
                }
            },
            _commonLookupServiceProxy: "",
            _hostSettingsServiceProxy: "",
            // 版本list
            versionList: [],
            // 图片验证码类型
            imgCodeTypeList: [],
            usingDefaultTimeZone: ""
        };
    },
    created() {
        this.form = this.$form.createForm(this, { name: "register" });
        this._commonLookupServiceProxy = new CommonLookupServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this._hostSettingsServiceProxy = new HostSettingsServiceProxy(
            this.$apiUrl,
            this.$api
        );
    },
    mounted() {
        this.spinning = true;
        this.getData();
        this.getimgCodeTypeList();
        this.getAllSetting();
        Promise.all([
            this.getData,
            this.getimgCodeTypeList,
            this.getAllSetting
        ]).then(res => {
            console.log(res);
            this.spinning = false;
        });
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this._commonLookupServiceProxy
                .getEditionsForCombobox(false)
                .finally(() => {})
                .then(res => {
                    this.versionList = res.items;
                });
        },
        /**
         * 获取图片验证嘛类型
         */
        getimgCodeTypeList() {
            this._commonLookupServiceProxy
                .getValidateCodeTypesForCombobox()
                .finally(() => {})
                .then(res => {
                    this.imgCodeTypeList = res.items;
                });
        },
        /**
         * 获取所有设置
         */
        getAllSetting() {
            this._hostSettingsServiceProxy
                .getAllSettings()
                .finally(() => {})
                .then(res => {
                    console.log(res);
                    this.hostSettings = res;
                });
        },
        /**
         * tab切换
         */
        tabChange(data) {
            if (data === "5") {
                // 设置邮箱
                this.$nextTick(() => {
                    this.form.setFieldsValue({
                        defaultFromAddress: this.hostSettings.email
                            .defaultFromAddress,
                        defaultFromDisplayName: this.hostSettings.email
                            .defaultFromDisplayName,
                        smtpHost: this.hostSettings.email.smtpHost,
                        smtpPort: String(this.hostSettings.email.smtpPort),
                        smtpEnableSsl: this.hostSettings.email.smtpEnableSsl,
                        smtpUseDefaultCredentials: this.hostSettings.email
                            .smtpUseDefaultCredentials,
                        smtpDomain: this.hostSettings.email.smtpDomain,
                        smtpUserName: this.hostSettings.email.smtpUserName,
                        smtpPassword: this.hostSettings.email.smtpPassword,
                        testEmailSettingsHeader: this.hostSettings.email
                            .defaultFromAddress
                    });
                });
            }
        },
        /**
         * 选择时区
         */
        selectedTimeZoneChange(data) {
            this.hostSettings.general.timezone = data;
        },
        /**
         * 选择版本
         */
        versionHandleChange(value) {
            this.hostSettings.tenantManagement.defaultEditionId = value;
            console.log(`selected ${value}`);
        },
        /**
         * 宿主用户登陆图片验证码类型
         */
        useCaptchaOnUserLoginHandleChange(value) {
            this.hostSettings.userManagement.captchaOnUserLoginType = value;
            console.log(`selected ${value}`);
        },
        /**
         * 租户注册图片验证码类型
         */
        CaptchaOnTenantRegistrationTypeChange(value) {
            this.hostSettings.tenantManagement.captchaOnTenantRegistrationType = value;
            console.log(`selected ${value}`);
        },
        /**
         * 保存全部
         */
        handleSubmit(e) {
            e.preventDefault();
            this.form.validateFieldsAndScroll((err, values) => {
                if (!err) {
                    Object.assign(this.hostSettings.email, values);
                    this.spinning = true;
                    this._hostSettingsServiceProxy
                        .updateAllSettings(this.hostSettings)
                        .finally(() => {
                            this.spinning = false;
                        })
                        .then(result => {
                            this.notify.success(this.l("SavedSuccessfully"));
                        });
                }
            });
        },
        /**
         * 发送测试邮箱
         */
        sendTestEmail() {
            this.spinning = true;
            this._hostSettingsServiceProxy
                .sendTestEmail({
                    emailAddress: this.form.getFieldsValue()
                        .testEmailSettingsHeader
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    this.notify.success(this.l("TestEmailSentSuccessfully"));
                });
        }
    }
};
</script>

<style scoped lang="less">
/deep/.ant-form-item-label {
    text-align: left;
}
/deep/.ant-input-group-addon {
    padding: 0 !important;
}
/deep/.ant-input {
    height: 35px;
}
</style>
