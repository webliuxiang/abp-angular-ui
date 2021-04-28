<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">{{l('EditTenant')}} <span v-if="entity">:{{entity.tenancyName}}</span></div>
        </div>
        <a-form :form="form" @submit="handleSubmit">
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
                        ]" />
            </a-form-item>

            <!-- UseHostDatabase -->
            <a-form-item :label="l('DatabaseConnectionString')" v-if="entity.connectionString">
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
                        ]" />
            </a-form-item>

            <!-- 版本 -->
            <a-form-item :label="l('Edition')" has-feedback v-if="isShowEdition">
                <edition-combo
                    :placeholder="l('PleaseSelect')"
                    :selectedEdition="selectedEdition"
                    @selectedEditionChange="selectedEditionChange($event)"></edition-combo>
            </a-form-item>

            <!-- IsUnlimited -->
            <a-checkbox @change="checkedChange('IsUnlimitedchecked', $event)"
                v-if="isShowIsUnlimited" :checked="IsUnlimitedchecked">{{l("IsUnlimited")}}</a-checkbox>
            <a-form-item :label="l('SubscriptionEndDateUtc')" v-if="!IsUnlimitedchecked && isShowIsUnlimited">
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
                        ]" />
            </a-form-item>
            <br>

            <!-- 是否试用 -->
            <a-checkbox @change="checkedChange('isInTrialPeriod', $event)"
                :disabled="IsInTrialPerioddisabled"
                :checked="isInTrialPeriod">{{l("IsInTrialPeriod")}}</a-checkbox>
            <br>

            <!-- 是否激活 -->
            <a-checkbox @change="checkedChange('isActive', $event)" :checked="isActive">{{l("IsActive")}}</a-checkbox>

            <a-form-item class="btn--container">
                <a-button type="button" @click="close()">
                    {{ l("Cancel") }}
                </a-button>
                <a-button type="primary" html-type="submit">
                    {{ l('Save') }}
                </a-button>
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
import pick from "lodash.pick";

export default {
    mixins: [ModalComponentBase],
    name: "create-tenant-component",
    data() {
        return {
            confirmDirty: false,
            // 版本
            selectedEdition: {},
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
            spinning: false,
            isShowEdition: false,
            // 获取到的数据
            entity: {},
            entityId: 0
        };
    },
    components: {
        EditionCombo
    },
    // watch: {
    //     entityId(val) {
    //         if (val) {
    //             console.log(val);
    //         }
    //     }
    // },
    beforeCreate() {
        this.form = this.$form.createForm(this, { name: "register" });
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
    },
    mounted() {
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._tenantService = new TenantServiceProxy(
                this.$apiUrl,
                this.$api
            );
            this._tenantService
                .getForEdit(this.entityId)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    console.log(res);
                    this.entity = res;
                    this.selectedEdition.value = this.entity.editionId ? this.entity.editionId : 0;
                    setTimeout(() => {
                        this.form.setFieldsValue(
                            pick(
                                this.entity,
                                "name",
                                "connectionString",
                                "subscriptionEndUtc"
                            )
                        );
                        this.IsUnlimitedchecked = !this.entity
                            .subscriptionEndUtc;
                        this.isActive = this.entity.isActive;
                    }, 500);
                    this.isShowEdition = true;
                });
        },
        /**
         * 验证密码
         */
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
                        isInTrialPeriod: this.isInTrialPeriod,
                        isActive: this.isActive,
                        editionId: this.editionId,
                        tenancyName: this.entity.tenancyName,
                        id: this.entityId
                    };
                    values = Object.assign(values, otherobj);
                    values.subscriptionEndUtc =
                        !this.editionId || this.IsUnlimitedchecked
                            ? null
                            : values.subscriptionEndUtc;
                    this.spinning = true;
                    this._tenantService = new TenantServiceProxy(
                        this.$apiUrl,
                        this.$api
                    );
                    this._tenantService
                        .update(values)
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