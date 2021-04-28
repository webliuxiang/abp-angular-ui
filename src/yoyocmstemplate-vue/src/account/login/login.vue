<template>
    <div id="login">
        <a-spin :spinning="submitting" :tip="l('LogIningWithThreeDot')">
            <a-card>
                <div slot="title">
                    <div class="text-center yoyo__block">
                        <i class="anticon anticon-login"></i>
                        <span>{{l("LogIn")}}</span>
                    </div>
                </div>

                <a-form :form="form" @submit="login($event)">
                    <a-form-item :help="userNameError() || ''" :validate-status="userNameError() ? 'error' : ''">

                        <a-input :placeholder="l('UserNameOrEmail')"
                                 @blur="userNameOrEmailAddressOnBlur()"
                                 v-decorator="['userNameOrEmailAddress',{
                                 rules: [
                                    { required: true, message: l('ThisFieldIsRequired')}
                                    ]
                                 }]"
                        >
                            <i class="anticon anticon-user" slot="prefix"></i>
                        </a-input>
                    </a-form-item>
                    <a-form-item :help="passwordError() || ''" :validate-status="passwordError() ? 'error' : ''">
                        <a-input
                                :placeholder="l('Password')"
                                type="password"
                                v-decorator="['password',{
                                rules: [
                                    { required: true, message: l('ThisFieldIsRequired') }
                                    ]
                                }]"
                        >
                            <i class="anticon anticon-lock" slot="prefix"></i>
                        </a-input>
                    </a-form-item>
                    <a-form-item v-if="useCaptcha">
                        <captcha ref="captcha"
                                 :placeholder="l('CAPTCHA')"
                                 :primaryKey="primaryKey"
                                 :type="captchaType"
                                 v-decorator="['verificationCode',{
                                    rules: [
                                        { required: true, message: l('ThisFieldIsRequired') }
                                    ]
                                    }]"></captcha>
                    </a-form-item>
                    <a-form-item>
                        <a-button :disabled="hasErrors(form.getFieldsError())"
                                  class="yoyo__block"
                                  html-type="submit"
                                  type="primary">
                            {{l("LogIn")}}
                        </a-button>
                    </a-form-item>
                </a-form>


            </a-card>
        </a-spin>
    </div>
</template>

<script>
    import {TokenAuthServiceProxy} from "@/shared/service-proxies";
    import {AppComponentBase} from "@/shared/component-base";
    import {Captcha} from '../components';
    import AFormItem from "ant-design-vue/es/form/FormItem";
    import {abpService, appSessionService} from '@/shared/abp';
    import {AppCaptchaType} from '@/abpPro/AppEnums';
    import {loginService} from './login.service';


    function hasErrors(fieldsError) {
        return Object.keys(fieldsError).some(field => fieldsError[field]);
    }

    export default {
        components: {
            AFormItem,
            Captcha
        },
        mixins: [AppComponentBase],
        data() {
            return {
                hasErrors,
                isSubmitting: false, // 提交中
                tmpPrimaryKey: undefined,
                verificationImgUrl: undefined, // 验证码地址
                form: this.$form.createForm(this),
                tokenAuthService: TokenAuthServiceProxy,
            };
        },
        computed: {
            primaryKey() { // 验证码主键
                return this.tmpPrimaryKey;
            },
            submitting() { // 提交中
                return this.isSubmitting;
            },
            externalLoginProviders() { // 第三方登陆提供者
                return loginService.externalLoginProviders;
            },
            multiTenancySideIsTeanant() { // 是否已选中租户
                return abpService.tenantId > 0;
            },
            isTenantSelfRegistrationAllowed() { // 是否允许注册租户
                if (abpService.tenantId) {
                    return false;
                }
                return abpService.abp.setting.getBoolean('App.Host.AllowSelfRegistration');
            },
            isMultiTenancyEnabled() { // 是否启用多租户
                return abpService.abp.multiTenancy.isEnabled;
            },
            isSelfRegistrationAllowed() { // 是否允许注册
                if (!this.appSessionService.tenantId) {
                    return false;
                }
                return abpService.abp.setting.getBoolean('App.AllowSelfRegistrationUser');
            },
            useCaptcha() { // 是否使用登陆验证码
                return abpService.abp.setting.getBoolean('App.UseCaptchaOnUserLogin');
            },
            captchaType() { // 验证码类型
                if (appSessionService.tenantId) {
                    return AppCaptchaType.TenantUserLogin;
                } else {
                    return AppCaptchaType.HostUserLogin;
                }
            },
            captchaLength() { // 验证码长度
                return abpService.abp.setting.getInt('App.CaptchaOnUserLoginLength');
            },
            enabledExternalLoginTypes() { // 第三方登陆的类型
                return JSON.parse(
                    abpService.abp.setting.get('App.UserManagement.ExternalLoginProviders')
                );
            }
        },
        created() {
            this.tokenAuthService = new TokenAuthServiceProxy(this.$apiUrl, this.$api);
            loginService.initExternalLoginProviders()
                .then(() => {

                });

            if (appSessionService.userId > 0 && loginService.getReturnUrl()) {
                appSessionService.session
                    .updateUserSignInToken()
                    .subscribe((result) => {
                        const initialReturnUrl = loginService.getReturnUrl();
                        const returnUrl =
                            initialReturnUrl +
                            (initialReturnUrl.indexOf('?') >= 0 ? '&' : '?') +
                            'accessToken=' +
                            result.signInToken +
                            '&userId=' +
                            result.encodedUserId +
                            '&tenantId=' +
                            result.encodedTenantId;

                        location.href = returnUrl;
                    });
            }
        },
        mounted() {

        },
        watch: {},
        methods: {
            // handler
            handleUsernameOrEmail(rule, value, callback) {
                const {state} = this;
                const regex = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/;
                if (regex.test(value)) {
                    state.loginType = 0;
                } else {
                    state.loginType = 1;
                }
                callback();
            },
            // Only show error after a field is touched.
            userNameError() {
                const {getFieldError, isFieldTouched} = this.form;
                return isFieldTouched('userNameOrEmailAddress') && getFieldError('userNameOrEmailAddress');
            },
            // Only show error after a field is touched.
            passwordError() {
                const {getFieldError, isFieldTouched} = this.form;
                return isFieldTouched('password') && getFieldError('password');
            },
            userNameOrEmailAddressOnBlur() {
                this.tmpPrimaryKey = this.form.getFieldValue('userNameOrEmailAddress');
            },
            login(e) {
                e.preventDefault();

                loginService.authenticateModel.userNameOrEmailAddress = this.form.getFieldValue('userNameOrEmailAddress');
                loginService.authenticateModel.password = this.form.getFieldValue('password');
                loginService.authenticateModel.verificationCode = this.form.getFieldValue('verificationCode');
                loginService.authenticateModel.rememberClient = this.form.getFieldValue('rememberClient');

                loginService.authenticate(undefined, () => {
                    if (this.$refs.captcha) {
                        this.$refs.captcha.clearImg();
                    }
                }, () => {
                    this.isSubmitting = false;
                });

            },
            loginSuccess(res) {
                /**
                 * TODO: 这里跳转会进入err函数,因为初始化 $router 时没有加入 path 为 / 的路由,跳转无法匹配到
                 * 但是会在 permission.js 中根据url redirect 信息重定向到正常页面,所以这里认为是正常的跳转
                 */
                // this.$router.goto(
                //     {path: "/"},
                //     () => {
                //     },
                //     err => {
                //         this.$notification.success({
                //             message: "欢迎",
                //             description: `${timeFix()}，欢迎回来`
                //         });
                //     }
                // );
                // this.isLoginError = false;
            }
        }
    };
</script>

<style lang="less" scoped>
    @import "./login.less";
</style>
