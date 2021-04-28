<template>
    <div class="ant-layout-sider-children">
        <!-- logo -->
        <div class="alain-pro__sider-logo">
            <router-link class="alain-pro__header-logo-link" to="/">
                <img class="alain-pro__header-logo-expanded"
                     src="/assets/images/logos/logo-txt-white-shield.svg"

                     style="max-height:40px;"/>
                <img class="alain-pro__header-logo-collapsed"
                     src="/assets/images/logos/logo-white-shield.svg"
                     style="max-height:30px;"/>
            </router-link>
        </div>

        <!-- sidebar -->
        <yoyo-sidebar-nav></yoyo-sidebar-nav>
    </div>
</template>

<script>
    import {YoyoSidebarNav} from './components';

    export default {
        name: "sidebar",
        components: {
            YoyoSidebarNav
        },

    }
</script>

<style lang="less" scoped>
    @import "./sidebar.less";
</style>
