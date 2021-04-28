<template>
    <div :class="[themeClass]">
        <div :class="{'alain-pro__collapsed':collapsed}" class="ant-layout ant-layout-has-sider">

            <a-layout-sider :collapsed="collapsed"
                            :width="256"
                            class="alain-pro__sider alain-pro__sider-fixed">

                <sidebar class="alain-pro__aside"></sidebar>

            </a-layout-sider>


            <div class="ant-layout ant-pro-content">

                <layout-header></layout-header>


                <a-layout-content class="layout-window">
                    <reuse-tab class="layout-tab" v-if="reuseTab"></reuse-tab>

                    <a-layout-content class="layout-content">
                        <router-view/>
                    </a-layout-content>
                </a-layout-content>

            </div>

        </div>
    </div>
</template>

<script>
    import {ReuseTab} from '@/components'
    import {LayoutHeader} from "./header";
    import {Sidebar} from './sidebar';
    import {layoutService} from '@/shared/common';
    import {SignalRAspNetCoreHelper} from '@/shared/helpers';
    import {abpSignalrService} from '@/shared/auth';
    import {abpService} from '@/shared/abp';

    export default {
        name: "layout-default",
        components: {
            LayoutHeader,
            ReuseTab,
            Sidebar
        },
        data() {
            return {}
        },
        computed: {
            collapsed() {
                return layoutService.data.collapsed;
            },
            theme() {
                return layoutService.data.theme;
            },
            themeClass() {
                return `alain-pro__${this.theme}`;
            },
            reuseTab() {
                return layoutService.data.reuseTab;
            }
        },
        mounted() {
            SignalRAspNetCoreHelper.initSignalR(() => {
                if (abpService.loginInfo.user) {
                    abpSignalrService.init();
                }
            });
        },
    }
</script>

<style lang="less" scoped>
    @import "./layout-default.less";
</style>
