<template>


    <a-dropdown class="alain-pro__nav-item" placement="bottomRight">
        <a-menu slot="overlay" style="width:200px;">
            <a-menu-item @click="changeTheme()">
                <a-icon type="bg-colors"></a-icon>
                切换主题
            </a-menu-item>
        </a-menu>

        <a-icon type="setting"></a-icon>
    </a-dropdown>

</template>

<script>
    import {layoutService} from '@/shared/common';

    export default {
        name: "header-settings",
        data() {
            return {}
        },
        methods: {
            changeTheme() {
                layoutService.changeTheme();
            }
        }
    }
</script>

<style scoped>

</style>
