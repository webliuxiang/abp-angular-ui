<template>
    <div>
        <div class="modal-header">
            <div class="ant-modal-title">
                {{l('ChangeTenant')}}
            </div>
        </div>

        <div>
            <a-form :form="form" @submit="save()">
                <a-form-item :label="l('TenancyName')" :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }">
                    <a-input v-decorator="['tenancyName']"/>
                    <span class="text-sm text-grey">{{l("LeaveEmptyToSwitchToHost")}}</span>
                </a-form-item>
            </a-form>
        </div>

        <div class="modal-footer">
            <a-button @click="close()">
                <i class="anticon anticon-close-circle-o"></i>
                {{l("Cancel")}}
            </a-button>
            <a-button @click="save()" type="primary">
                <i class="anticon anticon-save"></i>
                {{l("Save")}}
            </a-button>
        </div>

    </div>
</template>

<script>
    import {ModalComponentBase} from "@/shared/component-base";
    import {AccountServiceProxy, IsTenantAvailableInput} from "@/shared/service-proxies";
    import {AppTenantAvailabilityState} from "@/abpPro/AppEnums";
    import {abpService} from "@/shared/abp";
    import {Modal} from "ant-design-vue";

    export default {
        name: "tenant-change-modal",
        mixins: [ModalComponentBase],
        data() {
            return {
                isSaving: false,
                form: this.$form.createForm(this, {name: "coordinated"}),
                accountService: undefined,
                tenancyName: undefined
            };
        },
        computed: {
            saving: {
                set(val) {
                    this.isSaving = val;
                },
                get() {
                    return this.isSaving;
                }
            }
        },
        created() {
            this.fullData(); // 模态框必须,填充数据到data字段
            this.accountService = new AccountServiceProxy(this.$apiUrl, this.$api);
        },
        mounted() {
        },
        watch: {
            tenancyName(val) {
                if (val) {
                    this.form.setFieldsValue({tenancyName: val});
                }
            }
        },
        methods: {
            save() {
                this.saving = true;

                let newTenancyName = this.form.getFieldValue("tenancyName");

                if (newTenancyName === this.tenancyName) {
                    this.close();
                    return;
                }
                if (!newTenancyName || newTenancyName === "") {
                    abpService.abp.multiTenancy.setTenantIdCookie(undefined);
                    this.close();
                    location.reload();
                    return;
                }
                const input = new IsTenantAvailableInput();
                input.tenancyName = newTenancyName;
                this.accountService
                    .isTenantAvailable(input)
                    .finally(() => {
                        this.saving = false;
                    })
                    .then(
                        result => {
                            switch (result.state) {
                                case AppTenantAvailabilityState.Available:
                                    abpService.abp.multiTenancy.setTenantIdCookie(result.tenantId);
                                    this.success();
                                    window.location.reload();
                                    return;
                                case AppTenantAvailabilityState.InActive:
                                    Modal.warning({
                                        message: this.l("TenantIsNotActive", input.tenancyName)
                                    });
                                    // this.message.warn(
                                    //   this.l("TenantIsNotActive", this.tenancyName)
                                    // );
                                    break;
                                case AppTenantAvailabilityState.NotFound: // NotFound
                                    Modal.warning({
                                        content: this.l(
                                            "ThereIsNoTenantDefinedWithName{0}",
                                            input.tenancyName
                                        )
                                    });
                                    // this.message.warn(
                                    //   this.l("ThereIsNoTenantDefinedWithName{0}", this.tenancyName)
                                    // );
                                    break;
                            }
                        },
                        error => {
                            console.error(error);
                        }
                    );
            }
        }
    };
</script>

<style lang="less" scoped>
</style>
