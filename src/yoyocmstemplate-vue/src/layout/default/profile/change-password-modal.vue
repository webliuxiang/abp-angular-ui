<template>
  <div>
    <div class="modal-header">
      <div class="modal-title">{{l('ChangePassword')}}</div>
    </div>

    <a-form :form="form" @submit="save">
      <!-- 当前密码 -->
        
      <a-form-item :label="l('CurrentPassword')" :label-col="{ span: 6 }"  has-feedback :wrapper-col="{ span: 14 }">
        <a-input
          type="password"
          v-decorator="['currentPassword', { rules: [
            { required: true, message: l('ThisFieldIsRequired') },
          ] }]"
        />
      </a-form-item>
      <!-- 新密码 -->
      <a-form-item
        :label="l('NewPassword')"
        :label-col="{ span: 6 }"
         has-feedback
        :wrapper-col="{ span: 14 }"
      >
        <a-input
          type="password"
          v-decorator="['newPassword', { rules: [
            { required: true, message: l('MinLength'),min:6 },
            { required: true, message: l('MaxLength'),max:32 },
          ] }]"
        />
      </a-form-item>
      <!-- 确认新密码 -->
      <a-form-item
         has-feedback
        :label="l('ConfirmPassword')"
        :label-col="{ span: 6 }"
        :wrapper-col="{ span: 14 }"
      >
        <a-input
          type="password"
          v-decorator="['confirmNewPasswordStr', { rules: [{ required: true, validator: confirmNewPassword, }] }]"
        />
      </a-form-item>

      <!-- 功能按钮 -->
      <div class="modal-footer">
        <a-button :disabled="saving" @click="close()" nz-button type="button">{{l("Cancel")}}</a-button>
        <a-button html-type="submit" :loading="saveLoading" type="primary">{{l("Save")}}</a-button>
      </div>
    </a-form>
  </div>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
import {
  ChangePasswordInput,
  ProfileServiceProxy
} from "../../../shared/service-proxies";

export default {
  name: "change-password-modal",
  mixins: [ModalComponentBase],
  data() {
    return {
      form: this.$form.createForm(this),
      profileService: null,
      confirmNewPasswordVal: null,
      inputVal: null,
      saveLoading:false,
    };
  },
  created() {
    this.profileService = new ProfileServiceProxy(this.$apiUrl, this.$api);
    this.inputVal = new ChangePasswordInput();
  },
  computed: {},
  methods: {
    confirmNewPassword(rule, value, callback) {
      const form = this.form;
      if (value && value !== form.getFieldValue("newPassword")) {
        callback(this.l("PasswordsDontMatch"));
      } else {
        callback();
      }
    },
    save(e) {
      e.preventDefault();
      this.form.validateFields((err, values) => {
        if (!err) {
          this.saveLoading = true;
          this.inputVal.currentPassword=values.currentPassword;
          this.inputVal.newPassword=values.newPassword;
          this.profileService
            .changePassword(this.inputVal)
            .finally(() => {
              this.saveLoading = false;
            })
            .then(() => {
              this.saveLoading = false;
              this.notify.success(this.l("YourPasswordHasChangedSuccessfully"));
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
