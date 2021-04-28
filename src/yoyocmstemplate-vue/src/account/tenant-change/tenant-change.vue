<template>
    <div v-if="isMultiTenancyEnabled" class="card tenant-change-component">
        <div class="body text-center">
            <span>
                {{l("CurrentTenant")}}:
                <span v-if="tenancyName" :title="name">
                    <strong>{{tenancyName}}</strong>
                </span>
                <span v-if="!tenancyName">{{l("NotSelected")}}</span>
                (
                    <a href="javascript:;" @click="showChangeModal()">{{l("Change")}}</a>)
            </span>
        </div>
    </div>
</template>

<script>
    import {AppComponentBase} from "@/shared/component-base";
    import {appSessionService, abpService} from "@/shared/abp";
    import TenantChangeModal from "./tenant-change-modal";
    import {ModalHelper} from '@/shared/helpers';

    export default {
        name: "tenant-change",
        mixins: [AppComponentBase],
        components: {},
        data() {
            return {
                show: false
            };
        },
        computed: {
            tenancyName: vm => {
                if (!appSessionService.tenant) {
                    return undefined;
                }
                return appSessionService.tenant.tenancyName;
            },
            name: vm => {
                return appSessionService.tenant.name;
            },
            isMultiTenancyEnabled: vm => {
                return abpService.abp.multiTenancy.isEnabled;
            }
        },
        methods: {
            showChangeModal() {
                ModalHelper.create(TenantChangeModal, {
                    tenancyName: this.tenancyName
                }).subscribe((res) => {

                });
            }
        }
    };
</script>

<style lang="less" scoped>
    @import "./tenant-change.less";
</style>
