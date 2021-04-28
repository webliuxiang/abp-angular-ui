import localizationService from '../i18n/localization.service';
import abpService from '../abp/abp.service';
import apiHttpClient from '@/shared/utils/api-http-client';
import {AppConsts} from '@/abpPro/AppConsts';


const AppCompoentBase = {
    data() {
        return {
            lodingVal: false,
            savingVal: false
        }
    },
    computed: {
        $apiUrl() { // API 的 访问地址
            return AppConsts.remoteServiceBaseUrl;
        },
        $api() { // API 的 http 实例
            return apiHttpClient;
        },
        $notificationCount() {
            return AppConsts.notificationCount;
        },
        message() {
            return abpService.abp.message;
        },
        notify() {
            return abpService.abp.notify;
        },
        loading: {
            get() {
                return this.lodingVal;
            },
            set(val) {
                this.lodingVal = val;
            }
        },
        saving: {
            get() {
                return this.savingVal;
            },
            set(val) {
                this.savingVal = val;
            }
        }
    },
    methods: {
        l(key, ...args) {
            return localizationService.l(key, ...args);
        },
        isGranted(permissionName) {
            return abpService.abp.auth.isGranted(permissionName);
        },
        isGrantedAny(permissions) {
            if (!permissions) {
                return false;
            }
            for (const permission of permissions) {
                if (this.isGranted(permission)) {
                    return true;
                }
            }
            return false;
        }
    }
};

export default AppCompoentBase;

