<template>
    <a-dropdown class="alain-pro__item">
        <a class="ant-dropdown-link" href="#">
            <a-icon type="global"/>
            {{ currentLanguage.displayName }}
            <a-icon type="down"/>
        </a>
        <a-menu slot="overlay">
            <a-menu-item :key="index" v-for="(item,key,index) in languages" @click="change(item.name)">
                <i class="anticon" v-bind:class="[item.icon]"></i>
                <span>{{ item.displayName }}</span>
            </a-menu-item>
        </a-menu>
    </a-dropdown>
</template>

<script>
    import {abpService} from "@/shared/abp";
    import {ChangeUserLanguageDto, ProfileServiceProxy} from "@/shared/service-proxies";
    import {AppComponentBase} from "@/shared/component-base";

    export default {
        name: "header-i18n",
        mixins: [AppComponentBase],
        data() {
            return {
                profileService: null
            }
        },
        created() {
            this.profileService = new ProfileServiceProxy(this.$apiUrl, this.$api);
        },
        computed: {
            languages() {
                return abpService.abp.localization.languages
                    .filter(o => !o.isDisabled && o.name !== this.currentLanguage.name);
            },
            currentLanguage() {
                return abpService.abp.localization.currentLanguage;
            }
        },
        methods: {
            change(languageName) {
                const input = new ChangeUserLanguageDto();
                input.languageName = languageName;

                this.profileService.changeLanguage(input).then(() => {
                    abpService.abp.utils.setCookieValue(
                        'Abp.Localization.CultureName',
                        languageName,
                        new Date(new Date().getTime() + 5 * 365 * 86400000), // 5 year
                        abpService.abp.appPath
                    );

                    window.location.reload();
                });
            }
        }
    }
</script>

<style scoped>

</style>
