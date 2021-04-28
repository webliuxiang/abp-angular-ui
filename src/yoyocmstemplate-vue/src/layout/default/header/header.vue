<template>
    <div class="ant-layout-header alain-pro__header">
        <!-- 左边 -->
        <div class="d-flex align-items-center">
            <!-- Menu -->
            <div @click="toggleCollapsedSidebar()" class="alain-pro__header-item alain-pro__header-trigger">
                <a-icon type="menu-fold" v-show="!collapsed"/>
                <a-icon type="menu-unfold" v-show="collapsed"/>
            </div>
            <!-- Github Page -->
            <a class="alain-pro__header-item alain-pro__header-trigger"
               href="https://gitee.com/aiabpedu/LTMCompanyName.YoyoCmsTemplate" target="_blank">
                <a-icon type="github"/>
            </a>
            <!-- 52abp官网 -->
            <a class="alain-pro__header-item alain-pro__header-trigger" href="https://www.52abp.com">
                <a-icon type="link"/>
            </a>

        </div>

        <!-- 右边 -->
        <div class="alain-pro__header-right">
            <header-i18n class="alain-pro__header-item"></header-i18n>
            <header-notifications class="alain-pro__header-item mr-sm"></header-notifications>
            <header-settings class="alain-pro__header-item hidden-xs"></header-settings>
            <header-user class="hidden-xs alain-pro__header-item"></header-user>
        </div>
    </div>


</template>

<script>
    import {HeaderI18n, HeaderNotifications, HeaderSettings, HeaderUser} from './components';
    import {layoutService} from '@/shared/common';


    export default {
        name: "layout-header",
        components: {
            HeaderI18n,
            HeaderNotifications,
            HeaderSettings,
            HeaderUser
        },
        data() {
            return {

            }
        },
        computed: {
            collapsed: {
                get() {
                    return layoutService.data.collapsed;
                },
                set(val) {
                    layoutService.data.collapsed = val;
                }
            }
        },
        methods: {
            toggleCollapsedSidebar() {
                this.collapsed = !this.collapsed;
            }
        }
    }
</script>

<style scoped>

</style>
