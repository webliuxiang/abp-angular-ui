<template>
    <div>
        <div class="modal-header">
            <div class="modal-title">{{l('MySettings')}}</div>
        </div>
        <a-form :form="form" @submit="save">
            <!-- 用户名(登陆使用) -->
            <a-form-item
                :label="l('UserName')"
                :label-col="{ span: 6 }"
                has-feedback
                :wrapper-col="{ span: 14 }">
                <a-input
                    type="text"
                    :disabled="isAdmin"
                    v-decorator="['userName', { rules: [
            { required: true, message: l('ThisFieldIsRequired') },
          ] }]" />
            </a-form-item>

            <!-- 手机号码 -->
            <a-form-item
                :label="l('PhoneNumber')"
                :label-col="{ span: 6 }"
                has-feedback
                :wrapper-col="{ span: 14 }">
                <a-input
                    :placeholder="l('PhoneNumber')"
                    type="text"
                    v-decorator="['phoneNumber', { rules: [{ required: false},] }]" />
            </a-form-item>
            <!-- 邮箱地址 -->
            <a-form-item
                :label="l('EmailAddress')"
                :label-col="{ span: 6 }"
                has-feedback
                :wrapper-col="{ span: 14 }">
                <a-input
                    type="email"
                    :placeholder="l('EmailAddress')"
                    v-decorator="['emailAddress', { rules: [
            { required: true, message: l('ThisFieldIsRequired') },
            { required: true, validator: validationEmail, }
          ] }]" />
            </a-form-item>
            <!-- 功能按钮 -->
            <div class="modal-footer">
                <a-button :disabled="saving" @click="close()" nz-button type="button">{{l("Cancel")}}</a-button>
                <a-button html-type="submit" type="primary" :loading="saveLoading">{{l("Save")}}</a-button>
            </div>
        </a-form>
    </div>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
import { appSessionService } from "@/shared/abp/";
import { AppConsts } from "@/abpPro/AppConsts";
import pick from "lodash.pick";
import {
    CurrentUserProfileEditDto,
    ProfileServiceProxy
} from "../../../shared/service-proxies";
export default {
    name: "my-settings-modal",
    mixins: [ModalComponentBase],
    data() {
        return {
            saveLoading: false,
            form: this.$form.createForm(this),
            isAdmin: true,
            profileService: null,
            appSession: null
        };
    },
    created() {
        this.profileService = new ProfileServiceProxy(this.$apiUrl, this.$api);
        this.profileService.getCurrentUserProfileForEdit().then(result => {
            this.user = result;
            this.isAdmin =
                this.user.userName ===
                AppConsts.userManagement.defaultAdminUserName;
            this.$nextTick(() => {
                this.form.setFieldsValue(
                    pick(result, "userName", "phoneNumber", "emailAddress")
                );
            });
        });
    },
    methods: {
        validationEmail(rule, value, callback) {
            const form = this.form;
            var regEmail = /^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;
            if (value && !regEmail.test(value)) {
                callback(this.l("NotEmail"));
            } else {
                callback();
            }
        },
        save(e) {
            e.preventDefault();
            this.form.validateFields((err, values) => {
                if (!err) {
                    this.saveLoading = true;
                    this.user.userName = values.userName;
                    this.user.phoneNumber = values.phoneNumber;
                    this.user.emailAddress = values.emailAddress;
                    this.profileService
                        .updateCurrentUserProfile(this.user)
                        .finally(() => {
                            this.saving = false;
                        })
                        .then(() => {
                            this.saving = false;
                            appSessionService.user.userName = this.user.userName;
                            appSessionService.user.emailAddress = this.user.emailAddress;
                            this.notify.success(this.l("SavedSuccessfully"));
                            this.success();
                        });
                }
            });
        }
    }
};
</script>

<style scoped>
</style>
