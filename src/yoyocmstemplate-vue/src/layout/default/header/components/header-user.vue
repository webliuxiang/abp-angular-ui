<template>

    <a-dropdown class="alain-pro__nav-item" placement="bottomRight">
        <a-menu slot="overlay" style="width:200px;">
            <a-menu-item @click="backToMyAccount()" v-if="isImpersonatedLogin">
                <i class="anticon anticon-rollback mr-sm"></i>
                {{ l('BackToMyAccount') }}
            </a-menu-item>
            <a-menu-item @click="changePassword()">
                <i class="anticon anticon-ellipsis mr-sm"></i>
                {{ l('ChangePassword') }}
            </a-menu-item>
            <a-menu-item @click="showLoginAttempts()">
                <i class="anticon anticon-bars mr-sm"></i>
                {{ l('LoginAttempts') }}
            </a-menu-item>
            <a-menu-item @click="changeMySettings()">
                <i class="anticon anticon-setting mr-sm"></i>
                {{ l('MySettings') }}
            </a-menu-item>
            <a-divider/>
            <a-menu-item @click="logout()">
                <i class="anticon anticon-logout mr-sm"></i>
                {{ l('Logout') }}
            </a-menu-item>
        </a-menu>


        <div class="alain-pro__nav-item d-flex align-items-center px-sm">
            <a-avatar class="mr-sm" size="small"
                      src="/assets/images/user.png">
            </a-avatar>
            {{ loginUserName }}
        </div>
    </a-dropdown>

</template>

<script>
    import {AppComponentBase} from '@/shared/component-base';
    import {appAuthService, appSessionService} from "@/shared/abp";
    import {ChangePasswordModal, LoginAttemptsModal, MySettingsModal} from '../../profile';
    import {ModalHelper} from '@/shared/helpers';

    export default {
        name: "header-user",
        mixins: [AppComponentBase],
        data() {
            return {
                showNotificationSettings: false
            }
        },
        computed: {
            isImpersonatedLogin() {
                return appSessionService.user.impersonatorUserId > 0;
            },
            loginUserName() {
                return appSessionService.getShownLoginName();
            },
        },
        methods: {
            changePassword() {
                ModalHelper.create(ChangePasswordModal).subscribe((result) => {
                    if (result) {
                            this.logout();
                        }
                });
            },
            showLoginAttempts() {
                ModalHelper.create(LoginAttemptsModal).subscribe(() => {

                });
            },
            changeMySettings() {
                ModalHelper.create(MySettingsModal).subscribe(() => {

                });
            },
            backToMyAccount() {

            },
            logout() {
                appAuthService.logout();
            }
        }
    }
</script>

<style lang="less" scoped>
    .ant-divider-horizontal {
        margin: 4px 0px;
    }
</style>
