<template>
    <div class="container">
        <div class="text-center">
            <!-- logo -->
            <div>
                <a>
                    <img class="logo" src="/assets/images/logos/logo-txt-color-shield.svg"/>
                </a>
            </div>

            <!-- 标题信息 -->
            <div class="account-slogon">
                <h2 class="account-slogon-main">欢迎使用YoyoCmsTemplate</h2>
                <p class="account-slogon-subhead">
          <span>
            学 .NET CORE 及ABP框架、DDD就上
            <a
                    href="http://www.52abp.com/"
                    title="52abp官网网址"
                    target="_blank"
            >52ABP.COM</a>
          </span>
                </p>
            </div>
        </div>

        <!-- 租户切换 -->
        <div v-if="showTenantChange">
            <!-- *ngIf="showTenantChange()" -->
            <tenant-change></tenant-change>
        </div>

        <router-view/>

        <!-- 语言选择 -->
        <div class="text-center">{{l('LanguageSelection')}}</div>
        <account-languages></account-languages>

        <!-- 底部信息 -->
        <div class="global-footer">
            <global-footer :links="links">
                Copyright
                <i class="anticon anticon-copyright"></i>
                {{currentYear}}
                <b>Version</b>
                {{versionText}}
                <a href="http://www.52abp.com/" target="_blank">52ABP.COM</a>出品
            </global-footer>
        </div>
    </div>
</template>

<script>
    import {GlobalFooter} from "@/components";
    import {AccountLanguages} from "./account-languages";
    import {TenantChange} from "./tenant-change";

    import {AppComponentBase} from "@/shared/component-base";
    import {abpService, appSessionService} from "@/shared";

    export default {
        name: "account-layout",
        mixins: [AppComponentBase],
        components: {
            GlobalFooter,
            AccountLanguages,
            TenantChange
        },
        data() {
            return {
                links: [
                    {
                        title: "ABP",
                        href: ""
                    },
                    {
                        title: "隐私",
                        href: ""
                    },
                    {
                        title: "条款",
                        href: ""
                    }
                ]
            };
        },
        computed: {
            currentYear: vm => {
                return new Date().getFullYear();
            },
            versionText: vm => {
                return appSessionService.application.version;
            },
            showTenantChange: vm => {
                return abpService.abp.multiTenancy.isEnabled;
            }
        }
    };
</script>

<style scoped>
    @import "./account-layout.less";
</style>
