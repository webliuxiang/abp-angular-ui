<template>
  <a-spin :spinning="spinning">
    <div class="modal-header">
      <div class="modal-title">{{l('CreateNewTenant')}}</div>
    </div>
    <a-form :form="form" @submit="handleSubmit">
      <!-- 租户名称 -->
      <a-form-item :label="l('TenancyName')">
        <a-input
          :placeholder="l('TenancyName')"
          v-decorator="[
                        'tenancyName',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                }
                            ],
                        },
                        ]"
        />
      </a-form-item>

      <!-- 租户显示名称 -->
      <a-form-item :label="l('DisplayTenancyName')">
        <a-input
          :placeholder="l('Name')"
          v-decorator="[
                        'name',
                        {
                            rules: [
                                {
                                    required: true,
                                message: l('ThisFieldIsRequired'),
                                }
                            ],
                        },
                        ]"
        />
      </a-form-item>

      <!-- UseHostDatabase -->
      <a-checkbox
        @change="UseHostDatabaseChange"
        :checked="UseHostDatabasechecked"
      >{{l("UseHostDatabase")}}</a-checkbox>
      <a-form-item :label="l('DatabaseConnectionString')" v-if="!UseHostDatabasechecked">
        <a-input
          :placeholder="l('DatabaseConnectionString')+l('Optional')"
          v-decorator="[
                        'connectionString',
                        {
                            rules: [
                                {
                                    required: true,
                                message: l('ThisFieldIsRequired'),
                                }
                            ],
                        },
                        ]"
        />
      </a-form-item>

      <!-- 管理员邮箱 -->
      <a-form-item :label="l('AdminEmailAddress')">
        <a-input
          :placeholder="l('EmailAddress')"
          v-decorator="[
                        'adminEmailAddress',
                        {
                            rules: [
                                {
                                    type: 'email',
                                    message: l('NotEmail'),
                                },
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                },
                            ],
                        },
                        ]"
        />
      </a-form-item>

      <!-- 密码 -->
      <a-checkbox
        @change="passwordChange"
        :checked="passwordchecked"
      >{{l("UseDefaultPassword","123qwe")}}</a-checkbox>
      <a-form-item :label="l('Password')" v-if="!passwordchecked" has-feedback>
        <a-input
          :placeholder="l('Password')"
          type="password"
          v-decorator="[
                        'adminPassword',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                },
                                {
                                    max: 32,
                                    message: l('maxlength'),
                                },
                                {
                                    min: 6,
                                    message: l('minlength'),
                                },
                                {
                                    validator: validateToNextPassword,
                                },
                            ],
                        },
                        ]"
        />
      </a-form-item>
      <a-form-item :label="l('ConfirmPassword')" v-if="!passwordchecked" has-feedback>
        <a-input
          :placeholder="l('ConfirmPassword')"
          type="password"
          v-decorator="[
                        'confirmPassword',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                },
                                {
                                    validator: validateEqual,
                                },
                            ],
                        },
                        ]"
        />
      </a-form-item>

      <!-- 版本 -->
      <a-form-item :label="l('Edition')" has-feedback>
        <edition-combo
          :placeholder="l('PleaseSelect')"
          :selectedEdition="selectedEdition"
          @selectedEditionChange="selectedEditionChange($event)"
        ></edition-combo>
      </a-form-item>

      <!-- IsUnlimited -->
      <a-checkbox
        @change="checkedChange('IsUnlimitedchecked', $event)"
        v-if="isShowIsUnlimited"
        :checked="IsUnlimitedchecked"
      >{{l("IsUnlimited")}}</a-checkbox>
      <a-form-item
        :label="l('SubscriptionEndDateUtc')"
        v-if="!IsUnlimitedchecked && isShowIsUnlimited"
      >
        <a-date-picker
          @change="subscriptionEndDateUtconChange"
          placeholder="请选择日期"
          v-decorator="[
                        'subscriptionEndUtc',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                }
                            ],
                        },
                        ]"
        />
      </a-form-item>
      <br />

      <!-- 是否试用 -->
      <a-checkbox
        @change="checkedChange('isInTrialPeriod', $event)"
        :disabled="IsInTrialPerioddisabled"
        :checked="isInTrialPeriod"
      >{{l("IsInTrialPeriod")}}</a-checkbox>
      <br />

      <!-- 下次登录需要修改密码 -->
      <a-checkbox
        @change="checkedChange('shouldChangePasswordOnNextLogin', $event)"
        :checked="shouldChangePasswordOnNextLogin"
      >{{l("ShouldChangePasswordOnNextLogin")}}</a-checkbox>
      <br />

      <!-- 发送激活邮件 -->
      <a-checkbox
        @change="checkedChange('sendActivationEmail', $event)"
        :checked="sendActivationEmail"
      >{{l("SendActivationEmail")}}</a-checkbox>
      <br />

      <!-- 是否激活 -->
      <a-checkbox @change="checkedChange('isActive', $event)" :checked="isActive">{{l("IsActive")}}</a-checkbox>

      <a-form-item class="btn--container">
        <a-button type="button" @click="close()">{{ l("Cancel") }}</a-button>
        <a-button type="primary" html-type="submit">{{ l('Save') }}</a-button>
      </a-form-item>
    </a-form>
  </a-spin>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
import EditionCombo from "../../shared/edition-combo/edition-combo.vue";
import {
  TenantServiceProxy,
  TenantListDto,
  SubscribableEditionComboboxItemDto,
  EntityDtoOfInt64,
  NameValueDto,
  CommonLookupServiceProxy,
  CommonLookupFindUsersInput
} from "@/shared/service-proxies";

export default {
  name: "create-tenant-component",
  mixins: [ModalComponentBase],
  components: {
    EditionCombo
  },
  data() {
    return {
      confirmDirty: false,
      autoCompleteResult: [],
      UseHostDatabasechecked: true,
      passwordchecked: true,
      // 版本
      selectedEdition: {
        value: 0
      },
      // 是否在试用期
      isInTrialPeriod: false,
      IsInTrialPerioddisabled: false,
      // 下次登录需要修改密码
      shouldChangePasswordOnNextLogin: false,
      // 发送激活邮件
      sendActivationEmail: false,
      // 是否激活
      isActive: true,
      IsUnlimitedchecked: false,
      // IsUnlimited
      isShowIsUnlimited: false,
      // 版本号
      editionId: 0,
      spinning: false
    };
  },
  beforeCreate() {
    this.form = this.$form.createForm(this, { name: "register" });
  },
  created() {
    this.fullData(); // 模态框必须,填充数据到data字段
  },
  methods: {
    /**
     * UseHostDatabaseChange
     */
    UseHostDatabaseChange(e) {
      this.UseHostDatabasechecked = eval(`${e.target.checked}`.toLowerCase());
    },
    /**
     * password
     */
    passwordChange(e) {
      this.passwordchecked = eval(`${e.target.checked}`.toLowerCase());
    },
    /**
     * 验证密码
     */
    validateToNextPassword(rule, value, callback) {
      const form = this.form;
      if (value && this.confirmDirty) {
        form.validateFields(["confirm"], { force: true });
      }
      callback();
    },
    validateEqual(rule, value, callback) {
      const form = this.form;
      if (value && value !== form.getFieldValue("adminPassword")) {
        callback(this.l("PasswordInconsistent"));
      } else {
        callback();
      }
    },
    /**
     * 版本选择
     */
    selectedEditionChange(e) {
      if (e && e.value) {
        this.IsInTrialPerioddisabled = true;
        this.isShowIsUnlimited = true;
        this.editionId = parseInt(e.value);
      } else {
        this.IsInTrialPerioddisabled = false;
        this.isShowIsUnlimited = false;
        this.editionId = null;
      }
    },
    /**
     * 是否试用
     * 下次登录需要修改密码
     * 发送激活邮
     * 是否激活
     */
    checkedChange(type, e) {
      this[`${type}`] = eval(`${e.target.checked}`.toLowerCase());
    },
    /**
     * 订阅结束日期
     */
    subscriptionEndDateUtconChange(date, dateString) {
      console.log(date, dateString);
    },
    /**
     * 提交表单
     */
    handleSubmit(e) {
      e.preventDefault();
      this.form.validateFieldsAndScroll((err, values) => {
        if (!err) {
          let otherobj = {
            shouldChangePasswordOnNextLogin: this
              .shouldChangePasswordOnNextLogin,
            sendActivationEmail: this.sendActivationEmail,
            isInTrialPeriod: this.isInTrialPeriod,
            isActive: this.isActive,
            editionId: this.editionId
          };
          values = Object.assign(values, otherobj);
          values.subscriptionEndUtc =
            !this.editionId || this.IsUnlimitedchecked
              ? null
              : values.subscriptionEndUtc;
          this.spinning = true;
          this._tenantService = new TenantServiceProxy(this.$apiUrl, this.$api);
          this._tenantService
            .create(values)
            .finally(() => {
              this.spinning = false;
            })
            .then(res => {
              // this.notify.success(this.l('SavedSuccessfully'));
              this.$notification["success"]({
                message: this.l("SavedSuccessfully")
              });
              this.success();
            })
            .catch(err => {
              console.log(err);
            });
        }
      });
    }
  }
};
</script>

<style lang="less" scoped>
/deep/.btn--container .ant-form-item-children {
  display: block;
  margin: 10px auto;
  text-align: center;
}
</style>